using log4net;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Log4NetMongo.AspNetCore
{
    public class AppLogger : BaseLogger
    {
        readonly ILog target;

        /// <summary>
        /// Initialize Log4Net with optional settings.
        /// </summary>
        /// <param name="application">Application's name. If not provided reads from ApplicationSettings key 'ApplicationName'</param>
        /// <param name="environment">Environment's name. If not provided reads from ApplicationSettings key 'Environment'</param>
        /// <param name="connectionString">MongoDb ConnectionString. If not provided reads from ConnectionStrings name 'MongoLogConnection'</param>
        /// <param name="collectionName">MongoDb Collection's name. If not provided reads from ApplicationSettings key 'MongoLogCollectionName'</param>
        /// <param name="logLevel">Log4Net log level. Default is set to 'ALL'. If not provided reads from ApplicationSettings key 'LogLevel'</param>
        public AppLogger(string application = null,
        string environment = null,
        string connectionString = null,
        string collectionName = null,
        LogLevel? logLevel = null)
        {
            var AppSettingKeyValue = AppConfiguration.GetSectionKeys("ApplicationSettings").ToList();
            var ConnectionStringKeyValue = AppConfiguration.GetSectionKeys("ConnectionStrings").ToList();

            base.Application = string.IsNullOrWhiteSpace(application) ?
                AppSettingKeyValue.Any(x => x.Key == "ApplicationName") ?
                AppSettingKeyValue.Where(x => x.Key == "ApplicationName").FirstOrDefault().Value
                : throw new ArgumentNullException("Key 'ApplicationName' does not exists.") : application;

            base.Environment = string.IsNullOrWhiteSpace(environment) ?
                AppSettingKeyValue.Any(x => x.Key == "Environment") ?
                AppSettingKeyValue.Where(x => x.Key == "Environment").FirstOrDefault().Value
                : throw new ArgumentNullException("Key 'Environment' does not exists.") : environment;

            base.ConnectionString = string.IsNullOrWhiteSpace(connectionString) ?
                ConnectionStringKeyValue.Any(x => x.Key == "MongoLogConnection") ?
                ConnectionStringKeyValue.Where(x => x.Key == "MongoLogConnection").FirstOrDefault().Value
                : throw new ArgumentNullException("Key 'MongoLogConnection' does not exists.") : connectionString;

            base.Collection = string.IsNullOrWhiteSpace(collectionName) ?
                AppSettingKeyValue.Any(x => x.Key == "MongoLogCollectionName") ?
                AppSettingKeyValue.Where(x => x.Key == "MongoLogCollectionName").FirstOrDefault().Value
                : throw new ArgumentNullException("Key 'MongoLogCollectionName' does not exists.") : collectionName;

            if (AppSettingKeyValue.Any(x => x.Key == "LogLevel"))
            {
                string _logLevelString = AppSettingKeyValue.Where(x => x.Key == "LogLevel").FirstOrDefault().Value;
                bool isParsed = Enum.TryParse(_logLevelString, out LogLevel _logLevel);
                if (isParsed) LogLevel = _logLevel;
                else throw new InvalidCastException("Key 'LogLevel' is not of an acceptable value. Please set from one of the following: All, Debug, Info, Warn, Error, Fatal, Off");
            }
            base.LogLevel = logLevel;

            target = GetConfiguredLog();
        }

        public void LogDebug(string message, object data = null)
        {
            StackTrace stackTrace = new StackTrace(true);
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();
            SetThreadContextProperties(methodBase.Name, stackFrame.GetFileName(), stackFrame.GetFileLineNumber().ToString());

            if (data != null)
            {
                string dataJson = JsonConvert.SerializeObject(data);
                ThreadContext.Properties["data"] = dataJson;
            }

            target.Debug(message);
        }

        public void LogDebug(string message, Exception exception)
        {
            StackTrace stackTrace = new StackTrace(true);
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();
            SetThreadContextProperties(methodBase.Name, stackFrame.GetFileName(), stackFrame.GetFileLineNumber().ToString());
            target.Debug(message, exception);
        }

        public void LogError(string message, object data = null)
        {
            StackTrace stackTrace = new StackTrace(true);
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();
            SetThreadContextProperties(methodBase.Name, stackFrame.GetFileName(), stackFrame.GetFileLineNumber().ToString());
            if (data != null)
            {
                string dataJson = JsonConvert.SerializeObject(data);
                ThreadContext.Properties["data"] = dataJson;
            }
            target.Error(message);
        }

        public void LogError(string message, Exception exception)
        {
            StackTrace stackTrace = new StackTrace(true);
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();
            SetThreadContextProperties(methodBase.Name, stackFrame.GetFileName(), stackFrame.GetFileLineNumber().ToString());
            target.Error(message, exception);
        }

        public void LogFatal(string message, object data = null)
        {
            StackTrace stackTrace = new StackTrace(true);
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();
            SetThreadContextProperties(methodBase.Name, stackFrame.GetFileName(), stackFrame.GetFileLineNumber().ToString());
            if (data != null)
            {
                string dataJson = JsonConvert.SerializeObject(data);
                ThreadContext.Properties["data"] = dataJson;
            }
            target.Fatal(message);
        }

        public void LogFatal(string message, Exception exception)
        {
            StackTrace stackTrace = new StackTrace(true);
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();
            SetThreadContextProperties(methodBase.Name, stackFrame.GetFileName(), stackFrame.GetFileLineNumber().ToString());
            target.Fatal(message, exception);
        }

        public void LogInfo(string message, object data = null)
        {
            StackTrace stackTrace = new StackTrace(true);
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();
            SetThreadContextProperties(methodBase.Name, stackFrame.GetFileName(), stackFrame.GetFileLineNumber().ToString());
            if (data != null)
            {
                string dataJson = JsonConvert.SerializeObject(data);
                ThreadContext.Properties["data"] = dataJson;
            }
            target.Info(message);
        }

        public void LogInfo(string message, Exception exception)
        {
            StackTrace stackTrace = new StackTrace(true);
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();
            SetThreadContextProperties(methodBase.Name, stackFrame.GetFileName(), stackFrame.GetFileLineNumber().ToString());
            target.Info(message, exception);
        }

        public void LogWarning(string message, object data = null)
        {
            StackTrace stackTrace = new StackTrace(true);
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();
            SetThreadContextProperties(methodBase.Name, stackFrame.GetFileName(), stackFrame.GetFileLineNumber().ToString());
            if (data != null)
            {
                string dataJson = JsonConvert.SerializeObject(data);
                ThreadContext.Properties["data"] = dataJson;
            }
            target.Warn(message);
        }

        public void LogWarning(string message, Exception exception)
        {
            StackTrace stackTrace = new StackTrace(true);
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();
            SetThreadContextProperties(methodBase.Name, stackFrame.GetFileName(), stackFrame.GetFileLineNumber().ToString());
            target.Warn(message, exception);
        }
    }
}
