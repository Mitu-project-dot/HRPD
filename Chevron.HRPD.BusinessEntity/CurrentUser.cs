
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Chevron.HRPD.BusinessEntities.T
{
    [Serializable]
    public class CurrentUser : BusinessEntity
    {
        #region "Properties"

        public string CAI { get; set; }

        public string FirstName { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public bool isHRPDAdmin { get; set; }

        public bool isHRPDEmployee { get; set; }

        public bool isHRPDSupervisor { get; set; }

        public bool isOEDashboardUser { get; set; }

        public bool isOEDashBoardUploader { get; set; }

        public bool isOneWayCommunicationUser { get; set; }

        public bool isTwoWayCommunicationUser { get; set; }


        public string[] Admin_Location_Permission_List { get; set; }

        public string Old_User_User_ID { get; set; }

        public string USER_AD_LOCATION { get; set; }


        #endregion
    }
}
