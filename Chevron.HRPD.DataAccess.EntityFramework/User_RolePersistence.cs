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
    public class User_RolePersistence : BasePersistence<User_Role>, IUser_RolePersistence
    {
        public override User_Role FindByID(int ID)
        {
            using (BaseContext BC = new BaseContext())
            {
                return BC.User_Role_Context.Where(c => c.ID == ID).FirstOrDefault();
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

        public override void Update(User_Role entity)
        {
            using (BaseContext BContext = new BaseContext())
            {
                User_Role template = BContext.User_Role_Context.Where(c => c.ID == entity.ID).FirstOrDefault();

                template.CAI = entity.CAI;
               
                BContext.SaveChanges();

            }
        }

        public override void Save(User_Role entity)
        {
            using (BaseContext BContext = new BaseContext())
            {
                if (BContext.User_Role_Context.Any(u => u.ID == entity.ID))
                {
                    Update(entity);
                    return;
                }

                BContext.User_Role_Context.Add(entity);

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
                var Employee = BContext.User_Role_Context.Where(c => c.ID == ID).FirstOrDefault();
                if (Employee != null)
                {                    
                    BContext.Entry(Employee).State = System.Data.EntityState.Modified;
                    BContext.SaveChanges();
                }
            }
        }
    }
}
