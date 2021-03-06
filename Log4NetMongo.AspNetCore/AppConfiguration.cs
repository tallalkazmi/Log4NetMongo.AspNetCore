﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Log4NetMongo.AspNetCore
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

        public static string GetSectionValue(string sectionName, string sectionkey)
        {
            return configurationRoot.GetSection(sectionName).GetSection(sectionkey).Value;
        }

        public static List<KeyValuePair<string, string>> GetSectionKeys(string sectionName)
        {
            return configurationRoot.GetSection(sectionName).GetChildren().Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToList();
        }
    }
}
