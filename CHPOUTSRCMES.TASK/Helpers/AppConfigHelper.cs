using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.Helpers
{
    public static class AppConfigHelper
    {
        const string APP_SETTING_ERROR_MESSAGE = "Invalid or missing appSetting, ";

        public static string GetString(string appSettingName)
        {
            if (ConfigurationManager.AppSettings[appSettingName] != null && !String.IsNullOrEmpty(ConfigurationManager.AppSettings[appSettingName].ToString()))
            {
                return ConfigurationManager.AppSettings[appSettingName].ToString();
            }
            else
            {
                throw new Exception(APP_SETTING_ERROR_MESSAGE + appSettingName);
            }
        }

        public static double GetDouble(string appSettingName)
        {
            double doubleValue = 0;
            if (ConfigurationManager.AppSettings[appSettingName] == null || !double.TryParse(ConfigurationManager.AppSettings[appSettingName].ToString(), out doubleValue))
            {
                throw new Exception(APP_SETTING_ERROR_MESSAGE + appSettingName);
            }
            return doubleValue;
        }

        public static int GetInt(string appSettingName)
        {
            int intValue = 0;
            if (ConfigurationManager.AppSettings[appSettingName] == null || !int.TryParse(ConfigurationManager.AppSettings[appSettingName].ToString(), out intValue))
            {
                throw new Exception(APP_SETTING_ERROR_MESSAGE + appSettingName);
            }
            return intValue;
        }

        public static void SetValue(string appSettingName, String value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if(config.AppSettings.Settings[appSettingName] != null)
            {
                config.AppSettings.Settings[appSettingName].Value = value;
            }
            else
            {
                config.AppSettings.Settings.Add(appSettingName, value);
            }
            
            config.Save();
        }
        
    }

}
