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
    public class UserLogInLogPersistence : BasePersistence<UserLogInLog>, IUserLogInLogPersistence
    {
        public override UserLogInLog FindByID(int ID)
        {
            using (BaseContext BC = new BaseContext())
            {
                return BC.UserLogInLog_Context.Where(c => c.ID == ID).FirstOrDefault();
            }
        }

        //public SMSSendLog FindByLocationName(SMSSendLog entity)
        //{
        //    using (BaseContext BC = new BaseContext())
        //    {
        //        return BC.SMSSendLogContext.Where(c => c.Title == entity.Title).FirstOrDefault();
        //    }
        //}

        //public Location FindByCAI(int ID)
        //{
        //    using (BaseContext BC = new BaseContext())
        //    {
        //        return BC.Location_Context.Where(c => c.ID == ID).FirstOrDefault();
        //    }
        //}

        public override void Update(UserLogInLog entity)
        {
            using (BaseContext BContext = new BaseContext())
            {
                UserLogInLog template = BContext.UserLogInLog_Context.Where(c => c.ID == entity.ID).FirstOrDefault();

                template.CAI = entity.CAI;                
                template.LoginDate = entity.LoginDate;
                template.IPAddress = entity.IPAddress;
                template.SessionID = entity.SessionID;
                template.LoginTime = entity.LoginTime;
                template.LogoutTime = entity.LogoutTime;
                template.SessionLastHit = entity.SessionLastHit;
                template.SessionExpires = entity.SessionExpires;      
                
               
                BContext.SaveChanges();

            }
        }

        public override void Save(UserLogInLog entity)
        {
            using (BaseContext BContext = new BaseContext())
            {
                if (BContext.UserLogInLog_Context.Any(u => u.ID == entity.ID))
                {
                    Update(entity);
                    return;
                }

                BContext.UserLogInLog_Context.Add(entity);

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
                var Employee = BContext.UserLogInLog_Context.Where(c => c.ID == ID).FirstOrDefault();
                if (Employee != null)
                {
                    //Employee.IsActive = false;
                    BContext.Entry(Employee).State = System.Data.EntityState.Modified;
                    BContext.SaveChanges();
                }
            }
        }
    }
}
