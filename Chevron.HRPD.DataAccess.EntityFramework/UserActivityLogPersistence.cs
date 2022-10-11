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
    public class UserActivityLogPersistence : BasePersistence<UserActivityLog>, IUserActivityLogPersistence
    {
        public override UserActivityLog FindByID(int ID)
        {
            using (BaseContext BC = new BaseContext())
            {
                return BC.UserActivityLog_Context.Where(c => c.ID == ID).FirstOrDefault();
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

        public override void Update(UserActivityLog entity)
        {
            using (BaseContext BContext = new BaseContext())
            {
                UserActivityLog template = BContext.UserActivityLog_Context.Where(c => c.ID == entity.ID).FirstOrDefault();

                template.CAI = entity.CAI;
                template.SessionID = entity.SessionID;
                template.Remarks = entity.Remarks;
                               
               
                BContext.SaveChanges();

            }
        }

        public override void Save(UserActivityLog entity)
        {
            using (BaseContext BContext = new BaseContext())
            {
                if (BContext.UserActivityLog_Context.Any(u => u.ID == entity.ID))
                {
                    Update(entity);
                    return;
                }

                BContext.UserActivityLog_Context.Add(entity);

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
                var Employee = BContext.UserActivityLog_Context.Where(c => c.ID == ID).FirstOrDefault();
                if (Employee != null)
                {                    
                    BContext.Entry(Employee).State = System.Data.EntityState.Modified;
                    BContext.SaveChanges();
                }
            }
        }
    }
}
