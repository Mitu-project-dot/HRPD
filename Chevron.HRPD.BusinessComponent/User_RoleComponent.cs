


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

namespace Chevron.HRPD.BusinessComponent
{
    public class User_RoleComponent : BusinessComponent<User_Role>
    {
        #region Methods

        protected override void DoInsert(User_Role entity, TransactionWrapper transaction)
        {
            User_RolePersistence.Save(entity);
        }

        protected override void DoUpdate(User_Role entity, TransactionWrapper transaction)
        {
            User_RolePersistence.Update(entity);
        }

        public override void Delete(User_Role entity)
        {
            User_RolePersistence.Delete(entity);
        }

        public override ValidationResults Validate(User_Role entity)
        {
            return base.Validate(entity);
        }

        public override void Delete(int ID)
        {
            base.Delete(ID);
        }

        public override List<User_Role> Find()
        {
            List<User_Role> ListEntity = base.Find().ToList();
            return ListEntity;
        }

        public override User_Role FindByID(int ID)
        {
            User_Role entity = base.FindByID(ID);
            return entity;
        }

        public User_Role FindByCAI(string iCAI,string iRole)
        {
            User_Role entity = base.Find().Where(x => x.CAI.Trim().ToUpper() == iCAI.Trim().ToUpper() && x.Role.Trim().ToUpper()==iRole.Trim().ToUpper()).FirstOrDefault();
            return entity;
        }

        #endregion


        #region Properties

        protected IUser_RolePersistence User_RolePersistence
        {
            get
            {
                return UnityContainerHelper.Resolve<IUser_RolePersistence>();
            }
        }
        #endregion
    }
}