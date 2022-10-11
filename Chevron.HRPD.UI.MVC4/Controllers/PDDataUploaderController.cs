using Chevron.HRPD.BusinessComponent;
using Chevron.HRPD.BusinessEntities;
using Chevron.HRPD.Common.Helpers;
using Chevron.HRPD.DataAccess;
using Chevron.HRPD.UI.MVC4.Common;
using Chevron.HRPD.UI.MVC4.Controllers;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chevron.HRPD.UI.MVC4.Controllers
{    
 [AuthorizeAD(Groups = ConstantsAD.HRPDAdminGroup)]
    public class PDDataUploaderController : BaseController
    {
       Employee_PD_InfoComponent empComp = new Employee_PD_InfoComponent();


        public ActionResult Index()
        {
            try
            {
                string iCAI = CurrentUserAD.GetCurrentUser().CAI.ToString().Trim();


                User_RoleComponent urc = new User_RoleComponent();
               
                User_Role ur = urc.FindByCAI(iCAI, "HRA");
                if (ur != null && ur.Role.Trim().ToUpper() == "HRA")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Unauthorized", "Error");
                }
            }
            catch (Exception Ex)
            {
                return RedirectToAction("Unauthorized", "Error");
            }
        }

      

        public ActionResult Details(int id)
        {
            return View();
        }



        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            try
            {
                string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);

                //string filepath = "/excelfolder/" + filename;

                string fileLocation = Path.Combine(Server.MapPath("~/excelData"));

                file.SaveAs(Path.Combine(fileLocation, filename));

                InsertExceldata(fileLocation, filename);

                /*
                 To Delete all excel file from directory
                 */

                string[] filePaths = Directory.GetFiles(fileLocation);
                foreach (string filePath in filePaths)
                    System.IO.File.Delete(filePath);


                TempData["Success"] = "Data uploaded Successfully";

                return View();
            }
            catch (Exception ex)
            {
                TempData["Success"] = "Error!!! Not Uploaded. " + ex.Message.ToString();
            }

            return View();

        }

        private void InsertExceldata(string fileepath, string filename)
        {
            //Server.MapPath("~/Reports/PDLetter.rpt")
            try
            {

                string fullpath = fileepath + "\\" + filename;


                string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", fullpath);

                string query = string.Format("Select * from [{0}]", "ALL$");

                var adapter = new OleDbDataAdapter(query, constr);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "mypd");


                DataTable dt = ds.Tables[0];

                Employee_PD_InfoComponent empc = new Employee_PD_InfoComponent();

                foreach (DataRow dr in dt.Rows)
                {
                    Employee_PD_Info emp = new Employee_PD_Info();
                    string a = dr["Employee Number"].ToString();

                    if (a == "")
                    {
                        break;
                    }
                    else
                    {
                        emp = empc.Find().Where(c => (c.Employee_Number == Convert.ToInt32(dr["Employee Number"].ToString())) && (c.PD_Year == dr["PD Year"].ToString())).FirstOrDefault();


                        if (emp == null)
                            emp = new Employee_PD_Info();

                        emp.Employee_Number = Convert.ToInt32(dr["Employee Number"].ToString());
                        emp.Employee_Name = dr["Name"].ToString();
                        emp.CAI = dr["CAI"].ToString();
                        emp.Supervisor_Name = dr["Supervisor"].ToString();
                        emp.Supervisor_CAI = dr["Supervisor CAI"].ToString();
                        emp.Department = dr["Department"].ToString();
                        emp.Designation = dr["Designation"].ToString();
                        emp.PD_Year = dr["PD Year"].ToString();
                        emp.PD_Effective_Date = Convert.ToDateTime(dr["PD Eff Date"].ToString());

                        emp.New_PSG_Decrypted = dr["New PSG"].ToString();
                        emp.Old_Salary_Decrypted = dr["Old Salary"].ToString();
                        emp.New_Salary_Decrypted = dr["New Salary"].ToString();

                        emp.Change_Reason = dr["Change Reason"].ToString();
                        emp.Change_Percentage_Decrypted = dr["Change %"].ToString();
                        emp.Change_Amount_Decrypted = dr["Change Amount"].ToString();

                        emp.Corp_Rating = dr["Corp Rating"].ToString();
                        emp.Award_Unit_Rating = dr["Award Unit Rating"].ToString();

                        emp.Create_By = CurrentUserAD.GetCurrentUser().CAI.ToString().Trim();
                        emp.Create_Time = DateTime.Now;
                        emp.Updated_By = CurrentUserAD.GetCurrentUser().CAI.ToString().Trim();
                        emp.Updated_Time = DateTime.Now;

                        // *** changes in 2021 ****

                        emp.Ind_Bonus_Comp_Decrypted = dr["Individual Bonus"].ToString();

                        emp.Conveynance_allowance_old_Decrypted = dr["Conv allowance Old"].ToString();
                        emp.Conveynance_allowance_new_Decrypted = dr["Conv allowance new"].ToString();

                        emp.LFA_Allowance_old_Decrypted = dr["LFA Old"].ToString();
                        emp.LFA_Allowance_new_Decrypted = dr["LFA New"].ToString();
                        emp.Employee_type = dr["Employee type"].ToString();
                        emp.Allowance = dr["Allowance"].ToString();
                        emp.Total_CIP_Award_Decrypted = dr["Total CIP Award"].ToString();

                        // ***** changes in 2021 ****
                       
                       

                        // comment out for 2021
                     
                        emp.Old_CO_Decrypted = "1";
                        emp.New_CO_Decrypted = "1";
                        emp.Rating_Decrypted = "1";
                        emp.Old_PSG_Decrypted = "1";
                        
                        emp.Eligable_Earning_Decrypted = "1";
                        emp.CIP_Decrypted = "1";
                        emp.Housing_Allowance_Old = "1";
                        emp.Housing_Allowance_New = "1";
                        emp.OutPt_Med_Allowance_old = "1";
                        emp.OutPt_Med_Allowance_new = "1";
                        emp.Old_CO_Decrypted = "1"; 
                      
                        emp.Eligable_Earning_Decrypted = "1";
                        emp.PSG_Target = "1";
                        emp.IPF_Decrypted = "1";
                        emp.CIP_Decrypted = "1";
                        emp.Housing_Allowance_Old_Decrypted = "1";
                        emp.Housing_Allowance_New_Decrypted = "1";
                        emp.OutPt_Med_Allowance_old_Decrypted = "1";
                        emp.OutPt_Med_Allowance_new_Decrypted = "1";

                        empc.Persist(emp);

                        

                    }

                }
                  
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public JsonResult jsDeleteAllData()
        {
           
            string messageresul = string.Empty;
            Employee_PD_InfoComponent epc = new Employee_PD_InfoComponent();

            try
            {
                using (var context = new BaseContext())
                {

                    List<Employee_PD_Info> list = new List<Employee_PD_Info>();
                    list = context.Employee_PD_Info_Context.Where(c => c.PD_Year == "2020").ToList(); // hard coded date
                    foreach (var item in list)
                    {
                        context.Employee_PD_Info_Context.Remove(item);
                        context.SaveChanges();
                    }

                    messageresul = "Data Deleted Successfully.";

                }
            }
            catch (Exception Ex)
            {
                messageresul = "Error!! Data not deleted. Ex:" + Ex.Message.ToString();
            }

            return Json(messageresul,JsonRequestBehavior.AllowGet);
        }
    }
}
