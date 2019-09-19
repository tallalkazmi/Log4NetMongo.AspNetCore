using log4net;
using log4net.Config;
using log4net.Util;
using Log4NetMongo.Core;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Log4NetMongo.Console
{
    class Program
    {
        private static readonly TimeSpan Interval = TimeSpan.FromMilliseconds(100);
        private static int _count;
        static void Main(string[] args)
        {
            System.Console.WriteLine("InternalDebugging");

            #region ConfigXML
            string xml = @"<?xml version='1.0' encoding='utf-8' ?>
                <configuration>
                    <configSections>
                        <section name='log4net' type='log4net.Config.Log4NetConfigurationSectionHandler, log4net' />
                    </configSections>
                    <log4net>
                        <appender name='ConsoleAppender' type='log4net.Appender.ConsoleAppender'>
                            <layout type='log4net.Layout.SimpleLayout' />
                        </appender>
                        <appender name='MongoDBAppender' type='Log4NetMongo.Appender.MongoDBAppender, Log4NetMongo.Appender'>
                            <connectionString value='mongodb://appuser:admin#123@CAN-ALPHA:9010/MACH_LOG_CORE?authSource=admin' />
                        </appender>
                        <root>
                            <level value='ALL' />
                            <appender-ref ref='MongoDBAppender' />
                            <appender-ref ref='ConsoleAppender' />
                        </root>
                    </log4net>
                </configuration>";
            #endregion

            LogLog.InternalDebugging = true;
            var loggerRepository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(xml)))
            {
                XmlConfigurator.Configure(loggerRepository, stream);
            }

            ILog log = LogManager.GetLogger(typeof(Program));
            log.Info("Starting");

            //while (!System.Console.KeyAvailable)
            while (_count < 100)
            {
                log.Info(++_count);
                Thread.Sleep(Interval);
            }
            System.Console.WriteLine("Completed");

            //--
            System.Console.WriteLine("AppLogger Debugging");

            AppLogger logger = new AppLogger();
            logger.LogDebug("test debug message");

            System.Console.WriteLine("Completed");
        }
    }
}
