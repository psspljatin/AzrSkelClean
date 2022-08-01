using System;
using System.Collections.Generic;
using System.Text;

namespace AzrSkelClean.Common.JsonFormatter
{
    public interface IDataFormatter
    {
        string JsonSerialize<T>(T entity);
        T JsonDeSerialize<T>(string data);
    }
}
