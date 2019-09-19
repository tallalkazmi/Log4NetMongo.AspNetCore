using Log4NetMongo.AspNetCore;
using System;
using Xunit;

namespace Log4NetMongo.Test
{
    public class UnitTest
    {
        [Fact]
        public void TestMethod_with_config_values()
        {
            AppLogger logger = new AppLogger();
            logger.LogInfo($"Test Info @ {DateTime.UtcNow}");
            Console.WriteLine("See output for exceptions. Completed successfully");
        }

        [Fact]
        public void TestMethod_with_param_values()
        {
            AppLogger logger = new AppLogger("Log4NetMongo.Core", "Test");
            logger.LogInfo($"Test Info @ {DateTime.UtcNow}");
            Console.WriteLine("See output for exceptions. Completed successfully");
        }

        [Fact]
        public void TestMethod_with_data_values()
        {
            AppLogger logger = new AppLogger();
            DataStruct data = new DataStruct()
            {
                id = 1,
                IsActive = true,
                message = "Data message"
            };

            logger.LogDebug($"Test Debug @ {DateTime.UtcNow}", data);
            Console.WriteLine("See output for exceptions. Completed successfully");
        }

        class DataStruct
        {
            public int id { get; set; }
            public string message { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
