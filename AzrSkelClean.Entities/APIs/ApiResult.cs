using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzrSkelClean.Entities.APIs
{
    public class ApiResult<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("success")]
        public string Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("more_info")]
        public string MoreInfo { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
