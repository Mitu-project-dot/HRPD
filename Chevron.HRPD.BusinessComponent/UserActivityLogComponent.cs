


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
    public class UserActivityLogComponent : BusinessComponent<UserActivityLog>
    {
        #region Methods

        protected override void DoInsert(UserActivityLog entity, TransactionWrapper transaction)
        {
            UserActivityLogPersistence.Save(entity);
        }

        protected override void DoUpdate(UserActivityLog entity, TransactionWrapper transaction)
        {
            UserActivityLogPersistence.Update(entity);
        }

        public override void Delete(UserActivityLog entity)
        {
            UserActivityLogPersistence.Delete(entity);
        }

        public override ValidationResults Validate(UserActivityLog entity)
        {
            return base.Validate(entity);
        }

        public override void Delete(int ID)
        {
            base.Delete(ID);
        }

        public override List<UserActivityLog> Find()
        {
            //if (CurrentUserAD.GetCurrentUser().isSiteAdmin)
            //    return SMSTemplatePersistence.Find().ToList();
            //else
            //{
            //    string[] listLoc = CurrentUserAD.GetCurrentUser().Admin_Location_Permission_List;
            //    return SMSTemplatePersistence.Find().Where(c => (c.Created_By.ToString().CompareTo(CurrentUserAD.GetCurrentUser().CAI.ToString()) == 0)).ToList();
            //}

            List<UserActivityLog> ListUserActivityLog = base.Find().ToList();
            return ListUserActivityLog;
        }

        public override UserActivityLog FindByID(int ID)
        {
            UserActivityLog SMSSendinoentity = base.FindByID(ID);
            return SMSSendinoentity;
        }

        #endregion


        #region Properties

        protected IUserActivityLogPersistence UserActivityLogPersistence
        {
            get
            {
                return UnityContainerHelper.Resolve<IUserActivityLogPersistence>();
            }
        }
        #endregion
    }
}