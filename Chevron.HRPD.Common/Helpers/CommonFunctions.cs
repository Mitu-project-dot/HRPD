


using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.DirectoryServices;
using Chevron.HRPD.BusinessEntities;
using Chevron.HRPD.BusinessEntities.T;
using System.Text.RegularExpressions;

namespace Chevron.HRPD.Common.Helpers
{
    public static class CommonFunctions
    {
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

        public static void DisableControls(Control parent, bool State)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is DropDownList)
                {
                    ((DropDownList)(c)).Enabled = State;
                }
                else if (c is TextBox)
                {
                    ((TextBox)(c)).Enabled = State;
                }
                else if (c is Button)
                {
                    ((Button)(c)).Enabled = State;
                }
                DisableControls(c, State);
            }
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

        public static string GetURL()
        {
            return ConfigurationManager.AppSettings["webURL"].ToString();
        }

        //public static List<string> GetGACAI(string Option)
        //{
        //    string CAI = ConfigurationManager.AppSettings["GA"].ToString();
        //    string[] strCAI = CAI.Split(',');
        //    List<string> listCAI = new List<string>();
        //    listCAI = GetGroupMembers(CAI);

        //    foreach (string st in strCAI)
        //    {
        //        if(Option=="mail")
        //            listCAI.Add(st+"@chevron.com");
        //        else if(Option=="CAI")
        //            listCAI.Add(st);
        //    }

        //    return listCAI;
        //}

        /// <summary>
        /// This method returns CAI or email address based on parameter
        /// </summary>
        /// <param name="Option">Option may be CAI or mail</param>
        /// <returns></returns>
        //public static List<string> GetSCMCAI(string Option)
        //{
        //    string CAI = ConfigurationManager.AppSettings["SCM_CAI"].ToString();
        //    string[] strCAI = CAI.Split(',');
        //    List<string> listCAI = new List<string>();

        //    foreach (string st in strCAI)
        //    {
        //        if (Option == "mail")
        //            listCAI.Add(st + "@chevron.com");
        //        else if (Option == "CAI")
        //            listCAI.Add(st);
        //    }

        //    return listCAI;
        //}

        public static int GetFirstMail()
        {
            int day = 0;
            string mail = ConfigurationManager.AppSettings["FirstMail"].ToString();

            //if (mail != null && mail != "")
            int.TryParse(mail, out day);

            return day;
        }

        public static int GetSecondMail()
        {
            int day;
            int.TryParse(ConfigurationManager.AppSettings["SecondMail"].ToString(), out day);

            return day;// ConfigurationManager.AppSettings["SecondMail"].ToString();
        }

        public static CurrentUser GetUserByCAI(string CAI)
        {
            CurrentUser user = new CurrentUser();

            if (!string.IsNullOrEmpty(CAI))
            {
                using (DirectorySearcher searcher = new DirectorySearcher(new DirectoryEntry()))
                {
                    searcher.PageSize = 1000;
                    searcher.SearchScope = SearchScope.Subtree;
                    searcher.Filter = "(&(samAccountType=805306368)(cvx-cai=" + CAI + "))";

                    using (SearchResultCollection Results = searcher.FindAll())
                    {
                        if ((Results != null) && Results.Count != 0)
                        {
                            using (DirectoryEntry UserDE = Results[0].GetDirectoryEntry())
                            {
                                user.FirstName = UserDE.Properties["givenName"].Value.ToString();
                                user.LastName = UserDE.Properties["sn"].Value.ToString();
                                user.CAI = CAI;
                            }
                        }
                    }
                }
            }
            return user;
        }

        //public static Employee_Info GetEmployeeByCAI(string CAI)
        //{
        //    Employee_Info SearchEmpInfo = new Employee_Info();
        //    string checkPattern = @"^[a-zA-Z]{4}$";

        //    Regex pattern = new Regex(checkPattern);            

        //    if (pattern.IsMatch(CAI))
        //    {
        //        if (!string.IsNullOrEmpty(CAI))
        //        {
        //            using (DirectorySearcher searcher = new DirectorySearcher(new DirectoryEntry()))
        //            {
        //                searcher.PageSize = 1000;
        //                searcher.SearchScope = SearchScope.Subtree;
        //                searcher.Filter = "(&(samAccountType=805306368)(cvx-cai=" + CAI + "))";

        //                using (SearchResultCollection Results = searcher.FindAll())
        //                {
        //                    if ((Results != null) && Results.Count != 0)
        //                    {
        //                        using (DirectoryEntry UserDE = Results[0].GetDirectoryEntry())
        //                        {
        //                            SearchEmpInfo.First_Name = (UserDE.Properties["givenName"].Value ?? "").ToString();
        //                            SearchEmpInfo.Last_Name = (UserDE.Properties["sn"].Value ?? "").ToString();
        //                            SearchEmpInfo.CAI = CAI;
        //                            SearchEmpInfo.E_Mail = (UserDE.Properties["mail"].Value ?? "").ToString();
        //                            SearchEmpInfo.Mobile_No = (UserDE.Properties["mobile"].Value ?? "").ToString().Replace(" ", string.Empty).Replace("+", string.Empty);

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
            
        //    return SearchEmpInfo;
        //}

        //public static Employee_Info GetUserByUserID(string ID)
        //{
        //    string checkPattern = @"^[0-9]{5}$";

        //    Regex pattern = new Regex(checkPattern);

        //    Employee_Info SearchEmpInfo = new Employee_Info();

        //    if (pattern.IsMatch(ID))
        //    {

        //        if (!string.IsNullOrEmpty(ID))
        //        {
        //            using (DirectorySearcher searcher = new DirectorySearcher(new DirectoryEntry()))
        //            {
        //                searcher.PageSize = 1000;
        //                searcher.SearchScope = SearchScope.Subtree;
        //                searcher.Filter = "(&(samAccountType=805306368)(sAMAccountName=" + ID + "))";

        //                using (SearchResultCollection Results = searcher.FindAll())
        //                {
        //                    if ((Results != null) && Results.Count != 0)
        //                    {
        //                        using (DirectoryEntry UserDE = Results[0].GetDirectoryEntry())
        //                        {
        //                            SearchEmpInfo.First_Name = UserDE.Properties["givenName"].Value.ToString();
        //                            SearchEmpInfo.Last_Name = UserDE.Properties["sn"].Value.ToString();
        //                            SearchEmpInfo.CAI = UserDE.Properties["cvx-cai"].Value.ToString();
        //                            SearchEmpInfo.E_Mail = UserDE.Properties["mail"].Value.ToString();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return SearchEmpInfo;
        //}

        public static CurrentUser GetUserByID(string ID)
        {
            CurrentUser user = new CurrentUser();

            if (!string.IsNullOrEmpty(ID))
            {
                using (DirectorySearcher searcher = new DirectorySearcher(new DirectoryEntry()))
                {
                    searcher.PageSize = 1000;
                    searcher.SearchScope = SearchScope.Subtree;
                    searcher.Filter = "(&(samAccountType=805306368)(sAMAccountName=" + ID + "))";

                    using (SearchResultCollection Results = searcher.FindAll())
                    {
                        if ((Results != null) && Results.Count != 0)
                        {
                            using (DirectoryEntry UserDE = Results[0].GetDirectoryEntry())
                            {
                                user.FirstName = UserDE.Properties["givenName"].Value.ToString();
                                user.LastName = UserDE.Properties["sn"].Value.ToString();
                                user.CAI = UserDE.Properties["cvx-cai"].Value.ToString().ToUpper();
                            }
                        }
                    }
                }
            }
            return user;
        }

        //public static List<string> GetCurrentUseriDACSGroup()
        //{
        //    List<string> groupList = new List<string>();
        //    string grp = string.Empty;

        //    UserPrincipal user = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain, "CT"), IdentityType.SamAccountName, "BAIN");
        //    foreach (GroupPrincipal group in user.GetGroups())
        //    {
        //        //Console.Out.WriteLine(group);
        //        if (group.ToString().Contains("iDACS"))
        //            groupList.Add(group.ToString());
        //    }
        //    return groupList;
        //}

        //public static bool isSiteAdmin()
        //{
        //    bool isAdmin=false;

        //    string iUser = CurrentUserAD.GetCurrentUser().CAI;

        //    UserPrincipal user = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain, "CT"), IdentityType.SamAccountName, iUser);
        //    foreach (GroupPrincipal group in user.GetGroups())
        //    {
        //        //Console.Out.WriteLine(group);
        //        if (group.ToString().Contains(ConstantsAD.SiteAdminGropu))
        //        {
        //            isAdmin = true;
        //            break;
        //        }
        //    }
        //    return false;

        //   // return isAdmin;
        //}

        //public static List<Enums.ADGroupToLocations> Admin_Location_List()
        //{
        //    List<Enums.ADGroupToLocations> groupList = new List<Enums.ADGroupToLocations>();
        //    string grp = string.Empty;

        //    UserPrincipal user = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain, "CT"), IdentityType.SamAccountName, "BAIN");
        //    foreach (GroupPrincipal group in user.GetGroups())
        //    {
        //        if (group.ToString().Contains("DHAK94 iDACS Site Admins"))
        //            groupList.Add(Enums.ADGroupToLocations.DHAK94_iDACS_Site_Admins);
        //        else if (group.ToString().Contains("DHAK94 iDACS Admins"))
        //            groupList.Add(Enums.ADGroupToLocations.DHAK94_iDACS_Admins);
        //        else if (group.ToString().Contains("SYLLAC iDACS Admins"))
        //            groupList.Add(Enums.ADGroupToLocations.SYLLAC_iDACS_Admins);
        //    }
        //    return groupList;
        //}


        //public static string Admin_Location(string iUser)
        //{
        //    int locationCode=0;

        //    UserPrincipal user = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain, ConstantsAD.Domain), IdentityType.SamAccountName, iUser);
        //    foreach (GroupPrincipal group in user.GetGroups())
        //    {
        //        if (group.ToString().Contains("iDACS"))
        //        {
        //            string EnamName = group.ToString().Replace(' ', '_');

        //            Enums.ADGroupToLocations enumLocationCode = (Enums.ADGroupToLocations)Enum.Parse(typeof(Enums.ADGroupToLocations), EnamName);

        //            locationCode = (int)enumLocationCode;

        //           // locationCode = 1;

        //        }
        //    }

        //    return locationCode.ToString();
        //}

        //public static string[] Admin_Locations()
        //{

        //    string iUser = CurrentUserAD.GetCurrentUser().CAI;
        //    string locationCodes = string.Empty;

        //    bool bAdmin = false;

        //    UserPrincipal user = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain, ConstantsAD.Domain), IdentityType.SamAccountName, iUser);
        //    foreach (GroupPrincipal group in user.GetGroups())
        //    {
        //        if (group.ToString().Contains("iDACS"))
        //        {

        //            if (!string.IsNullOrEmpty(locationCodes))
        //                locationCodes += ",";

        //            string EnamName = group.ToString().Replace(' ', '_');

        //            Enums.ADGroupToLocations enumLocationCode = (Enums.ADGroupToLocations)Enum.Parse(typeof(Enums.ADGroupToLocations), EnamName);

        //            int locCode = (int)enumLocationCode;

        //            if (locCode == 0)
        //                bAdmin = true;

        //            locationCodes += locCode;

        //            // locationCode = 1;

        //        }
        //    }

        //    if (bAdmin)
        //    {
        //        locationCodes = string.Empty;
        //        foreach (Enums.ADGroupToLocations AD in Enum.GetValues(typeof(Enums.ADGroupToLocations)))
        //        {
        //            if (!string.IsNullOrEmpty(locationCodes))
        //                locationCodes += ",";
        //            int locCode = (int)AD;
        //            locationCodes += locCode;
        //        }
        //    }

        //   // locationCodes = "1,2,3,4,5,19";

        //    return locationCodes.ToString().Split(',');
        //}


        //public IEnumerable<Tuple<string, int>> GetEnumValuePairs(Type enumType)
        //{
        //    if (!enumType.IsEnum)
        //    {
        //        throw new ArgumentException();
        //    }

        //    List<Tuple<string, int>> result = new List<Tuple<string, int>>();

        //    foreach (var value in Enum.GetValues(enumType))
        //    {
        //        string fieldName = Enum.GetName(enumType, value);

        //        FieldInfo fieldInfo = enumType.GetField(fieldName);
        //        var descAttribute = fieldInfo.GetCustomAttributes(false).Where(a => a is DescriptionAttribute).Cast<DescriptionAttribute>().FirstOrDefault();

        //        // ideally check if descAttribute is null here
        //        result.Add(Tuple.Create(descAttribute.Description, (int)value));
        //    }

        //    return result;
        //}



        //public static CurrentUser GetCurrentUser()
        //{
        //    string myLoginID = "", LogonUser = "";
        //    LogonUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

        //    CurrentUser user = new CurrentUser();

        //    int startIndex = LogonUser.IndexOf("\\") + 1;
        //    myLoginID = LogonUser.Substring(startIndex, LogonUser.Length - startIndex);

        //    if (!string.IsNullOrEmpty(myLoginID))
        //    {
        //        using (DirectorySearcher searcher = new DirectorySearcher(new DirectoryEntry()))
        //        {
        //            searcher.PageSize = 1000;
        //            searcher.SearchScope = SearchScope.Subtree;
        //            searcher.Filter = "(&(samAccountType=805306368)(sAMAccountName=" + myLoginID + "))";

        //            using (SearchResultCollection Results = searcher.FindAll())
        //            {
        //                if ((Results != null) && Results.Count != 0)
        //                {
        //                    using (DirectoryEntry UserDE = Results[0].GetDirectoryEntry())
        //                    {
        //                        user.FirstName = UserDE.Properties["givenName"].Value.ToString();
        //                        user.LastName = UserDE.Properties["sn"].Value.ToString();
        //                        user.CAI = UserDE.Properties["cvx-cai"].Value.ToString();  
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return user;
        //}


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
    }
}
