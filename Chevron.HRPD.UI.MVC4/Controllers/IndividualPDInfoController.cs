
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
using Chevron.HRPD.UI.MVC4.Controllers;
using CrystalDecisions.CrystalReports.Engine;
using Chevron.HRPD.UI.MVC4.Common;
using Chevron.HRPD.Common.Helpers;

namespace Chevron.HROT.UI.MVC4.Controllers
{   
   [AuthorizeAD(Groups = ConstantsAD.HRPDEmployeeGroup)]
    public class IndividualPDInfoController : BaseController
    {
        Employee_PD_InfoComponent empComp = new Employee_PD_InfoComponent();

        public ActionResult Index()
        {
            //if (CurrentUserAD.GetCurrentUser().isHRPDEmployee)
            //{
                string CAI = CurrentUserAD.GetCurrentUser().CAI.ToString().Trim();

                //string CAI = "OGPA";
                List<Employee_PD_Info> empdetail = empComp.FindIndividualByCAI(CAI);


                foreach (var item in empdetail)
                {
                    if ((item.Supervisor_Review == null) || item.Supervisor_Review == Convert.ToBoolean(0))
                    {
                        return RedirectToAction("NotPermisible", "Error");
                    }
                }

                return View(empdetail);
            //}
            //else
            //{               
            //    return RedirectToAction("Unauthorized", "Error");
            //}
        }


        public ActionResult Create(string id)
        {

            string PD_Year = id;
            List<Employee_PD_Info> lstEmp = empComp.FindIndividualByCAI_PDYear(CurrentUserAD.GetCurrentUser().CAI.ToString().Trim(),PD_Year);
            
                ReportDocument rd = new ReportDocument();

                if (id == "2018")
                {


                    rd.Load(Server.MapPath("~/Reports/PDLetter.rpt"));
                    DataTable dt = new DataTable();

                    dt = ToDataTable<Employee_PD_Info>(lstEmp);

                    dt = formatData(dt);

                    rd.SetDataSource(dt);
                }
                else if (id == "2019")
                {

                    rd.Load(Server.MapPath("~/Reports/PDLetter2019.rpt"));
                    DataTable dt = new DataTable();

                    dt = ToDataTable<Employee_PD_Info>(lstEmp);

                    dt = formatData(dt);

                    rd.SetDataSource(dt);
                }

                else if (id == "2020")
                {
                    try
                    {

                        rd.Load(Server.MapPath("~/Reports/PDLetter2020.rpt"));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ReportFolder!!!" + ex.Message.ToString() + ex.InnerException.ToString());
                    }


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
                    //if (Props[i].Name.Contains("Decrypted"))
                    //{
                        
                    //    string iEncrypName = Props[i].Name.ToString();
                    //    int charLocation = iEncrypName.IndexOf("_Decrypted", StringComparison.Ordinal);
                    //    string iName = Props[i].Name.Substring(0, charLocation).ToString();

                        

                    //}
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
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

    }
}