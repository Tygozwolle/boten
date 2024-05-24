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
        public static string DBAdress
        {
            get
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
            private set
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["dbadress"] != null)
                {
                    confCollection["dbadress"].Value = value;
                }
                else
                {
                    confCollection.Add("dbadress", value);
                }


                configManager.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
            }
        }

        public static void SetDBAdress(string dbAdress)
        {
            DBAdress = dbAdress;
        }

        public static string DBUsername
        {
            get
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
            private set
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["dbusername"] != null)
                {
                    confCollection["dbusername"].Value = value;
                }
                else
                {
                    confCollection.Add("dbusername", value);
                }

                configManager.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
            }
        }

        public static void SetDBUsername(string dbUsername)
        {
            DBUsername = dbUsername;
        }

        public static string DBPassword
        {
            get
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["dbpassword"] != null)
                {
                    return confCollection["dbpassword"].Value;
                }
                else
                {
                    return null;
                }
            }
            private set
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["dbpassword"] != null)
                {
                    confCollection["dbpassword"].Value = value;
                }
                else
                {
                    confCollection.Add("dbpassword", value);
                }

                configManager.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
            }
        }

        public static void SetDBPassword(string password)
        {
            DBPassword = password;
        }

        public static string DBPort
        {
            get
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
            private set
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["dbport"] != null)
                {
                    confCollection["dbport"].Value = value;
                }
                else
                {
                    confCollection.Add("dbport", value);
                }

                configManager.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
            }
        }

        public static UInt32 DBPortInt
        {
            get
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["dbport"] != null)
                {
                    try
                    {
                        return UInt32.Parse(confCollection["dbport"].Value);
                    }
                    catch { return 0; }
                }
                else
                {
                    return 0;
                }
            }
        }

        public static void SetDBPort(string port)
        {
            DBPort = port;
        }

        public static string ControlUsername
        {
            get
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
            private set
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["controlusername"] != null)
                {
                    confCollection["controlusername"].Value = value;
                }
                else
                {
                    confCollection.Add("controlusername", value);
                }

                configManager.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
            }
        }

        public static void SetControlUsername(string username)
        {
            ControlUsername = username;
        }

        public static string ControlPassword
        {
            get
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
            private set
            {
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;

                if (confCollection["controlpassword"] != null)
                {
                    confCollection["controlpassword"].Value = value;
                }
                else
                {
                    confCollection.Add("controlpassword", value);
                }

                configManager.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
            }
        }

        public static void SetControlPassword(string password)
        {
            ControlPassword = password;
        }

#if !RELEASE
        public static void FillFromSecrets()
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Config>().Build();
            DBUsername = config["DB:username"];
            DBPassword = config["DB:password"];
            DBPort = config["DB:port"];
            DBAdress = config["DB:adress"];
        }
#endif
    }
}
