using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;
using Chevron.HRPD.BusinessComponent;
using Chevron.HRPD.BusinessEntities;
using CrystalDecisions.CrystalReports.Engine;
using Chevron.HRPD.UI.MVC4.Common;
using Chevron.HRPD.Common.Helpers;


namespace Chevron.HRPD.UI.MVC4.Controllers
{    
  [AuthorizeAD(Groups = ConstantsAD.HRPDSupervisorGroup)]
    public class SupervisorPanelController : BaseController
    {
        Employee_PD_InfoComponent empComp = new Employee_PD_InfoComponent();



        public ActionResult Index(string PD_Year)
        {
            try
            {

                string iCAI = CurrentUserAD.GetCurrentUser().CAI.ToString().Trim();

                User_RoleComponent urc = new User_RoleComponent();
                User_Role ur = urc.FindByCAI(iCAI, "SPV");

                if (ur != null && ur.Role.Trim().ToUpper() == "SPV")
                {
                    List<Employee_PD_Info> lstEmp = new List<Employee_PD_Info>();

                    if (PD_Year == null)
                    {
                        PD_Year = "2020";
                        TempData["Year"] = PD_Year;
                        
                        lstEmp = empComp.ShowAllReportForDirectSupervisor1(iCAI, PD_Year);
                    }
                    else
                    {
                        lstEmp = empComp.ShowAllReportForDirectSupervisor1(iCAI, PD_Year);
                        TempData["Year"] = PD_Year;
                    }

                    foreach (var item in lstEmp)
                    {
                        if ((item.HR_Review == null) || item.HR_Review == Convert.ToBoolean(0))
                        {
                            return RedirectToAction("NotPermisible", "Error");
                        }
                    }

                    return View(lstEmp);
                }
                else
                    return RedirectToAction("Unauthorized", "Error");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Unauthorized", "Error");
            }
        }


        public ActionResult AllReportForDirectSupervisor()
        {

           


            string SupervisorCAI = CurrentUserAD.GetCurrentUser().CAI.ToString().Trim();
            string year = TempData["Year"].ToString();
            List<Employee_PD_Info> lstEmp = new List<Employee_PD_Info>();
            ReportDocument rd = new ReportDocument();

            if (year == "2018")
            {
                lstEmp = empComp.ShowAllReportForDirectSupervisor(SupervisorCAI);

                rd.Load(Server.MapPath("~/Reports/PDLetter.rpt"));
                DataTable dt = new DataTable();

                dt = ToDataTable<Employee_PD_Info>(lstEmp);

                dt = formatData(dt);

                rd.SetDataSource(dt);
            }
            else if (year == "2019")

            {
                lstEmp = empComp.ShowAllReportForDirectSupervisor(SupervisorCAI);
                rd.Load(Server.MapPath("~/Reports/PDLetter2019.rpt"));
                DataTable dt = new DataTable();

                dt = ToDataTable<Employee_PD_Info>(lstEmp);

                dt = formatData(dt);

                rd.SetDataSource(dt);
            }
            else

            {

                lstEmp = empComp.ShowAllReportForDirectSupervisor(SupervisorCAI);
                rd.Load(Server.MapPath("~/Reports/PDLetter2020.rpt"));
                DataTable dt = new DataTable();

                dt = ToDataTable<Employee_PD_Info>(lstEmp);

                dt = formatData(dt);

                rd.SetDataSource(dt);
            }
         
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                //rd.Close();
                return File(stream, "application/pdf", "PDLetter.pdf");
            }
            catch (Exception ex)
            {
                throw;
            }

            finally
            {
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Clear();
                rd.Close();
            }

            return View();
        }



        public DataTable formatData(DataTable DTS)
        {

            foreach (DataRow dr in DTS.Rows)
            {
                foreach (DataColumn dc in DTS.Columns)
                {
                    if (dc.ToString().Contains("Decrypted"))
                    {
                        string iEncrypColName = dc.ToString();
                        int charLocation = iEncrypColName.IndexOf("_Decrypted", StringComparison.Ordinal);
                        string iColName = iEncrypColName.Substring(0, charLocation).ToString();
                        dr[iColName] = dr[dc].ToString();
                    }
                }
            }

            return DTS;
        }
                 

        public ActionResult SupervisorMailToEmployee()
        {
            Employee_PD_InfoComponent empc = new Employee_PD_InfoComponent();

            string SupervisorCAI = CurrentUserAD.GetCurrentUser().CAI.ToString().Trim();
            string PD_Year = "2020";

            List<Employee_PD_Info> emplist = empc.FindSupervisorRFeviewforAlldirectreport(SupervisorCAI, PD_Year);
            if (emplist.Count > 0)
            {
                TempData["Success"] = "Supervisor Review Not completed for all. Please review it and send mail.";

                return RedirectToAction("Index", "SupervisorPanel");
            }


            Employee_PD_Info Employee_PD_Info = new Employee_PD_Info();            
            var message = new System.Net.Mail.MailMessage();

           
            string supervisorCAI = CurrentUserAD.GetCurrentUser().CAI.ToString().Trim();

            var emailList = empComp.SupervisorMailToEmployee(supervisorCAI);
            var emails = String.Join(",", emailList);
         
            message.Bcc.Add(emails);         

            message.From = new MailAddress("BPCPDT@chevron.com");  // replace with valid value
            message.IsBodyHtml = true;
            message.BodyEncoding = UTF8Encoding.UTF8;

            message.Subject = "Salary Action Letter";
            message.Body = "BPC Employees, <br>The pay change statement in the link below captures a more detailed documentation of the changes to your allowance/s, merit, and bonus respectively in addition to the pay change statement through Workday which contains changes to your merit and bonus.<br> <a href='http://bd-test.chevron.com/mypdtest/IndividualPDInfo'>Link</a><br>You will be able to view the respective compensation changes in Workday on March 1, 2021."
                            + "<br>Business HR will be available to provide further guidance and assist in answering any questions you may have.";


           
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {

                    UserName = "BPCPDT@chevron.com",  // replace with valid value


                };
                smtp.Credentials = credential;
                smtp.UseDefaultCredentials = false;

                smtp.Host = "sghq.mox-mx.chevron.net";


                smtp.Port = 25;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Timeout = 100000;


                smtp.Send(message);
            }
            TempData["Success"] = "Mail Sent successfully";

            return RedirectToAction("Index", "SupervisorPanel");

        }

        public ActionResult SupervisorMailToIndividual(string id, string PD_Year)
        {
            try
            {

                Employee_PD_Info emp = empComp.Find().Where(c => (c.CAI == id && c.PD_Year == PD_Year && c.Supervisor_CAI == CurrentUserAD.GetCurrentUser().CAI.ToString().Trim())).FirstOrDefault();

               

                if (emp.Supervisor_Review!= true)
                { 
                    emp.Supervisor_Review = true;
                    emp.Updated_By = CurrentUserAD.GetCurrentUser().CAI.ToString().Trim();
                    empComp.Persist(emp);

                    var message = new System.Net.Mail.MailMessage();
                    string indEmp = id + "@chevron.com";

                    message.Bcc.Add(new MailAddress(indEmp));

                    message.From = new MailAddress("BPCPDT@chevron.com");  // replace with valid value              
                    message.IsBodyHtml = true;
                    message.BodyEncoding = UTF8Encoding.UTF8;

                    message.Subject = "Salary Action Letter";
                    message.Body = "BPC Employees, <br>The pay change statement in the link below captures a more detailed documentation of the changes to your allowance/s, merit, and bonus respectively in addition to the pay change statement through Workday which contains changes to your merit and bonus.<br> <a href='http://bd-test.chevron.com/mypdtest/IndividualPDInfo'>Link</a><br>You will be able to view the respective compensation changes in Workday on March 1, 2021."
                              + "<br>Business HR will be available to provide further guidance and assist in answering any questions you may have.";



                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {


                            UserName = "BPCPDT@chevron.com",  // replace with valid value

                        };
                        smtp.Credentials = credential;
                        smtp.UseDefaultCredentials = false;

                        smtp.Host = "sghq.mox-mx.chevron.net";


                        smtp.Port = 25;
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                        smtp.Timeout = 100000;


                        smtp.Send(message);


                    }


                    TempData["Success"] = "Review and Mail Sent Successfully";
                    return RedirectToAction("Index", "SupervisorPanel");
           
                  
                }
                
                 else
                {


                    TempData["Success"] = "Already reviewed !!";
                    return RedirectToAction("Index", "SupervisorPanel");
                }
                

               
               
                
            
            }
            catch (Exception ex)
            {
                TempData["Success"] = "Error: Main not Send. Exp: "+ex.Message.ToString();
                return RedirectToAction("Index", "SupervisorPanel");
            }

        }

        public ActionResult SupervisorReviewUpdate()
        {
            string SupervisorCAI = CurrentUserAD.GetCurrentUser().CAI.ToString().Trim();

            //string SupervisorCAI = "OGPA";
            List<Employee_PD_Info> lstEmp = empComp.ShowAllReportForDirectSupervisor(SupervisorCAI);

            foreach (var item in lstEmp)
            {
                item.Supervisor_Review = true;
                empComp.Persist(item);
            }

            TempData["Success"] = "Supervisor Review Done";
            return RedirectToAction("Index", "SupervisorPanel");        
        }

        public ActionResult Create(string id,string PD_Year)
        {
          
            // individual employee letter for supervisor check

            List<Employee_PD_Info> lstEmp = empComp.FindByEmployeeCAIforSupervisorcheck(id, CurrentUserAD.GetCurrentUser().CAI.ToString().Trim(), PD_Year);

            ReportDocument rd = new ReportDocument();
            if (PD_Year == "2018")
            {


                rd.Load(Server.MapPath("~/Reports/PDLetter.rpt"));
                DataTable dt = new DataTable();

                dt = ToDataTable<Employee_PD_Info>(lstEmp);

                dt = formatData(dt);

                rd.SetDataSource(dt);
            }
            else if (PD_Year == "2019")
            {

                rd.Load(Server.MapPath("~/Reports/PDLetter2019.rpt"));
                DataTable dt = new DataTable();

                dt = ToDataTable<Employee_PD_Info>(lstEmp);

                dt = formatData(dt);

                rd.SetDataSource(dt);
            }

             else 
            {

                rd.Load(Server.MapPath("~/Reports/PDLetter2020.rpt"));
                DataTable dt = new DataTable();

                dt = ToDataTable<Employee_PD_Info>(lstEmp);

                dt = formatData(dt);

                rd.SetDataSource(dt);
            }

            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "PDLetter.pdf");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Clear();
                rd.Close();
            }

            return View();
        }



        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
