/// <summary>     
///  Added by       : Soumitra Bain
///  Date           : 12-May-2015
///  Purpose        : This Enums class will hold all Active Groups Name related to iDACS and assign value to Active Group to a
///                   particular Location id. For compliance issu, we can not keep this infomation Open such as web.config file.
///                   If any new Active Group is created related to iDACS, we neeed to write that group name here and assign a value 
///                   according....
///                   
///  Updated By     : 
///  Date           : 
///  Purpose        :
/// </summary>


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
  
namespace Chevron.HRPD.Common
{
    public class ADGroupEnums
    {
        public enum ADGroupToDBLocationID
        {
            // This is the site Admin so not belongs to any location.
            // It can acceass any location informatino.
            [Description("Site Admin")]
            DHAK94_iDACS_Site_Admins = 0,

            [Description("Admin for Dhaka")]
            DHAK94_iDACS_Users = 1,


            //[Description("Admin for Jalalabad Gas Plant (JB)")]
            //SYLLAC_iDACS_Admins = 2,

            [Description("Admin for Jalalabad Gas Plant (JB)")]
            SYLLAC_iDACS_Users = 2,


            //[Description("Admin for Bibiyana Gas Plant(BY)")]
            //OSM43_iDACS_Admins = 3,

            [Description("Admin for Bibiyana Gas Plant(BY)")]
            OSM43_iDACS_Users = 3,

            //[Description("Admin for Moulvibazar Gas Plant (MB)")]
            //SRIGHI_iDACS_Admins = 4

            [Description("Admin for Moulvibazar Gas Plant (MB)")]
            SRIGHI_iDACS_Users = 4

            //[Description("Admin for Muchai Station (MUC)")]
            //MUCHI_iDACS_Admins = 5            
        };

        public enum ADGroupToOEDepartmetnEnums
        {
            // This is the site Admin so not belongs to any location.
            // It can acceass any location informatino.

            [Description("GRP BANGLADESH OEDASH BBADMIN")]
            GRP_BANGLADESH_OEDASH_BBADMIN = 1,

            [Description("GRP BANGLADESH OEDASH FEADMIN")]
            GRP_BANGLADESH_OEDASH_FEADMIN = 2,

            [Description("GRP BANGLADESH OEDASH HESADMIN")]
            GRP_BANGLADESH_OEDASH_HESADMIN = 3,
        };
    }
}
