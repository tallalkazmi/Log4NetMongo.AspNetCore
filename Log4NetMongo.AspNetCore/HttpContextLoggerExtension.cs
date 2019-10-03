using Microsoft.AspNetCore.Http;

namespace Log4NetMongo.AspNetCore
{
    public static class HttpContextLoggerExtension
    {
        private static IHttpContextAccessor httpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextLoggerExtension.httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext Current
        {
            get
            {
                return httpContextAccessor?.HttpContext;
            }
        }
    }
}
