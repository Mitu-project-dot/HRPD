using Microsoft.VisualBasic;
using System.Security.Principal;
using System.Web;
using System.Web.Util;
using System.Web.SessionState;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Data.SqlClient;
using System;
using System.Configuration;

namespace Chevron.HRPD.Security
{
    [Serializable]
    public class CurrentUser : GenericIdentity
    {

        #region "Fields"

        private string _CAI;

        private string _email;

        private string _firstName;

        private string _lastName;

        private int _roleID;

        private string _roleName;

        private int _sysUserID;

        #endregion


        #region "Properties"

        public string CAI { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string LastName { get; set; }

        public int RoleID { get; set; }

        public string RoleName { get; set; }

        public int SysUserID { get; set; }

        #endregion

        protected CurrentUser()
            : this(HttpContext.Current.User.Identity.Name)
        {
            
        }

        //Note, ID can be CAI or logon id
        public CurrentUser(string LogonID_or_CAI_or_Email)
            : base(LogonID_or_CAI_or_Email, "Windows")
        {

            if ((LogonID_or_CAI_or_Email == null) || (LogonID_or_CAI_or_Email.Length == 0))
            {
                throw (new ArgumentException("Logon ID of the current user is empty. Make sure to enable Windows authentication on the website."));
            }

         //   PopulateUserAttrsFromDirectory(LogonID_or_CAI_or_Email);

            SetUserRole();
        }

        private void PopulateUserAttrsFromDirectory(string LogonID_or_CAI_or_Email)
        {

            if (LogonID_or_CAI_or_Email.Contains("\\"))
            {
                LogonID_or_CAI_or_Email = LogonID_or_CAI_or_Email.Split('\\')[1];
            }
        
            GPDA.GPDASoapClient ws = new GPDA.GPDASoapClient(new WseKerberosBinding(), new EndpointAddress(new Uri("http://ws-hr-gpda.chevron.com/gpda.asmx"),EndpointIdentity.CreateSpnIdentity("http/ws-hr-gpda.chevron.com")));
        
            ws.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;

            GPDA.InputParameters input =  new GPDA.InputParameters();
        
            input.LoginID = LogonID_or_CAI_or_Email;
        
           GPDA.ContactInfo contactInfo;
        
            try
            {
                contactInfo = ws.getContactInfo(input);


                if (contactInfo.CAI == "")
                {
                    input = new GPDA.InputParameters();

                    input.CAI = LogonID_or_CAI_or_Email;

                    contactInfo = ws.getContactInfo(input);
                }

                if (contactInfo.CAI == "" && LogonID_or_CAI_or_Email.Contains("@"))
                {
                    input = new GPDA.InputParameters();

                    input.Chevron_Primary_Email_Address = LogonID_or_CAI_or_Email;

                    contactInfo = ws.getContactInfo(input);
                }

                if (contactInfo.CAI == "")
                {
                    CAI = LogonID_or_CAI_or_Email;
                }
                else
                {
                    CAI = contactInfo.CAI;

                    Email = contactInfo.Chevron_Email;

                    FirstName = contactInfo.First_Name;

                    LastName = contactInfo.Last_Name;


                }
            }
            
            catch (System.Exception ex)
            {
                ws.Abort();
            
                throw;
            }
                                    
        finally
            {
                if (ws.State == CommunicationState.Opened) 
                {
                    ws.Close();
                }
            }
            
        

        }

        public Boolean IsUserInRole(string roleName)
        {
            return HttpContext.Current.User.IsInRole(RoleID.ToString());
        }

        private void fillAlternateContactInfo()
        {
            GPDA.InputParameters input = new GPDA.InputParameters();

            input.CAI = this.CAI;
            

            GPDA.GPDASoapClient ws = new GPDA.GPDASoapClient(new WseKerberosBinding(), new EndpointAddress(new Uri("http://ws-hr-gpda.chevron.com/gpda.asmx"),EndpointIdentity.CreateSpnIdentity("http/ws-hr-gpda.chevron.com")));

            ws.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
            
            GPDA.AlternateContactInfo altInfo;

            try
            {
                altInfo = ws.getAlternateContactInfo(input);
            }

            catch
            {
                ws.Abort();
            
                throw;
            }

            finally
            {

             if (ws.State == CommunicationState.Opened) 
                {
                    ws.Close();
                }
            
            }
        }

        private void fillBillingInfo()
        { 
             GPDA.InputParameters input = new GPDA.InputParameters();

            input.CAI = this.CAI;
            

            GPDA.GPDASoapClient ws = new GPDA.GPDASoapClient(new WseKerberosBinding(), new EndpointAddress(new Uri("http://ws-hr-gpda.chevron.com/gpda.asmx"),EndpointIdentity.CreateSpnIdentity("http/ws-hr-gpda.chevron.com")));

            ws.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;

            GPDA.BillingAndSourcingInfo billInfo;

            try
            {
                billInfo = ws.getBillingAndSourcingInfo(input);
            }
            catch
            {
                ws.Abort();
            
                throw;
            }

            finally
            {

             if (ws.State == CommunicationState.Opened) 
                {
                    ws.Close();
                }
            
            }
        }

        private void fillOrgInfo()
        { 
            GPDA.InputParameters input = new GPDA.InputParameters();

            input.CAI = this.CAI;

            GPDA.GPDASoapClient ws = new GPDA.GPDASoapClient(new WseKerberosBinding(), new EndpointAddress(new Uri("http://ws-hr-gpda.chevron.com/gpda.asmx"),EndpointIdentity.CreateSpnIdentity("http/ws-hr-gpda.chevron.com")));

            ws.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;

            GPDA.OrganizationInfo orgInfo;

            try
            {
                orgInfo = ws.getOrganizationInfo(input);
            }

            catch
            {
                ws.Abort();
            
                throw;
            }

            finally
            {

             if (ws.State == CommunicationState.Opened) 
                {
                    ws.Close();
                }
            
            }
        }

        private void fillSysInfo()
        {
            GPDA.InputParameters input = new GPDA.InputParameters();

            input.CAI = this.CAI;

            GPDA.GPDASoapClient ws = new GPDA.GPDASoapClient(new WseKerberosBinding(), new EndpointAddress(new Uri("http://ws-hr-gpda.chevron.com/gpda.asmx"),EndpointIdentity.CreateSpnIdentity("http/ws-hr-gpda.chevron.com")));

            ws.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;

            GPDA.SystemInfo sysInfo;

            try
            {
            sysInfo = ws.getSystemInfo(input);
            }

            catch
            {
                ws.Abort();
            
                throw;
            }

            finally
            {

             if (ws.State == CommunicationState.Opened) 
                {
                    ws.Close();
                }
            
            }
        }

        //Checks if user is member of a particular AD group based on group name or group DN (Distinguished Name)
        public Boolean IsUserInADGroup(string ADGroupNameORGroupDN)
        {
            GPDA.InputParameters input = new GPDA.InputParameters();

            input.CAI = this.CAI;

            input.LookupMethod = "GROUPTREE";

            GPDA.GPDASoapClient ws = new GPDA.GPDASoapClient(new WseKerberosBinding(), new EndpointAddress(new Uri("http://ws-hr-gpda.chevron.com/gpda.asmx"),EndpointIdentity.CreateSpnIdentity("http/ws-hr-gpda.chevron.com")));

            ws.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;

            try
            {
                input.Group_Name = ADGroupNameORGroupDN;
                if(ws.IsUserMemberOfGroup(input))
                {
                    return true;
                }
                else
                {
                    input.Group_Name = "";
                    input.Group_DN = ADGroupNameORGroupDN;
                    if(ws.IsUserMemberOfGroup(input))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            catch
            {
                ws.Abort();
            
                throw;
            }

            finally
            {

             if (ws.State == CommunicationState.Opened) 
                {
                    ws.Close();
                }
            
            }
        }

        protected virtual void SetUserRole()
        {

        }

        //Gets current user
        public static CurrentUser GetCurrentUser()
        {
            CurrentUser retVal;

            Object cachedObject = HttpContext.Current.Cache[HttpContext.Current.User.Identity.Name];

            //Checks if user is already stored in cache
            if (cachedObject == null)
            {
                retVal = new CurrentUser();
                HttpContext.Current.Cache.Insert(HttpContext.Current.User.Identity.Name, retVal, null, System.Web.Caching.Cache.NoAbsoluteExpiration,TimeSpan.FromSeconds(GetPersonCacheTime()));
            }
            else
            {
                retVal = (CurrentUser)cachedObject;
            }

            return retVal;
        }

        //Gets cache information expiration time previously set in web.config
        private static int GetPersonCacheTime()
        {
            AppSettingsReader configReader = new AppSettingsReader();

            int cacheSeconds = (int)configReader.GetValue("PersonCacheSeconds", typeof(int));

            return cacheSeconds;
        }

    }
}
