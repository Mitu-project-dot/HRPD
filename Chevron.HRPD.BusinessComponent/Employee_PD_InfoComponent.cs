


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chevron.HRPD.BusinessComponent;
using Chevron.HRPD.BusinessEntities;
using Chevron.HRPD.Common.Helpers;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Chevron.HRPD.Common.Interfaces;
using Chevron.HRPD.Common;
using System.Web.Mvc;

namespace Chevron.HRPD.BusinessComponent
{
    public class Employee_PD_InfoComponent : BusinessComponent<Employee_PD_Info>
    {
        #region Methods

        protected override void DoInsert(Employee_PD_Info entity, TransactionWrapper transaction)
        {
            Employee_PD_InfoPersistence.Save(entity);
        }

        protected override void DoUpdate(Employee_PD_Info entity, TransactionWrapper transaction)
        {
            Employee_PD_InfoPersistence.Update(entity);
        }

        public override void Delete(Employee_PD_Info entity)
        {
            Employee_PD_InfoPersistence.Delete(entity);
        }


        public void DeleteAll()
        {
            //var all = from c in base.Find() select c;

            //base.Find().RemoveRange(all);
            //base.Find().SaveChanges();



            
        }

        public override ValidationResults Validate(Employee_PD_Info entity)
        {
            return base.Validate(entity);
        }

        public override void Delete(int ID)
        {
            base.Delete(ID);
        }


        public List<Employee_PD_Info> FindAllDataYEarwise(string PD_Year)
        {

            List<Employee_PD_Info> ListUser = base.Find().Where(c => c.PD_Year == PD_Year).ToList();
            return ListUser;
        }
        
        public  List<Employee_PD_Info> FindYearwiseforHRReview(string PD_Year)
        {

            List<Employee_PD_Info> ListUser = base.Find().Where(c => c.PD_Year == PD_Year).ToList();
            return ListUser;
        }

        public List<Employee_PD_Info> FindNoHRReviewList(string PD_Year)
        {

            List<Employee_PD_Info> ListUserActivityLog = base.Find().Where(c => c.HR_Review == null && c.PD_Year == PD_Year).ToList();
            return ListUserActivityLog;
        }
        public List<Employee_PD_Info> FindSupervisorRFeviewforAlldirectreport( string SupervisorCAI, string PD_Year)
        {

            List<Employee_PD_Info> ListUser = base.Find().Where(x => (!string.IsNullOrEmpty(x.Supervisor_CAI) && x.Supervisor_CAI.Trim().ToUpper() == SupervisorCAI.Trim().ToUpper()) && x.Supervisor_Review == null && x.PD_Year== PD_Year).ToList();
            return ListUser;
        }

        public List<Employee_PD_Info> FindNoSupervisorReviewList()
        {

            List<Employee_PD_Info> ListUserActivityLog = base.Find().Where(c => c.Supervisor_Review == null).ToList();
            return ListUserActivityLog;
        }

        public List<Employee_PD_Info> FindIndividualByCAI(string CAI)
        {
            List<Employee_PD_Info> userdetail = base.Find().Where(x => !string.IsNullOrEmpty(x.CAI) && x.CAI.Trim().ToUpper() == CAI.Trim().ToUpper()).OrderByDescending(x => x.PD_Year).ToList();
            return userdetail;
        }

        public List<Employee_PD_Info> FindIndividualByCAI_PDYear(string CAI,  string PD_Year)
        {
            List<Employee_PD_Info> userdetail = base.Find().Where(x => !string.IsNullOrEmpty(x.CAI) && x.CAI.Trim().ToUpper() == CAI.Trim().ToUpper() && !string.IsNullOrEmpty(x.PD_Year) && x.PD_Year.Trim().ToUpper() == PD_Year.Trim().ToUpper()).OrderByDescending(x => x.PD_Year).ToList();
            return userdetail;
        }

        public override Employee_PD_Info FindByID(int ID)
        {
            Employee_PD_Info SMSSendinoentity = base.FindByID(ID);
            return SMSSendinoentity;
        }

        //public List<Employee_PD_Info> FindByParameter(Employee_PD_Info pdlettergenerate)
        //{

        //    List<Employee_PD_Info> listPD = base.Find().Where(x => ((pdlettergenerate.Supervisor_CAI == null) || (!string.IsNullOrEmpty(pdlettergenerate.Supervisor_CAI) && x.Supervisor_CAI.Trim().ToUpper() == pdlettergenerate.Supervisor_CAI.Trim().ToUpper()))
        //     && ((pdlettergenerate.PD_Year == null) || (!string.IsNullOrEmpty(pdlettergenerate.PD_Year) && x.PD_Year.Trim().ToUpper() == pdlettergenerate.PD_Year.Trim().ToUpper()))
        //        && ((pdlettergenerate.Department == null) || (!string.IsNullOrEmpty(pdlettergenerate.Department) && x.Department.Trim().ToUpper() == pdlettergenerate.Department.Trim().ToUpper()))
        //     && ((pdlettergenerate.Old_PSG == null) || (!string.IsNullOrEmpty(pdlettergenerate.Old_PSG) && x.Old_PSG_Decrypted.Trim().Trim().ToUpper() == pdlettergenerate.Old_PSG.Trim().ToUpper()))
        //      && ((pdlettergenerate.New_PSG == null) || (!string.IsNullOrEmpty(pdlettergenerate.New_PSG) && x.New_PSG_Decrypted.Trim().Trim().ToUpper() == pdlettergenerate.New_PSG.Trim().ToUpper()))
        // && ((pdlettergenerate.Rating == null) || (!string.IsNullOrEmpty(pdlettergenerate.Rating) && x.Rating_Decrypted.Trim().ToUpper() == pdlettergenerate.Rating.Trim().ToUpper()))).OrderBy(x => x.Supervisor_Name).ThenBy(c=>c.Employee_Name).ToList();

        //    return listPD;
        //}

        public List<Employee_PD_Info> FindByParameter(Employee_PD_Info pdlettergenerate)
        {

            List<Employee_PD_Info> listPD = base.Find().Where(x => ((pdlettergenerate.Supervisor_CAI == null) || (!string.IsNullOrEmpty(pdlettergenerate.Supervisor_CAI) && x.Supervisor_CAI.Trim().ToUpper() == pdlettergenerate.Supervisor_CAI.Trim().ToUpper()))
           
            && ((pdlettergenerate.PD_Year == null) || (!string.IsNullOrEmpty(pdlettergenerate.PD_Year) && x.PD_Year.Trim().ToUpper() == pdlettergenerate.PD_Year.Trim().ToUpper()))
              
            && ((pdlettergenerate.Department == null) || (!string.IsNullOrEmpty(pdlettergenerate.Department) && x.Department.Trim().ToUpper() == pdlettergenerate.Department.Trim().ToUpper()))).OrderBy(x => x.Supervisor_Name).ThenBy(c => c.Employee_Name).ToList();
            
            return listPD;
        }


        public List<Employee_PD_Info> FindByParameter()
        {
            List<Employee_PD_Info> listPD = base.Find().ToList();
            return listPD;
        }

        public List<Employee_PD_Info> ShowAllReportForDirectSupervisor(string SupervisorCAI)
        {

            List<Employee_PD_Info> EmpPD = base.Find().Where(x => (!string.IsNullOrEmpty(x.Supervisor_CAI) && x.Supervisor_CAI.Trim().ToUpper() == SupervisorCAI.Trim().ToUpper())  && x.PD_Year.Trim().ToUpper() == "2020").ToList();

            return EmpPD;
        }

        public List<Employee_PD_Info> ShowAllReportForDirectSupervisor1(string SupervisorCAI, string PD_Year)
        {

            List<Employee_PD_Info> EmpPD = base.Find().Where(x => (!string.IsNullOrEmpty(x.Supervisor_CAI) && x.Supervisor_CAI.Trim().ToUpper() == SupervisorCAI.Trim().ToUpper()) && (!string.IsNullOrEmpty(x.PD_Year) && x.PD_Year.Trim().ToUpper() == PD_Year.Trim().ToUpper())).ToList();

            return EmpPD;
        }

        public List<Employee_PD_Info> FindByEmployeeCAIforSupervisorcheck(string CAI, string SupervisorCAI, string PD_Year)
        {
            List<Employee_PD_Info> EmpPD = base.Find().Where(x => ((x.CAI.ToString().Trim().ToUpper() == CAI.ToString().Trim().ToUpper()) && (x.Supervisor_CAI.ToString().Trim().ToUpper() == SupervisorCAI.ToString().Trim().ToUpper()) && (x.PD_Year.ToString().Trim().ToUpper() == PD_Year.ToString().Trim().ToUpper()))).ToList();

            return EmpPD;

        }       

        public List<string> FindBySupervisorEmail()
        {
            var listPD = base.Find().Where(x => x.PD_Year == "2020").Select(u => u.Supervisor_CAI + "@chevron.com").Distinct().ToList();
            
            return listPD;
        }

        public List<string> SupervisorMailToEmployee(string supervisorCAI)
        {
            
            var listEmployee1 = base.Find().Where(x => x.Supervisor_CAI == supervisorCAI).Select(x => x.CAI + "@chevron.com").Distinct().ToList();                          

            return listEmployee1;
        }


        public List<string> SupervisorMailToIndividual(string supervisorCAI)
        {

            var listEmployee1 = base.Find().Where(x => x.Supervisor_CAI == supervisorCAI).Select(x => x.CAI).ToList();

            return listEmployee1;
        }



        public List<string> FindAllDeptName()
        {
           
            var Deptlist1 = base.Find().Where(x => x.PD_Year == "2020").Select(u =>  u.Department ).Distinct().ToList();
           
            return Deptlist1;
        }        


        public List<string[]> GetSupervisorName(string iDepartment)
         {
       
            var SupervisorList = (from c in base.Find()
                            where c.Department == iDepartment && c.PD_Year == "2020"
                             
                            group c by new { c.Supervisor_Name, c.Supervisor_CAI } into grp
                            select new[]
                       {
                           grp.Key.Supervisor_Name,
                           grp.Key.Supervisor_CAI
                       }).Distinct().ToList();


            return SupervisorList;
        }


      
        #endregion


        #region Properties

        protected IEmployee_PD_InfoPersistence Employee_PD_InfoPersistence
        {
            get
            {
                return UnityContainerHelper.Resolve<IEmployee_PD_InfoPersistence>();
            }
        }


     
}
        #endregion
    
}