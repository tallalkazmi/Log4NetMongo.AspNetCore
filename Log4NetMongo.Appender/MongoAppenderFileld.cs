using log4net.Layout;

namespace Log4NetMongo.Appender
{
    public class MongoAppenderFileld
    {
        public string Name { get; set; }
        public IRawLayout Layout { get; set; }
    }
}
