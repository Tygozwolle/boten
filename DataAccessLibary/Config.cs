using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace DataAccessLibary
{
    public abstract class Config
    {
        public static void SetDBAdress(string adress)
        {
            Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

            if (confCollection["dbadress"] != null)
            {
                confCollection["dbadress"].Value = adress;
            }
            else
            {
                confCollection.Add("dbadress", adress);
            }


            configManager.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
        }

        public static void SetDBUsername(string username)
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["dbusername"] != null)
                {
                    confCollection["dbusername"].Value = username;
                }
                else
                {
                    confCollection.Add("dbusername", username);
                }

                configManager.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
            }
            public static void SetDBPassword(string password)
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["dbpassword"] != null)
                {
                    confCollection["dbpassword"].Value = password;
                }
                else
                {
                    confCollection.Add("dbpassword", password);
                }

                configManager.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
            }
            public static void SetDBPort(string port)
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["dbport"] != null)
                {
                    confCollection["dbport"].Value = port;
                }
                else
                {
                    confCollection.Add("dbport", port);
                }

                configManager.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
            }

            public static void SetControlUsername(string username)
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["controlusername"] != null)
                {
                    confCollection["controlusername"].Value = username;
                }
                else
                {
                    confCollection.Add("controlusername", username);
                }

                configManager.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
            }
            public static void SetControlPassword(string password)
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["controlpassword"] != null)
                {
                    confCollection["controlpassword"].Value = password;
                }
                else
                {
                    confCollection.Add("controlpassword", password);
                }

                configManager.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
            }

        public static string GetDBAdress()
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["dbadress"] != null)
                {
                    return confCollection["dbadress"].Value;
                }
                else
                {
                    return null;
                }
            }
            public static string GetDBUsername()
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["dbusername"] != null)
                {
                    return confCollection["dbusername"].Value;
                }
                else
                {
                    return null;
                }
            }

        public static string GetDBPassword()
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["dbpassword"] != null)
                {
                  return  confCollection["dbpassword"].Value;
                }
                else
                {
                    return null;
                }
            }
        public static string GetDBPort()
        {
            Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

            if (confCollection["dbport"] != null)
            {
                return confCollection["dbport"].Value;
            }
            else
            {
                return null;
            }
        }

        public static string GetControlUsername()
        {
            Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

            if (confCollection["controlusername"] != null)
            {
                return confCollection["controlusername"].Value;
            }
            else
            {
                return null;
            }
        }

        public static string GetControlPassword()
        {
            Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

            if (confCollection["controlpassword"] != null)
            {
                return confCollection["controlpassword"].Value;
            }
            else
            {
                return null;
            }
        }
    }
}
