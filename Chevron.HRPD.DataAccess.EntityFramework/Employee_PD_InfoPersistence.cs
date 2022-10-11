using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Chevron.HRPD.Common.Interfaces;
using Chevron.HRPD.BusinessEntities;
using Chevron.HRPD.DataAccess;

namespace Chevron.HRPD.DataAccess
{
    public class Employee_PD_InfoPersistence : BasePersistence<Employee_PD_Info>, IEmployee_PD_InfoPersistence
    {
        public override Employee_PD_Info FindByID(int ID)
        { 
            using (BaseContext BC = new BaseContext())
            {
                return BC.Employee_PD_Info_Context.Where(c => c.ID == ID).FirstOrDefault();
            }
        }



        public override void Update(Employee_PD_Info entity)
        {
            using (BaseContext BContext = new BaseContext())
            {
                Employee_PD_Info template = BContext.Employee_PD_Info_Context.Where(c => c.ID == entity.ID).FirstOrDefault();



                template.Employee_Number = entity.Employee_Number;
                template.Employee_Name = entity.Employee_Name;
                template.CAI = entity.CAI;
                template.Supervisor_Name = entity.Supervisor_Name;
                template.Supervisor_CAI = entity.Supervisor_CAI;
                template.Department = entity.Department;
                template.Designation = entity.Designation;
                template.PD_Year = entity.PD_Year;
                template.PD_Effective_Date = entity.PD_Effective_Date;
                template.New_PSG_Decrypted = entity.New_PSG_Decrypted;
                template.Old_Salary_Decrypted = entity.Old_Salary_Decrypted;
                template.New_Salary_Decrypted = entity.New_Salary_Decrypted;

                template.Change_Reason = entity.Change_Reason;
                template.Change_Percentage_Decrypted = entity.Change_Percentage_Decrypted;
                template.Change_Amount_Decrypted = entity.Change_Amount_Decrypted;

                template.Corp_Rating = entity.Corp_Rating;
                template.Award_Unit_Rating = entity.Award_Unit_Rating;


                template.HR_Review = entity.HR_Review;
                template.Supervisor_Review = entity.Supervisor_Review;
                template.Updated_By = entity.Updated_By;
                template.Updated_Time = DateTime.Now;

                // ****  changes in 2021 ****


                template.Ind_Bonus_Comp_Decrypted = entity.Ind_Bonus_Comp_Decrypted;
                template.Conveynance_allowance_old_Decrypted = entity.Conveynance_allowance_old_Decrypted;


                template.Conveynance_allowance_new_Decrypted = entity.Conveynance_allowance_new_Decrypted;
                template.LFA_Allowance_old_Decrypted = entity.LFA_Allowance_old_Decrypted;
                template.LFA_Allowance_new_Decrypted = entity.LFA_Allowance_new_Decrypted;
                template.Employee_type = entity.Employee_type;
                template.Allowance = entity.Allowance;
                template.Total_CIP_Award_Decrypted = entity.Total_CIP_Award_Decrypted;
                // ***** changes in 2021 ***


                //  template.Rating_Decrypted = entity.Rating_Decrypted;
                //  template.Old_PSG_Decrypted = entity.Old_PSG_Decrypted;



                // template.Old_CO_Decrypted = entity.Old_CO_Decrypted;
                //  template.New_PSG_Decrypted = entity.New_PSG_Decrypted;

                // template.New_CO_Decrypted = entity.New_CO_Decrypted;

                //  template.Eligable_Earning_Decrypted = entity.Eligable_Earning_Decrypted;
                //template.PSG_Target = entity.PSG_Target;

                //template.IPF_Decrypted = entity.IPF_Decrypted;
                //template.CIP_Decrypted = entity.CIP_Decrypted;

                //template.Housing_Allowance_Old = entity.Housing_Allowance_Old;
                //template.Housing_Allowance_New = entity.Housing_Allowance_New;
                //template.OutPt_Med_Allowance_old = entity.OutPt_Med_Allowance_old;
                //template.OutPt_Med_Allowance_new = entity.OutPt_Med_Allowance_new;





                BContext.SaveChanges();

            }
        }

        public override void Save(Employee_PD_Info entity)
        {
            using (BaseContext BContext = new BaseContext())
            {
                if (BContext.Employee_PD_Info_Context.Any(u => u.ID == entity.ID))
                {
                    Update(entity);
                    return;
                }

                BContext.Employee_PD_Info_Context.Add(entity);

                try
                {
                    BContext.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationerror in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property {0} Error: {1}", validationerror.PropertyName, validationerror.ErrorMessage);
                        }
                    }
                }
            }
        }

        public override void Delete(int ID)
        {
            using (BaseContext BContext = new BaseContext())
            {
                var entity = BContext.Employee_PD_Info_Context.Where(c => c.ID == ID).FirstOrDefault();
                if (entity != null)
                {                    
                    BContext.Entry(entity).State = System.Data.EntityState.Modified;
                    BContext.SaveChanges();
                }
            }
        }
    }
}
