using AzrSkelClean.Common.JsonFormatter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzrSkelClean.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private string _postData = string.Empty;
        private IDataFormatter _dataFormatter;

        public BaseController(IDataFormatter dataFormatter)
        {
            _dataFormatter = dataFormatter;
        }
        public string RequestData
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_postData)) return _postData;

                if (!Request.HasFormContentType)
                {
                    using (var streamReader = new StreamReader(Request.Body, Encoding.UTF8))
                    {
                        _postData = streamReader.ReadToEndAsync().Result;
                    }
                }
                else
                {
                    var formData = GetFormData();
                    _postData = _dataFormatter.JsonSerialize(formData);
                }

                return _postData;
            }
        }
        private Dictionary<string, string> GetFormData()
        {
            return Request.Form.Keys.Cast<string>().ToDictionary(key => key, key => Convert.ToString(Request.Form[key]));
        }
        protected Dictionary<string, string> GetHeaderData()
        {
            return Request.Headers.Keys.Cast<string>().ToDictionary(key => key, key => Convert.ToString(Request.Headers[key]));
        }
    }
}
