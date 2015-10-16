using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace andbeans
{
    public class Config
    {
        public T Get<T>(string key)
        {
            // If the key is null (not present in the file)
            if (ConfigurationManager.AppSettings[key] == null)
                throw new Exception("Configuration Key Not Found: " + key);

            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }
    }
}