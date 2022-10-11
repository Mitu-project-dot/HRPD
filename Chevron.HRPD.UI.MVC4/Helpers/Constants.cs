using System;
using System.Configuration;
using System.Reflection;

namespace Chevron.HRPD.UI.MVC4.Helpers
{
    public class ApplicationSettings
    {
        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        public static string ApplicationName
        {
            get { return ConfigurationManager.AppSettings["ApplicationName"]; }
        }

        /// <summary>
        /// Gets the name of the operating company.
        /// </summary>
        /// <value>The name of the operating company.</value>
        public static string OperatingCompanyName
        {
            get { return ConfigurationManager.AppSettings["OperatingCompanyName"]; }
        }

        /// <summary>
        /// Gets the operating company link.
        /// </summary>
        /// <value>The operating company link.</value>
        public static string OperatingCompanyLink
        {
            get { return ConfigurationManager.AppSettings["OperatingCompanyLink"]; }
        }

        /// <summary>
        /// Gets the last updated date
        /// </summary>
        /// <value>The last updated.</value>
        public static string LastUpdated
        {
            get { return ConfigurationManager.AppSettings["LastUpdated"]; }
        }

        /// <summary>
        /// Gets the name of the content contact of the site
        /// </summary>
        /// <value>The name of the content contact.</value>
        public static string ContentContactName
        {
            get { return ConfigurationManager.AppSettings["ContentContactName"]; }
        }

        /// <summary>
        /// Gets the content contact link of the site
        /// </summary>
        /// <value>The content contact link.</value>
        public static string ContentContactLink
        {
            get { return ConfigurationManager.AppSettings["ContentContactLink"]; }
        }

        /// <summary>
        /// Gets the name of the technical contact for the site
        /// </summary>
        /// <value>The name of the technical contact.</value>
        public static string TechnicalContactName
        {
            get { return ConfigurationManager.AppSettings["TechnicalContactName"]; }
        }

        /// <summary>
        /// Gets the technical contact link for the site
        /// </summary>
        /// <value>The technical contact link.</value>
        public static string TechnicalContactLink
        {
            get { return ConfigurationManager.AppSettings["TechnicalContactLink"]; }
        }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>The environment.</value>
        public static string Environment
        {
            get { return ConfigurationManager.AppSettings["Environment"]; }
        }

        /// <summary>
        /// Gets a value indicating whether [show version information].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [show version information]; otherwise, <c>false</c>.
        /// </value>
        public static bool ShowVersionInformation
        {
            get { return Boolean.Parse(ConfigurationManager.AppSettings["ShowVersionInformation"]); }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public static string ApplicationVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        /// <summary>
        /// Gets the copyright. message for the site footer
        /// </summary>
        /// <value>The copyright.</value>
        public static string Copyright
        {
            get { return string.Format("{0} Chevron Corporation", DateTime.Now.Year); }
        }
    }
}