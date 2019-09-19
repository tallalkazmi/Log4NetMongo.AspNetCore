using log4net.Layout;

namespace Log4Net.Mongo
{
    public class MongoAppenderFileld
    {
        public string Name { get; set; }
        public IRawLayout Layout { get; set; }
    }
}
