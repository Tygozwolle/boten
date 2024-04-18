using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVerenigingLibary
{
    public  class Secrets
    {
        private IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Secrets>().Build();
        public string DBAdress()
        {
            return config["DB:adress"];
        }
        public string DBUserName()
        {
            return config["DB:username"];
        }
        public string DBPassWord()
        {
            return config["DB:password"];
        }
    }
}
