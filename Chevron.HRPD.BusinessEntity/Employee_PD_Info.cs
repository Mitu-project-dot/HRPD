


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Chevron.HRPD.BusinessEntities
{
    [Serializable]
    public class Employee_PD_Info : BusinessEntity
    {

        public int Employee_Number { get; set; }
        public string Employee_Name { get; set; }
        public string CAI { get; set; }
        public string Supervisor_Name { get; set; }
        public string Supervisor_CAI { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string PD_Year { get; set; }
        public DateTime PD_Effective_Date { get; set; }
        public string Rating { get; set; }
        public string Old_PSG { get; set; }
        public string Old_Salary { get; set; }
        public string Old_CO { get; set; }
        public string New_PSG { get; set; }
        public string New_Salary { get; set; }
        public string New_CO { get; set; }
        public string Change_Reason { get; set; }
        public string Change_Percentage { get; set; }
        public string Change_Amount { get; set; }
        public string Eligable_Earning { get; set; }
        public string PSG_Target { get; set; }
        public string Corp_Rating { get; set; }
        public string Award_Unit_Rating { get; set; }
        public string IPF { get; set; }
        public string CIP { get; set; }

        public string Housing_Allowance_Old { get; set; }
        public string Housing_Allowance_New { get; set; }

        public string OutPt_Med_Allowance_old { get; set; }
        public string OutPt_Med_Allowance_new { get; set; }


        //  new enhancement of 2021
        public string Ind_Bonus_Comp { get; set; }
        public string Conveynance_allowance_old { get; set; }
        public string Conveynance_allowance_new { get; set; }
        public string LFA_Allowance_new { get; set; }
        public string LFA_Allowance_old { get; set; }

        public string Employee_type { get; set; }
        public string Allowance { get; set; }

        public string Total_CIP_Award { get; set; }

        //

        public string Create_By { get; set; }
        public DateTime? Create_Time { get; set; }
        public string Updated_By { get; set; }
        public DateTime? Updated_Time { get; set; }

        public bool? HR_Review { get; set; }
        public bool? Supervisor_Review { get; set; }       


        [NotMappedAttribute]
        [Display(Name = "Your name")]
        public string FromName { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Your email"), EmailAddress]
        public string FromEmail { get; set; }
        [NotMappedAttribute]
        public string Message { get; set; }

        [NotMappedAttribute]        
        public string DPTSHRT {

            get { return Department.Substring(0, 2).ToString(); }
        }

        public IEnumerable<SelectListItem> DeptList { get; set; }
        public IEnumerable<SelectListItem> SupervisorList { get; set; }
        //public IEnumerable<SelectListItem> CIF_CUST_CONCENT_LIST { get; set; }


        /* For Encrypted Data */





        #region  2021 enhanacement


        [NotMapped]
        public string New_PSG_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.New_PSG, true);
            }
            set
            {
                this.New_PSG = EncryptDecrypt.Encrypt(value, true);
            }
        }

        [NotMapped]
        public string Total_CIP_Award_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.Total_CIP_Award, true);
            }
            set
            {
                this.Total_CIP_Award = EncryptDecrypt.Encrypt(value, true);
            }
        }

        [NotMapped]
        public string Ind_Bonus_Comp_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.Ind_Bonus_Comp, true);
            }
            set
            {
                this.Ind_Bonus_Comp = EncryptDecrypt.Encrypt(value, true);
            }
        }


        [NotMapped]
        public string Conveynance_allowance_old_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.Conveynance_allowance_old, true);
            }
            set
            {
                this.Conveynance_allowance_old = EncryptDecrypt.Encrypt(value, true);
            }
        }

        [NotMapped]
        public string Conveynance_allowance_new_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.Conveynance_allowance_new, true);
            }
            set
            {
                this.Conveynance_allowance_new = EncryptDecrypt.Encrypt(value, true);
            }
        }


        [NotMapped]
        public string LFA_Allowance_old_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.LFA_Allowance_old, true);
            }
            set
            {
                this.LFA_Allowance_old = EncryptDecrypt.Encrypt(value, true);
            }
        }


        [NotMapped]
        public string LFA_Allowance_new_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.LFA_Allowance_new, true);
            }
            set
            {
                this.LFA_Allowance_new = EncryptDecrypt.Encrypt(value, true);
            }
        }


        #endregion

     
        [NotMapped]
        public string Old_Salary_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.Old_Salary, true);
            }
            set
            {
                this.Old_Salary = EncryptDecrypt.Encrypt(value, true);
            }
        }

        [NotMapped]
        public string New_Salary_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.New_Salary, true);
            }
            set
            {
                this.New_Salary = EncryptDecrypt.Encrypt(value, true);
            }
        }

      


        [NotMapped]
        public string Change_Percentage_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.Change_Percentage, true);
            }
            set
            {
                this.Change_Percentage = EncryptDecrypt.Encrypt(value, true);
            }
        }

        [NotMapped]
        public string Change_Amount_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.Change_Amount, true);
            }
            set
            {
                this.Change_Amount = EncryptDecrypt.Encrypt(value, true);
            }
        }

        [NotMapped]
        public string OutPt_Med_Allowance_old_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.OutPt_Med_Allowance_old, true);
            }
            set
            {
                this.OutPt_Med_Allowance_old = EncryptDecrypt.Encrypt(value, true);
            }
        }


        [NotMapped]
        public string OutPt_Med_Allowance_new_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.OutPt_Med_Allowance_new, true);
            }
            set
            {
                this.OutPt_Med_Allowance_new = EncryptDecrypt.Encrypt(value, true);
            }
        }





        [NotMapped]
        public string Housing_Allowance_Old_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.Housing_Allowance_Old, true);
            }
            set
            {
                this.Housing_Allowance_Old = EncryptDecrypt.Encrypt(value, true);
            }
        }


        [NotMapped]
        public string Housing_Allowance_New_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.Housing_Allowance_New, true);
            }
            set
            {
                this.Housing_Allowance_New = EncryptDecrypt.Encrypt(value, true);
            }
        }




        [NotMapped]
        public string Rating_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.Rating, true);
            }
            set
            {
                this.Rating = EncryptDecrypt.Encrypt(value, true);
            }
        }

        [NotMapped]
        public string Old_PSG_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.Old_PSG, true);
            }
            set
            {
                this.Old_PSG = EncryptDecrypt.Encrypt(value, true);
            }
        }



        [NotMapped]
        public string Old_CO_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.Old_CO, true);
            }
            set
            {
                this.Old_CO = EncryptDecrypt.Encrypt(value, true);
            }
        }



        [NotMapped]
        public string New_CO_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.New_CO, true);
            }
            set
            {
                this.New_CO = EncryptDecrypt.Encrypt(value, true);
            }
        }

        [NotMapped]
        public string Eligable_Earning_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.Eligable_Earning, true);
            }
            set
            {
                this.Eligable_Earning = EncryptDecrypt.Encrypt(value, true);
            }
        }


        [NotMapped]
        public string IPF_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.IPF, true);
            }
            set
            {
                this.IPF = EncryptDecrypt.Encrypt(value, true);
            }
        }

        [NotMapped]
        public string CIP_Decrypted
        {
            get
            {
                return EncryptDecrypt.Decrypt(this.CIP, true);
            }
            set
            {
                this.CIP = EncryptDecrypt.Encrypt(value, true);
            }
        }
    }
}
