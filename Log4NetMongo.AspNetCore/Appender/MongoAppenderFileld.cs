using log4net.Layout;

namespace Log4NetMongo.AspNetCore.Appender
{
    public class MongoAppenderFileld
    {
        public string Name { get; set; }
        public IRawLayout Layout { get; set; }
    }
}
