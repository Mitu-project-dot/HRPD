


/// <summary>     
///  Added by       : Soumitra Bain
///  Date           : 12-May-2015
///  Purpose        : This class will handle all kind of Active Directory releated functionality.
///                   Such as gettting User Details information, User Active Directory group details information
///                   User AD group permissions
///                   
///  Updated By     : Soumitra Bain
///  Date           : 16-Apr-2017 
///  Purpose        : Implementing new two GLAM group for sending one way and two way communication
///
///  Updated By     : 
///  Date           : 
///  Purpose        :
/// </summary>


using Chevron.HRPD.BusinessEntities.T;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
//using System.DirectoryServices.ActiveDirectory;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Linq;
using System.Collections.Specialized;

namespace Chevron.HRPD.Common.Helpers
{
    [Serializable]
    public class CurrentUserAD : GenericIdentity
    {
        #region "Property"

        public string CAI { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public string DisplayName { get; set; }

        public bool isHRPDEmployee { get; set; }

        public bool isHRPDAdmin { get; set; }

        public bool isHRPDSupervisor { get; set; }

        public bool isPSMDashboardUser { get; set; }

        public bool isPSMDashBoardUploader { get; set; }

        public bool isOneWayCommunicationUser { get; set; }

        public bool isTwoWayCommunicationUser { get; set; }

        //public string userCurrentSessionID { get; set; }

        public string[] Admin_Location_Permission_List { get; set; }

        public string[] Admin_PSM_Upload_Permission_List { get; set; }

        public string Old_User_User_ID { get; set; }

        public string USER_AD_LOCATION { get; set; }

        #endregion

        protected CurrentUserAD()
            : this(HttpContext.Current.User.Identity.Name)
        {

        }

        public CurrentUserAD(string LogonID_or_CAI_or_Email)
            : base(LogonID_or_CAI_or_Email, "Windows")
        {

            if ((LogonID_or_CAI_or_Email == null) || (LogonID_or_CAI_or_Email.Length == 0))
            {
                throw (new ArgumentException("Logon ID of the current user is empty. Make sure to enable Windows authentication on the website."));
            }

            PopulateUserAttrsFromDirectory(LogonID_or_CAI_or_Email);

            //Temporary coded for perfomance test.

            //isSiteAdmin = isSiteAdmin_ByCAI(CAI);

            //Admin_Location_Permission_List = Admin_Locations_Permission(CAI);


            //HRPD User's Access Permission
            isHRPDEmployee = false;
            isHRPDAdmin = false; // isSiteAdmin_ByCAI(CAI);
            isHRPDSupervisor = false;
            isPSMDashboardUser = false;
            isPSMDashBoardUploader = false;
            //Admin_Location_Permission_List = Admin_Locations_Permission(CAI, Old_User_User_ID);

            //OE User's access permission

           // Admin_PSM_Upload_Permission_List = Admin_PSM_File_Upload_Permission(CAI,Old_User_User_ID);
            //if (!isPSMDashboardUser)
           //     isPSMDashboardUser = Check_PSM_Dashboard_View_Permission(CAI, Old_User_User_ID); //CheckUserinADgroup(CAI, Old_User_User_ID, ConstantsAD.PSMDashBoardViewerGroup);
            //isOEDashBoardUploader = false;          


          //  isOneWayCommunicationUser = Check_One_Way_SMS_Send_Permission(CAI, Old_User_User_ID); // Checking user permission for One way SMS Send

           // isTwoWayCommunicationUser = Check_Two_Way_SMS_Send_Permission(CAI, Old_User_User_ID); // Checking user permission for Two way SMS send 



            isHRPDAdmin = isHRPDAdmin_ByCAI(CAI, Old_User_User_ID);
            isHRPDSupervisor = isHRPDSupervisor_ByCAI(CAI, Old_User_User_ID);
            isHRPDEmployee = isHRPDEmployee_ByCAI(CAI, Old_User_User_ID);


            //if (isHRPDAdmin)
            //{
            //    isOneWayCommunicationUser = true;
            //    isTwoWayCommunicationUser = true;
            //}
            //else
            //{
            //    if (isTwoWayCommunicationUser)
            //    {
            //        if (Admin_Location_Permission_List.Length <= 1)
            //        {
            //            if (string.IsNullOrEmpty(Admin_Location_Permission_List[0].ToString()))
            //            {
            //                var array1 = Two_Way_Locations_Permission_From_AD_Location(CAI, USER_AD_LOCATION);

            //                Admin_Location_Permission_List[0] = array1[0];
            //            }
            //            else
            //            {
            //                var array1 = Two_Way_Locations_Permission_From_AD_Location(CAI, USER_AD_LOCATION);
            //                var array2 = Admin_Locations_Permission(CAI, Old_User_User_ID);

            //                Admin_Location_Permission_List = array1.Union(array2).ToArray();
            //            }
            //        }
            //    }
            //}


            //bool vtrue = false;
            //vtrue = GetValidateLogin(CAI, "xxxx"); //ValidateAgainstADAndGroup(CAI, "1234","");

           

            //*******************

        }


        private void PopulateUserAttrsFromDirectory(string LogonID_or_CAI_or_Email)
        {
            if (LogonID_or_CAI_or_Email.Contains("\\"))
            {
                LogonID_or_CAI_or_Email = LogonID_or_CAI_or_Email.Split('\\')[1];
            }

            Chevron.HRPD.BusinessEntities.T.CurrentUser iCurrentUser = new Chevron.HRPD.BusinessEntities.T.CurrentUser();

            iCurrentUser = GetUserByCAI(LogonID_or_CAI_or_Email);

            CAI = iCurrentUser.CAI.ToString().ToUpper();

            Email = "";

            FirstName = iCurrentUser.FirstName;

            LastName = iCurrentUser.LastName;

            DisplayName = iCurrentUser.DisplayName;

            Old_User_User_ID = iCurrentUser.Old_User_User_ID;

            USER_AD_LOCATION = iCurrentUser.USER_AD_LOCATION;
        }

        public static CurrentUser GetUserByCAI(string CAI_or_ID)
        {
            CurrentUser user = new CurrentUser();

            if (!string.IsNullOrEmpty(CAI_or_ID))
            {
                using (DirectorySearcher searcher = new DirectorySearcher(new DirectoryEntry()))
                {
                    searcher.PageSize = 1000;
                    searcher.SearchScope = SearchScope.Subtree;
                    searcher.Filter = "(&(samAccountType=805306368)(cvx-cai=" + CAI_or_ID + "))";                    

                    using (SearchResultCollection Results = searcher.FindAll())
                    {
                        if ((Results != null) && Results.Count != 0)
                        {
                            using (DirectoryEntry UserDE = Results[0].GetDirectoryEntry())
                            {
                                user.FirstName = UserDE.Properties["givenName"].Value.ToString();
                                user.LastName = UserDE.Properties["sn"].Value.ToString();
                                user.DisplayName = UserDE.Properties["displayName"].Value.ToString();
                                user.CAI = UserDE.Properties["cvx-cai"].Value.ToString().ToUpper();
                                user.Old_User_User_ID = UserDE.Properties["cn"].Value.ToString();
                                user.USER_AD_LOCATION = UserDE.Properties["cvx-facilityCode"].Value.ToString();
                            }
                        }
                    }
                }
            }

            if (user.CAI == null)
            {
                using (DirectorySearcher searcher = new DirectorySearcher(new DirectoryEntry()))
                {
                    searcher.PageSize = 1000;
                    searcher.SearchScope = SearchScope.Subtree;
                    searcher.Filter = "(&(samAccountType=805306368)(sAMAccountName=" + CAI_or_ID + "))";

                    using (SearchResultCollection Results = searcher.FindAll())
                    {
                        if ((Results != null) && Results.Count != 0)
                        {
                            using (DirectoryEntry UserDE = Results[0].GetDirectoryEntry())
                            {
                                user.FirstName = UserDE.Properties["givenName"].Value.ToString();
                                user.LastName = UserDE.Properties["sn"].Value.ToString();
                                user.DisplayName = UserDE.Properties["displayName"].Value.ToString();
                                user.CAI = UserDE.Properties["cvx-cai"].Value.ToString().ToUpper();
                                user.Old_User_User_ID = UserDE.Properties["cn"].Value.ToString();
                                user.USER_AD_LOCATION = UserDE.Properties["cvx-facilityCode"].Value.ToString();
                            }
                        }
                    }
                }
            }

            return user;
        }

        public static CurrentUserAD GetCurrentUser()
        {
            CurrentUserAD retVal;

            Object cachedObject = HttpContext.Current.Cache[HttpContext.Current.User.Identity.Name];

            //Checks if User Information is already stored in cache
            if (cachedObject == null)
            {
                retVal = new CurrentUserAD();
                HttpContext.Current.Cache.Insert(HttpContext.Current.User.Identity.Name, retVal, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(GetPersonCacheTime()));
            }
            else
            {
                retVal = (CurrentUserAD)cachedObject;
            }

            return retVal;
        }

        public static CurrentUser GetCurrentUserInfo()
        {
            CurrentUserAD retVal;
            CurrentUser retValInfoCurrent = new CurrentUser();

            Object cachedObject = HttpContext.Current.Cache[HttpContext.Current.User.Identity.Name];

            //Checks if User Information is already stored in cache
            if (cachedObject == null)
            {
                retVal = new CurrentUserAD();

                HttpContext.Current.Cache.Insert(HttpContext.Current.User.Identity.Name, retVal, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(GetPersonCacheTime()));
            }
            else
            {
                retVal = (CurrentUserAD)cachedObject;
            }

            retValInfoCurrent.Admin_Location_Permission_List = retVal.Admin_Location_Permission_List;
            retValInfoCurrent.CAI = retVal.CAI;
            retValInfoCurrent.DisplayName = retVal.DisplayName;
            retValInfoCurrent.FirstName = retVal.FirstName;
            retValInfoCurrent.LastName = retVal.LastName;

            retValInfoCurrent.isHRPDAdmin = retVal.isHRPDAdmin;
            retValInfoCurrent.isHRPDEmployee = retVal.isHRPDEmployee;
            retValInfoCurrent.isHRPDSupervisor = retVal.isHRPDSupervisor;
            
            retValInfoCurrent.Old_User_User_ID = retVal.Old_User_User_ID;

            return retValInfoCurrent;
        }


        //Gets cache information expiration time previously set in web.config
        private static int GetPersonCacheTime()
        {
            AppSettingsReader configReader = new AppSettingsReader();
            int cacheSeconds = (int)configReader.GetValue("PersonCacheSeconds", typeof(int));
            return cacheSeconds;
        }

        public static List<string> GetGroupMembers(string strGroup, string option = null)
        {
            DirectoryEntry DirectoryRoot2 = new DirectoryEntry("LDAP://RootDSE");
            string DNC = DirectoryRoot2.Properties["DefaultNamingContext"][0].ToString();
            List<string> GroupMembers = new List<string>();


            DirectoryEntry DirectoryRoot = new DirectoryEntry("LDAP://" + DNC);
            DirectorySearcher DirectorySearch = new DirectorySearcher(DirectoryRoot, "(CN=" + strGroup + ")");

            try
            {
                SearchResultCollection DirectorySearchCollection = DirectorySearch.FindAll();

                foreach (SearchResult DirectorySearchResult in DirectorySearchCollection)
                {
                    ResultPropertyCollection ResultPropertyCollection = DirectorySearchResult.Properties;
                    string GroupMemberDN = null;
                    foreach (string GroupMemberDN_loopVariable in ResultPropertyCollection["member"])
                    {
                        GroupMemberDN = GroupMemberDN_loopVariable;

                        DirectoryEntry DirectoryMember = new DirectoryEntry("LDAP://" + GroupMemberDN);
                        System.DirectoryServices.PropertyCollection DirectoryMemberProperties = DirectoryMember.Properties;
                        object DirectoryItem = DirectoryMemberProperties["sAMAccountName"].Value;

                        if (GroupMemberDN.Contains("OU=Groups"))
                        {
                            GetGroupMembers(DirectoryMemberProperties["cn"].Value.ToString());
                        }
                        else
                        {
                            if (DirectoryItem != null)
                            {
                                if (option == null)
                                    GroupMembers.Add(DirectoryItem.ToString());
                                else
                                    GroupMembers.Add(DirectoryItem.ToString() + "@chevron.com");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Trace.Write(ex.Message)
            }
            return GroupMembers;
        }



        public static bool CheckCAIInGropuParentGroup(string strGroup, string iCAI,string iUser, ref bool bfound, string option = null)
        {
            DirectoryEntry DirectoryRoot2 = new DirectoryEntry("LDAP://RootDSE");
            string DNC = DirectoryRoot2.Properties["DefaultNamingContext"][0].ToString();
            List<string> GroupMembers = new List<string>();


            DirectoryEntry DirectoryRoot = new DirectoryEntry("LDAP://" + DNC);
            DirectorySearcher DirectorySearch = new DirectorySearcher(DirectoryRoot, "(CN=" + strGroup + ")");

            try
            {
                SearchResultCollection DirectorySearchCollection = DirectorySearch.FindAll();

                foreach (SearchResult DirectorySearchResult in DirectorySearchCollection)
                {
                    ResultPropertyCollection ResultPropertyCollection = DirectorySearchResult.Properties;
                    string GroupMemberDN = null;
                    foreach (string GroupMemberDN_loopVariable in ResultPropertyCollection["member"])
                    {
                        GroupMemberDN = GroupMemberDN_loopVariable;

                        DirectoryEntry DirectoryMember = new DirectoryEntry("LDAP://" + GroupMemberDN);
                        System.DirectoryServices.PropertyCollection DirectoryMemberProperties = DirectoryMember.Properties;
                        object DirectoryItem = DirectoryMemberProperties["sAMAccountName"].Value;

                        if (GroupMemberDN.Contains("OU=Groups"))
                        {
                            CheckCAIInGropuParentGroup(DirectoryMemberProperties["cn"].Value.ToString(),iCAI,iUser,ref bfound);
                        }
                        else
                        {
                            if (DirectoryItem != null)
                            {
                                if (option == null)
                                {
                                    if ((DirectoryItem.ToString().ToUpper() == iCAI.ToString().ToUpper())||(DirectoryItem.ToString().ToUpper() == iUser.ToString().ToUpper()))
                                    {
                                        bfound = true;
                                        GroupMembers.Add(DirectoryItem.ToString());
                                        return bfound;
                                    }
                                }
                                else
                                    GroupMembers.Add(DirectoryItem.ToString() + "@chevron.com");
                            }
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                //Trace.Write(ex.Message)
            }
            return bfound;
        }


        public static string FindManager(string CAI)
        {
            string _CAI = null, manager = string.Empty;
            _CAI = string.Empty;
            StringBuilder test = new StringBuilder();
            if (!string.IsNullOrEmpty(CAI))
            {
                using (DirectorySearcher searcher = new DirectorySearcher(new DirectoryEntry()))
                {
                    searcher.PageSize = 1000;
                    searcher.SearchScope = SearchScope.Subtree;
                    searcher.Filter = "(&(samAccountType=805306368)(sAMAccountName=" + CAI + "))";

                    using (SearchResultCollection Results = searcher.FindAll())
                    {
                        if ((Results != null) && Results.Count != 0)
                        {
                            using (DirectoryEntry UserDE = Results[0].GetDirectoryEntry())
                            {
                                manager = UserDE.Properties["manager"][0].ToString();
                                manager = manager.Substring(3, 4);
                            }
                        }
                    }
                }
            }
            return manager;
        }

        public static bool isHRPDAdmin_ByCAI(string iCAI,string iUserID)
        {
            bool siteAdmin = false;

            siteAdmin = CheckUserinADgroup(iCAI, iUserID, ConstantsAD.HRPDAdminGroup);

            //  StringCollection strcoll = Admin_Locations_List(iCAI);    



         //   if (iCAI.ToUpper().Trim().Contains("BBTN") || iCAI.ToUpper().Trim().Contains("BVFI") || iCAI.ToUpper().Trim().Contains("UMMM") || iCAI.ToUpper().Trim().Contains("UBCE"))
            if (iCAI.ToUpper().Trim().Contains("NWQN") || iCAI.ToUpper().Trim().Contains("BVFI") || iCAI.ToUpper().Trim().Contains("UBCE"))
                return siteAdmin;
            else
                return false;

            //return siteAdmin;
        }

        public static bool isHRPDSupervisor_ByCAI(string iCAI, string iUserID)
        {
            bool siteAdmin = false;

            siteAdmin = CheckUserinADgroup(iCAI, iUserID, ConstantsAD.HRPDSupervisorGroup);

            //  StringCollection strcoll = Admin_Locations_List(iCAI);       

            return siteAdmin;
        }

        public static bool isHRPDEmployee_ByCAI(string iCAI, string iUserID)
        {
            bool siteAdmin = false;

            siteAdmin = CheckUserinADgroup(iCAI, iUserID, ConstantsAD.HRPDEmployeeGroup);

            //  StringCollection strcoll = Admin_Locations_List(iCAI);       

            return siteAdmin;
        }


        public static bool CheckUserinADgroup(string userCAI, string UserOLDID, string adGropuName)
        {
            bool yesInAD = false;

            // set up domain context
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, ConstantsAD.Domain);

            // find a user
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, userCAI);

            //if user is not avaialbe with CAI then search his/her permission with the help of his user ID, for old user.

            if (user == null)
            {
                user = UserPrincipal.FindByIdentity(ctx, UserOLDID);
            }

            // find the group in question
            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, adGropuName);

            if (user != null)
            {
                if (group != null)
                {
                    // check if user is member of that group
                    if (user.IsMemberOf(group))
                    {
                        yesInAD = true;
                    }
                }
            }

            return yesInAD;
        }

        public string GetUserIDFOROLDUSER()
        {
            return Old_User_User_ID;
        }

        public string[] Admin_Locations_Permission(string iCAI, string iUserID)
        {            

            string iUser = iCAI;
            string locationCodes = string.Empty;
            bool bAdmin = false;

          //throw new Exception("test Excep");

            try
            {
                
                foreach (string name in Enum.GetNames(typeof(ADGroupEnums.ADGroupToDBLocationID)))
                {
                    string adGroupName = name.ToString().Replace('_', ' ');

                    if (CheckUserinADgroup(iCAI, iUserID, adGroupName))
                    {
                        isHRPDEmployee = true;

                        ADGroupEnums.ADGroupToDBLocationID enumLocationCode = (ADGroupEnums.ADGroupToDBLocationID)Enum.Parse(typeof(ADGroupEnums.ADGroupToDBLocationID), name);

                        int locCode = (int)enumLocationCode;

                        if (locCode == 0)
                        {
                            isHRPDAdmin = true;
                            bAdmin = true;
                            break;
                        }
                        locationCodes += locCode + ",";
                    }
                }

                if (bAdmin)
                {
                    locationCodes = string.Empty;
                    foreach (ADGroupEnums.ADGroupToDBLocationID AD in Enum.GetValues(typeof(ADGroupEnums.ADGroupToDBLocationID)))
                    {
                        if (!string.IsNullOrEmpty(locationCodes))
                            locationCodes += ",";
                        int locCode = (int)AD;
                        locationCodes += locCode;
                    }
                }
                return locationCodes.ToString().Split(',');
            }
            catch (Exception Ex)
            {
                return locationCodes.ToString().Split(',');
            }
        }


        public string[] Two_Way_Locations_Permission_From_AD_Location(string iCAI, string iADLocation)
        {

            string iUser = iCAI;
            string locationCodes = string.Empty;          

            //throw new Exception("test Excep");

            try
            {

                foreach (string name in Enum.GetNames(typeof(ADGroupEnums.ADGroupToDBLocationID)))
                {
                    string adGroupName = name.ToString().Replace('_', ' ');

                   // if (CheckUserinADgroup(iCAI, iUserID, adGroupName))
                    if (adGroupName.Contains(iADLocation))
                    {
                        ADGroupEnums.ADGroupToDBLocationID enumLocationCode = (ADGroupEnums.ADGroupToDBLocationID)Enum.Parse(typeof(ADGroupEnums.ADGroupToDBLocationID), name);

                        int locCode = (int)enumLocationCode;

                        if (locCode == 0)
                            continue;

                        locationCodes += locCode + ",";

                        break;
                    }
                }
         
                return locationCodes.ToString().Split(',');
            }
            catch (Exception Ex)
            {
                return locationCodes.ToString().Split(',');
            }
        }


        public string[] Admin_PSM_File_Upload_Permission(string iCAI, string iUserID)
        {
            string iUser = iCAI;
            string oeDepartmentCodes = string.Empty;
            isPSMDashBoardUploader = false;
            try
            {

                foreach (string name in Enum.GetNames(typeof(ADGroupEnums.ADGroupToOEDepartmetnEnums)))
                {
                    string adGroupName = name.ToString().Replace('_', ' ');

                    if (CheckUserinADgroup(iCAI, iUserID, adGroupName))
                    {
                        isPSMDashBoardUploader = true;
                        isPSMDashboardUser = true;

                        ADGroupEnums.ADGroupToOEDepartmetnEnums enumOECode = (ADGroupEnums.ADGroupToOEDepartmetnEnums)Enum.Parse(typeof(ADGroupEnums.ADGroupToOEDepartmetnEnums), name);

                        int oeDeptCode = (int)enumOECode;

                        oeDepartmentCodes += oeDeptCode + ",";
                    }
                }

                return oeDepartmentCodes.ToString().Split(',');
            }
            catch (Exception Ex)
            {
                return oeDepartmentCodes.ToString().Split(',');
            }
        }


        //Check for User in PSM Dashboard view Group

        public bool Check_PSM_Dashboard_View_Permission(string iCAI, string iUserID)
        {
            string iUser = iCAI;
            string oeDepartmentCodes = string.Empty;
            isPSMDashBoardUploader = false;
            try
            {
                string[] PSM_View_Group = ConstantsAD.HRPDEmployeeGroup.ToString().Split(',');

                foreach (string adGroupName in PSM_View_Group)
                {

                    if (CheckUserinADgroup(iCAI, iUserID, adGroupName))
                    {
                        isPSMDashboardUser = true;
                        return isPSMDashboardUser;
                    }
                }

                return isPSMDashboardUser;
            }
            catch (Exception Ex)
            {
                return isPSMDashboardUser;
            }
        }


        //Check permission for One way Message send permission

        public bool Check_One_Way_SMS_Send_Permission(string iCAI, string iUserID)
        {
            string iUser = iCAI;
            string oeDepartmentCodes = string.Empty;
            isOneWayCommunicationUser = false;
            try
            {
                //string[] PSM_View_Group = ConstantsAD.HESSMSSendGroup.ToString().Split(',');
                string[] PSM_View_Group = ConstantsAD.HRPDTwoWayCommunication.ToString().Split(',');

                bool foundUser = false;

                if (CheckCAIInGropuParentGroup(PSM_View_Group[0].ToString(), iCAI,iUserID, ref foundUser))
                {
                    isOneWayCommunicationUser = true;
                    return isOneWayCommunicationUser;
                }

                return isOneWayCommunicationUser;
            }
            catch (Exception Ex)
            {
                return isOneWayCommunicationUser;
            }
        }


        //Check for User in Permission in HES SMS Send

        public bool Check_Two_Way_SMS_Send_Permission(string iCAI, string iUserID)
        {
            string iUser = iCAI;
            string oeDepartmentCodes = string.Empty;
            isTwoWayCommunicationUser = false;
            try
            {
                //string[] PSM_View_Group = ConstantsAD.HESSMSSendGroup.ToString().Split(',');
                string[] PSM_View_Group = ConstantsAD.HRPDTwoWayCommunication.ToString().Split(',');

                bool foundUser = false;

                if (CheckCAIInGropuParentGroup(PSM_View_Group[0].ToString(), iCAI,iUserID, ref foundUser))
                {
                    isTwoWayCommunicationUser = true;
                    return isTwoWayCommunicationUser;
                }

                return isTwoWayCommunicationUser;
            }
            catch (Exception Ex)
            {
                return isTwoWayCommunicationUser;
            }
        }



        public static List<string> GetHRPDGroupMembers(string strGroup, string option = null)
        {
            DirectoryEntry DirectoryRoot2 = new DirectoryEntry("LDAP://RootDSE");
            string DNC = DirectoryRoot2.Properties["DefaultNamingContext"][0].ToString();
            List<string> GroupMembers = new List<string>();


            DirectoryEntry DirectoryRoot = new DirectoryEntry("LDAP://" + DNC);
            DirectorySearcher DirectorySearch = new DirectorySearcher(DirectoryRoot, "(CN=" + strGroup + ")");

            try
            {
                SearchResultCollection DirectorySearchCollection = DirectorySearch.FindAll();

                foreach (SearchResult DirectorySearchResult in DirectorySearchCollection)
                {
                    ResultPropertyCollection ResultPropertyCollection = DirectorySearchResult.Properties;
                    string GroupMemberDN = null;
                    foreach (string GroupMemberDN_loopVariable in ResultPropertyCollection["member"])
                    {
                        GroupMemberDN = GroupMemberDN_loopVariable;

                        DirectoryEntry DirectoryMember = new DirectoryEntry("LDAP://" + GroupMemberDN);
                        System.DirectoryServices.PropertyCollection DirectoryMemberProperties = DirectoryMember.Properties;
                        object DirectoryItem = DirectoryMemberProperties["sAMAccountName"].Value;

                        if (GroupMemberDN.Contains("OU=Groups"))
                        {
                            GetGroupMembers(DirectoryMemberProperties["cn"].Value.ToString());
                        }
                        else
                        {
                            if (DirectoryItem != null)
                            {
                                if (option == null)
                                    GroupMembers.Add(DirectoryItem.ToString().ToUpper());
                                else
                                    GroupMembers.Add(DirectoryItem.ToString().ToUpper() + "@chevron.com");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Trace.Write(ex.Message)
            }

            return GroupMembers;
        }

        //public static List<string> GetUserGroupList(string CAI)
        //{
        //    List<string> GrpList = new List<string>();
        //    var userNameContains = "part_of_user_name";

        //    var identity = CAI;//WindowsIdentity.GetCurrent().User;
        //    var allDomains = Forest.GetCurrentForest().Domains.Cast<Domain>();

        //    var allSearcher = allDomains.Select(domain =>
        //    {
        //        var searcher = new DirectorySearcher(new DirectoryEntry("LDAP://" + domain.Name));

        //        // Apply some filter to focus on only some specfic objects
        //        searcher.Filter = String.Format("(&(&(objectCategory=person)(objectClass=user)(name=*{0}*)))", userNameContains);
        //        return searcher;
        //    });

        //    var directoryEntriesFound = allSearcher
        //        .SelectMany(searcher => searcher.FindAll()
        //            .Cast<SearchResult>()
        //            .Select(result => result.GetDirectoryEntry()));

        //    var memberOf = directoryEntriesFound.Select(entry =>
        //    {
        //        using (entry)
        //        {
        //            return new
        //            {
        //                Name = entry.Name,
        //                GroupName = ((object[])entry.Properties["MemberOf"].Value).Select(obj => obj.ToString())
        //            };
        //        }
        //    });

        //    foreach (var item in memberOf)
        //    {
        //        //Debug.Print("Name = " + item.Name);
        //        //Debug.Print("Member of:");

        //        foreach (var groupName in item.GroupName)
        //        {
        //            GrpList.Add(groupName.ToString());
        //            // Debug.Print("   " + groupName);
        //        }

        //        return GrpList;

        //        // Debug.Print(String.Empty);
        //    }
        //}



        public static bool GetValidateLogin(string strAccountId, string strPassword)
        {
            bool bSucceeded = false;
            string strError = string.Empty;
            string strAuthenticatedBy = string.Empty;


            DirectoryEntry DirectoryRoot2 = new DirectoryEntry("LDAP://RootDSE");
            string DNC = DirectoryRoot2.Properties["DefaultNamingContext"][0].ToString();
            List<string> GroupMembers = new List<string>();
            DirectoryEntry DirectoryRoot = new DirectoryEntry("LDAP://" + DNC);

            //using (DirectoryEntry adsEntry = new DirectoryEntry("LDAP://" + DNC, strAccountId, strPassword))
            //{
            //    using (DirectorySearcher adsSearcher = new DirectorySearcher(adsEntry))
            //    {
            //        //adsSearcher.Filter = "(&(objectClass=user)(objectCategory=person))";
            //        adsSearcher.Filter = "(sAMAccountName=" + strAccountId + ")";

            //        try
            //        {
            //            SearchResult adsSearchResult = adsSearcher.FindOne();
            //            bSucceeded = true;

            //            strAuthenticatedBy = "Active Directory";
            //            strError = "User has been authenticated by Active Directory.";
            //        }
            //        catch (Exception ex)
            //        {
            //            // Failed to authenticate. Most likely it is caused by unknown user
            //            // id or bad strPassword.
            //            strError = ex.Message;
            //        }
            //        finally
            //        {
            //            adsEntry.Close();
            //        }
            //    }
            //}

            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + DNC, strAccountId, strPassword);
                entry.RefreshCache();
                DirectorySearcher mySearcher = new DirectorySearcher(entry);
                mySearcher.Filter = "(&(objectClass=user)(|(cn=" + strAccountId + ")(sAMAccountName=" + strAccountId + ")))";
                mySearcher.PropertiesToLoad.Add("memberOf");
                SearchResult result = mySearcher.FindOne();
                if (result != null)
                {
                    foreach (string GroupPath in result.Properties["memberOf"])
                    {
                        if (GroupPath.Contains("Group Name"))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
 
            }

            return false;

            return bSucceeded;
        }

        private bool ValidateAgainstADAndGroup(string username, string password, string groupname)
        {
            var ok = false;
            using (var pc = new PrincipalContext(ContextType.Domain, ConstantsAD.Domain))
            {
                if (pc.ValidateCredentials(username, password))
                {
                    //User is alright
                    using (var searcher = new PrincipalSearcher(new UserPrincipal(pc)))
                    {
                        searcher.QueryFilter.SamAccountName = username;
                        Principal u = searcher.FindOne();
                        foreach (Principal p in u.GetGroups())
                        {
                            if (p.Name == groupname)
                            {
                                //User is in group
                                ok = true;
                            }
                        }
                    }
                }
            }

            return ok;
        }

    }
}
