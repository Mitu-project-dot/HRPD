using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Chevron.HRPD.Common.Helpers
{
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Retrieves a setting from the AppSettings collection
        /// </summary>
        /// <param name="key">Key name</param>
        /// <param name="defaultValue">If the key is not found, this value is returned</param>
        public static T GetConfigValue<T>(string key, T defaultValue)
        {
            if (ConfigurationManager.AppSettings[key] == null)
            {
                return defaultValue;
            }

            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }

        /// <summary>
        /// Retrieves a settings from the AppSettings collection. If the key is not found, an exception is thrown
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetConfigValue<T>(string key)
        {
            if (ConfigurationManager.AppSettings[key] == null)
            {
                throw new ArgumentException("The key was not found in the configuration file");
            }

            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));            
        }

        /// <summary>
        /// Retrieves a section from the application's configuration file. If the section is not found, an exception is thrown.
        /// </summary>
        public static T GetSection<T>(string sectionName)
        {
            var section = ConfigurationManager.GetSection(sectionName);

            if (section == null)
            {
                throw new ArgumentException("The section was not found in the configuration file");
            }

            return (T)section;
        }
    }
}
