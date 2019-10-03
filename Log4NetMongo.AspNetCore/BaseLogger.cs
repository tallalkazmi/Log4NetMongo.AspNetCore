using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Log4NetMongo.AspNetCore
{
    public class BaseLogger
    {
        private string Username;
        private string TenantId;
        protected string Application;
        protected string Environment;
        protected string Collection;
        protected string ConnectionString;
        protected LogLevel? LogLevel;

        public BaseLogger()
        {
            GlobalContext.Properties.Clear();
            ThreadContext.Properties.Clear();
            SetInstanceVariables(ClaimsPrincipal.Current);
        }

        protected ILog GetConfiguredLog()
        {
            #region ConfigXML
            string xml = $@"
<log4net>
	<appender name='MongoDBAppender' type='Log4NetMongo.AspNetCore.Appender.MongoDBAppender, Log4NetMongo.AspNetCore'>
		<connectionString value='{ConnectionString}' />
        <collectionName value='{Collection}' />
        <expireAfterSeconds value='2592000' />
        <field>
			<name value='timestamp' />
			<layout type='log4net.Layout.RawTimeStampLayout' />
		</field>
		<field>
			<name value='level' />
			<layout type='log4net.Layout.PatternLayout' value='%level' />
		</field>
		<field>
			<name value='thread' />
			<layout type='log4net.Layout.PatternLayout' value='%thread' />
		</field>
		<field>
			<name value='application' />
			<layout type='log4net.Layout.RawPropertyLayout'>
				<key value='application' />
			</layout>
		</field>
        <field>
			<name value='environment' />
			<layout type='log4net.Layout.RawPropertyLayout'>
				<key value='environment' />
			</layout>
		</field>
        <field>
			<name value='user' />
			<layout type='log4net.Layout.RawPropertyLayout'>
				<key value='user' />
			</layout>
		</field>
        <field>
			<name value='tenant' />
			<layout type='log4net.Layout.RawPropertyLayout'>
				<key value='tenant' />
			</layout>
		</field>
		<field>
			<name value='message' />
			<layout type='log4net.Layout.PatternLayout' value='%message' />
		</field>
		<field>
			<name value='data' />
			<layout type='log4net.Layout.RawPropertyLayout'>
				<key value='data' />
			</layout>
		</field>
		<field>
			<name value='exception' />
			<layout type='log4net.Layout.ExceptionLayout' />
		</field>
		<field>
			<name value='methodName' />
			<layout type='log4net.Layout.RawPropertyLayout'>
				<key value='methodName' />
			</layout>
		</field>
		<field>
			<name value='fileName' />
			<layout type='log4net.Layout.RawPropertyLayout'>
				<key value='fileName' />
			</layout>
		</field>
        <field>
			<name value='lineNumber' />
			<layout type='log4net.Layout.RawPropertyLayout'>
				<key value='lineNumber' />
			</layout>
		</field>
	</appender>
	<root>
		<level value='{(LogLevel.HasValue ? LogLevel.ToString().ToUpper() : "ALL")}' />
		<appender-ref ref='MongoDBAppender' />
	</root>
</log4net>
";
            #endregion

            var loggerRepository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(xml)))
            {
                XmlConfigurator.Configure(loggerRepository, stream);
            }

            return LogManager.GetLogger(typeof(BaseLogger));
        }

        private void SetInstanceVariables(ClaimsPrincipal principal)
        {
            if (principal == null) return;
            Username = ClaimsPrincipal.Current.Identity.Name;
            Claim tenantClaim = ClaimsPrincipal.Current.FindFirst("tenant");
            TenantId = tenantClaim?.Value;
        }

        protected void SetThreadContextProperties(string methodName, string fileName, string lineNumber)
        {
            ThreadContext.Properties["application"] = Application;
            ThreadContext.Properties["environment"] = Environment;
            ThreadContext.Properties["methodName"] = methodName;
            ThreadContext.Properties["fileName"] = fileName;
            ThreadContext.Properties["lineNumber"] = lineNumber;
            ThreadContext.Properties["user"] = Username;
            ThreadContext.Properties["tenant"] = TenantId;
        }
    }
}
