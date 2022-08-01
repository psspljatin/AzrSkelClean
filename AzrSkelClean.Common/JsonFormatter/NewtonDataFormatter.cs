using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AzrSkelClean.Common.JsonFormatter
{
    public class NewtonDataFormatter : IDataFormatter
    {
        public T JsonDeSerialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public string JsonSerialize<T>(T entity)
        {
            return JsonConvert.SerializeObject(entity);
        }
    }
}
