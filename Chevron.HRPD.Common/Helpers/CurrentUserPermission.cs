//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Chevron.HRPD.Common.Helpers
//{
//    class CurrentUserPermission
//    {
//    }
//}


using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chevron.HRPD.Common.Helpers;

namespace Chevron.HRPD.Common.Helpers
{
   public static class CurrentUserPermission
    {

       public static bool isUserSiteAdmin(string CAI)
       {
           bool yesSiteAdmin = false;

           yesSiteAdmin = CheckUserinADgroup(CAI, ConstantsAD.HRPDAdminGroup);

           return yesSiteAdmin;
       }

       public static StringCollection Admin_Locations_List(string iCAI)
       {
           StringCollection locations = new StringCollection();

           foreach (ADGroupEnums.ADGroupToDBLocationID grps in Enum.GetValues(typeof(ADGroupEnums.ADGroupToDBLocationID)))
           {
               string EnamName = grps.ToString().Replace(' ', '_');

               string ADGroupName = grps.ToString().Replace('_', ' ');

               ADGroupEnums.ADGroupToDBLocationID enumLocationCode = (ADGroupEnums.ADGroupToDBLocationID)Enum.Parse(typeof(ADGroupEnums.ADGroupToDBLocationID), EnamName);

               int locationCode = (int)enumLocationCode;

               if (CheckUserinADgroup(iCAI, ADGroupName))
               { locations.Add(locationCode.ToString()); }

           }

           return locations;
       }


       public static List<int> Local_Admins_List(string iCAI)
       {
           List<int> locations = new List<int>();

           foreach (ADGroupEnums.ADGroupToDBLocationID grps in Enum.GetValues(typeof(ADGroupEnums.ADGroupToDBLocationID)))
           {
               string EnamName = grps.ToString().Replace(' ', '_');

               string ADGroupName = grps.ToString().Replace('_', ' ');

               ADGroupEnums.ADGroupToDBLocationID enumLocationCode = (ADGroupEnums.ADGroupToDBLocationID)Enum.Parse(typeof(ADGroupEnums.ADGroupToDBLocationID), EnamName);

               int locationCode = (int)enumLocationCode;

               if (CheckUserinADgroup(iCAI, ADGroupName))
               { locations.Add(locationCode); }

           }

           return locations;
       }

       public static bool CheckUserinADgroup(string username, string adGropuName)
       {
           bool yesAD = false;

           // set up domain context
           PrincipalContext ctx = new PrincipalContext(ContextType.Domain, ConstantsAD.Domain);

           // find a user
           UserPrincipal user = UserPrincipal.FindByIdentity(ctx, username);

           // find the group in question
           GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, adGropuName);

           if (user != null)
           {
               // check if user is member of that group
               if (user.IsMemberOf(group))
               {
                   yesAD = true;
               }
           }

           return yesAD;
       }

    }
}

