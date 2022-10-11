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
    public class UserLogInLogComponent : BusinessComponent<UserLogInLog>
    {
        #region Methods

        protected override void DoInsert(UserLogInLog entity, TransactionWrapper transaction)
        {
            UserLogInLogPersistence.Save(entity);
        }

        protected override void DoUpdate(UserLogInLog entity, TransactionWrapper transaction)
        {
            UserLogInLogPersistence.Update(entity);
        }

        public override void Delete(UserLogInLog entity)
        {
            UserLogInLogPersistence.Delete(entity);
        }

        public override ValidationResults Validate(UserLogInLog entity)
        {
            return base.Validate(entity);
        }

        public override void Delete(int ID)
        {
            base.Delete(ID);
        }

        public override List<UserLogInLog> Find()
        {
            //if (CurrentUserAD.GetCurrentUser().isSiteAdmin)
            //    return SMSTemplatePersistence.Find().ToList();
            //else
            //{
            //    string[] listLoc = CurrentUserAD.GetCurrentUser().Admin_Location_Permission_List;
            //    return SMSTemplatePersistence.Find().Where(c => (c.Created_By.ToString().CompareTo(CurrentUserAD.GetCurrentUser().CAI.ToString()) == 0)).ToList();
            //}

            List<UserLogInLog> ListLocation = base.Find().ToList();
            return ListLocation;
        }

        public override UserLogInLog FindByID(int ID)
        {
            UserLogInLog SMSSendinoentity = base.FindByID(ID);
            return SMSSendinoentity;
        }

        public UserLogInLog FindBySessionID(string sID)
        {
            UserLogInLog SMSSendinoentity = UserLogInLogPersistence.Find().Where(s => s.SessionID == sID).FirstOrDefault();
            return SMSSendinoentity;
        }

        #endregion


        #region Properties

        protected IUserLogInLogPersistence UserLogInLogPersistence
        {
            get
            {
                return UnityContainerHelper.Resolve<IUserLogInLogPersistence>();
            }
        }
        #endregion
    }
}