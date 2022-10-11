using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Chevron.HRPD.BusinessEntities;
using Chevron.HRPD.UI.MVC4.Controllers;
using Chevron.HRPD.UI.MVC4.Common;
using Chevron.HRPD.Common.Helpers;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Reflection;
using Chevron.HRPD.BusinessComponent;
using System.Net.Mail;
using System.Text;
using System.Net;

namespace Chevron.HROT.UI.MVC4.Controllers
{    
  [AuthorizeAD(Groups = ConstantsAD.HRPDAdminGroup)]
    public class PDReportController : BaseController
    {
        private string strSearch = string.Empty;

        DataSet dsRpt = new DataSet();
        Employee_PD_InfoComponent pd = new Employee_PD_InfoComponent();


      
       
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            try
            {
                string iCAI = CurrentUserAD.GetCurrentUser().CAI.ToString().Trim();                              

                User_RoleComponent urc = new User_RoleComponent();
               
                User_Role ur = urc.FindByCAI(iCAI, "HRA");
                if (ur != null && ur.Role.Trim().ToUpper() == "HRA")
                {

                    Employee_PD_Info emp = new Employee_PD_Info();
                    var DeptList = GetDeptName();
                    emp.DeptList = DeptList;


                    TempData["Year"] = "2020"; // hard coded year
                    return View(emp);
                }
                else
                {
                    return RedirectToAction("Unauthorized", "Error");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Unauthorized", "Error");
            }
        }


         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Index(Employee_PD_Info pdlettergenerate)
         {
             List<Employee_PD_Info> listofPd = new List<Employee_PD_Info>();

             listofPd = pd.FindByParameter(pdlettergenerate);



             ReportDocument rd = new ReportDocument();
            if (pdlettergenerate.PD_Year == "2018")
            {

                TempData["Year"] = pdlettergenerate.PD_Year;

                rd.Load(Server.MapPath("~/Reports/PDLetter.rpt"));
                DataTable dt = new DataTable();

                dt = ToDataTable<Employee_PD_Info>(listofPd);

                dt = formatData(dt);

                rd.SetDataSource(dt);
            }
            else if (pdlettergenerate.PD_Year == "2019")
            {
                TempData["Year"] = pdlettergenerate.PD_Year;

                rd.Load(Server.MapPath("~/Reports/PDLetter2019.rpt"));
                DataTable dt = new DataTable();

                dt = ToDataTable<Employee_PD_Info>(listofPd);

                dt = formatData(dt);

                rd.SetDataSource(dt);
            }

            else

            {

                TempData["Year"] = pdlettergenerate.PD_Year;

                rd.Load(Server.MapPath("~/Reports/PDLetter2020.rpt"));

                DataTable dt = new DataTable();

                dt = ToDataTable<Employee_PD_Info>(listofPd);

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
                 rd.Dispose();
             }
         }

         public ActionResult HRReviewUpdate()
         {
             string employee_Number = string.Empty;
             try
             {
                 string PD_Year = "2020";
                 List<Employee_PD_Info> lstEmp = pd.FindYearwiseforHRReview(PD_Year);


                 foreach (var item in lstEmp)
                 {
                     employee_Number = item.Employee_Number.ToString();
                     item.HR_Review = true;
                     item.Updated_By = CurrentUserAD.GetCurrentUser().CAI.ToString().Trim();
                     pd.Persist(item);
                 }

                 TempData["Success"] = "HR Review Updated Successfully";

                 return RedirectToAction("Index", "PDReport");
             }
             catch (Exception Ex)
             {
                 TempData["Success"] = "Error: Review not completed. Check Employee Numbre:" + employee_Number + ", Exp:" + Ex.Message.ToString();

                 return RedirectToAction("Index", "PDReport");
             }
         }

         public ActionResult SendMail()
         {
             try
             {
                 Employee_PD_InfoComponent empc = new Employee_PD_InfoComponent();

                 string PD_Year = "2020";

                 List<Employee_PD_Info> emplist = empc.FindNoHRReviewList(PD_Year);
                 if (emplist.Count > 0)
                 {
                     TempData["Success"] = "HR Review Not completed for all. Please review it and send mail.";

                     return RedirectToAction("Index", "PDReport");
                 }


                 Employee_PD_Info Employee_PD_Info = new Employee_PD_Info();
                 var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                 var message = new MailMessage();

                 var emailList = pd.FindBySupervisorEmail();
                 var emails = String.Join(",", emailList);

                 message.Bcc.Add(emails);

                 message.From = new MailAddress("BPCPDT@chevron.com");  // replace with valid value               
                 message.IsBodyHtml = true;
                 message.BodyEncoding = UTF8Encoding.UTF8;

                 message.Subject = "Salary Action Letter";
                 message.Body =
                    "BPC Supervisors, <br>You should have received by now a notification on your direct reports salary change action in Workday indicating merit changes and bonus payment. In addition, find the below link capturing a more detailed statement indicating the changes to your direct report/s allowance/s, merit, and bonus respectively. <br> <a href='http://bd-test.chevron.com/mypdtest/SupervisorPanel'>Link</a> <br>You are requested to share both reports; detail and Workday statements with your direct reports by February 28, as all employees will be able to view the respective compensation changes in Workday on March 1, 2021."
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

                 TempData["Success"] = "Mail Sent Successfully";
                 return RedirectToAction("Index", "PDReport");
             }
             catch (Exception ex)
             {
                 TempData["Success"] = "Error: Mail not Sent. Exp:" + ex.Message.ToString();
                 return RedirectToAction("Index", "PDReport");
             }

         }


         public DataTable formatData(DataTable DTS)
         {
             try
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
             catch (Exception ex)
             {

             }
             return DTS;
         }

        public IEnumerable<SelectListItem> GetDeptName()
         {
             var selectList = new List<SelectListItem>();
             try
             {
                 List<string> elements = new List<string>();

                 elements = pd.FindAllDeptName();

                 foreach (var element in elements)
                 {
                     selectList.Add(new SelectListItem
                     {
                         Value = element.ToString(),
                         Text = element.ToString()
                     });
                 }

                 elements = null;

                 return selectList;
             }
             catch (Exception Ex)
             {

             }
             return selectList;
         }

         [HttpPost]
         public ActionResult GetSupervisorName(string DepartmentName)
         {
             var selectList = new List<SelectListItem>();
             try
             {
                 List<SelectListItem> SupervisorNames = new List<SelectListItem>();
                 List<string[]> elements = new List<string[]>();

                 elements = pd.GetSupervisorName(DepartmentName);

                 foreach (var element in elements)
                 {
                     selectList.Add(new SelectListItem
                     {
                         Value = element[1].ToString(),
                         Text = element[0].ToString()
                     });
                 }
                 elements = null;
             }
             catch (Exception ex)
             {

             }
             return Json(selectList, JsonRequestBehavior.AllowGet);
         }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            try
            {
                
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
            catch (Exception ex)
            {
 
            }
            return dataTable;
        }


        

    
    }        
}
