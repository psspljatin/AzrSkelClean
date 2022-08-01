using AzrSkelClean.Common.JsonFormatter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AzrSkelClean.CompositeRoot
{
    public class Injector
    {
        public static void Define(IServiceCollection services, IConfiguration Configuration, bool fromAPIs = true)
        {
            if (fromAPIs)
            {
                //services.AddDbContext<ChexContext>();
                //services.AddTransient<MongoChexContext>();


                services.AddTransient<IDataFormatter, NewtonDataFormatter>();
            }
            else
            {
                services.AddTransient<IDataFormatter, NewtonDataFormatter>();
            }
        }
    }
}
