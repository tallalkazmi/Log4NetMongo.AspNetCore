using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Log4NetMongo.Appender
{
    public static class AppConfiguration
    {
        static readonly IConfigurationRoot configurationRoot;
        static AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            configurationRoot = configurationBuilder.Build();
        }

        public static string GetConnectionString(string key)
        {
            if (configurationRoot.GetSection("ConnectionStrings").Exists() == false)
                throw new InvalidOperationException("The Section 'ConnectionStrings' does not exists");

            return configurationRoot.GetConnectionString(key);
        }
    }
}

