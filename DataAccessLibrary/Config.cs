using System.Configuration;
using Microsoft.Extensions.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace DataAccessLibrary;

public abstract class Config
{
    public static string DBAdress
    {
        get
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var confCollection = configManager.AppSettings.Settings;

            if (confCollection["dbadress"] != null)
                return confCollection["dbadress"].Value;
            return null;
        }
        private set
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var confCollection = configManager.AppSettings.Settings;

            if (confCollection["dbadress"] != null)
                confCollection["dbadress"].Value = value;
            else
                confCollection.Add("dbadress", value);


            configManager.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
        }
    }

    public static string DBUsername
    {
        get
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var confCollection = configManager.AppSettings.Settings;

            if (confCollection["dbusername"] != null)
                return confCollection["dbusername"].Value;
            return null;
        }
        private set
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var confCollection = configManager.AppSettings.Settings;

            if (confCollection["dbusername"] != null)
                confCollection["dbusername"].Value = value;
            else
                confCollection.Add("dbusername", value);

            configManager.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
        }
    }

    public static string DBPassword
    {
        get
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var confCollection = configManager.AppSettings.Settings;

            if (confCollection["dbpassword"] != null)
                return confCollection["dbpassword"].Value;
            return null;
        }
        private set
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var confCollection = configManager.AppSettings.Settings;

            if (confCollection["dbpassword"] != null)
                confCollection["dbpassword"].Value = value;
            else
                confCollection.Add("dbpassword", value);

            configManager.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
        }
    }

    public static string DBPort
    {
        get
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var confCollection = configManager.AppSettings.Settings;

            if (confCollection["dbport"] != null)
                return confCollection["dbport"].Value;
            return null;
        }
        private set
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var confCollection = configManager.AppSettings.Settings;

            if (confCollection["dbport"] != null)
                confCollection["dbport"].Value = value;
            else
                confCollection.Add("dbport", value);

            configManager.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
        }
    }

    public static uint DBPortInt
    {
        get
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var confCollection = configManager.AppSettings.Settings;

            if (confCollection["dbport"] != null)
                try
                {
                    return uint.Parse(confCollection["dbport"].Value);
                }
                catch
                {
                    return 0;
                }

            return 0;
        }
    }

    public static string ControlUsername
    {
        get
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var confCollection = configManager.AppSettings.Settings;

            if (confCollection["controlusername"] != null)
                return confCollection["controlusername"].Value;
            return null;
        }
        private set
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var confCollection = configManager.AppSettings.Settings;

            if (confCollection["controlusername"] != null)
                confCollection["controlusername"].Value = value;
            else
                confCollection.Add("controlusername", value);

            configManager.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
        }
    }

    public static string ControlPassword
    {
        get
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var confCollection = configManager.AppSettings.Settings;

            if (confCollection["controlpassword"] != null)
                return confCollection["controlpassword"].Value;
            return null;
        }
        private set
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var confCollection = configManager.AppSettings.Settings;

            if (confCollection["controlpassword"] != null)
                confCollection["controlpassword"].Value = value;
            else
                confCollection.Add("controlpassword", value);

            configManager.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
        }
    }

    public static void SetDBAdress(string dbAdress)
    {
        DBAdress = dbAdress;
    }

    public static void SetDBUsername(string dbUsername)
    {
        DBUsername = dbUsername;
    }

    public static void SetDBPassword(string password)
    {
        DBPassword = password;
    }

    public static void SetDBPort(string port)
    {
        DBPort = port;
    }

    public static void SetControlUsername(string username)
    {
        ControlUsername = username;
    }

    public static void SetControlPassword(string password)
    {
        ControlPassword = password;
    }

#if !RELEASE
    public static void FillFromSecrets()
    {
        var config = new ConfigurationBuilder().AddUserSecrets<Config>().Build();
        DBUsername = config["DB:username"];
        DBPassword = config["DB:password"];
        DBPort = config["DB:port"];
        DBAdress = config["DB:adress"];
    }
#endif
}