using AzrSkelClean.Common.JsonFormatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzrSkelClean.Common
{
    public class HttpClientService<TResult> : IDisposable where TResult : class
    {
        private readonly HttpClient client = new HttpClient();
        private readonly IDataFormatter formatter;

        public string BaseAddress { get; set; }

        public string AccessToken { get; set; }

        public HttpClientService(string url)
        {
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = new TimeSpan(0, 30, 0);
            formatter = new NewtonDataFormatter();
        }

        /// <summary>
        /// Common HTTP GET method to call API methods
        /// </summary>
        /// <param name="url">Url of an API</param>
        /// <param name="urlParameters">Url parameters. Null if not</param>
        /// <param name="headers">Headers parameters. Null if not</param>
        /// <returns>Deserialize object of api result</returns>
        public TResult GetAPI(string url, List<KeyValuePair<string, string>> urlParameters = null, List<KeyValuePair<string, string>> headers = null)
        {
            string parameters = string.Empty;

            if (urlParameters != null)
            {
                parameters = BuildURLParametersString(urlParameters);
                url = string.IsNullOrEmpty(parameters) ? url : url + parameters;
            }

            if (headers != null)
            {
                AddHeaders(headers);
            }

            var response = Task.Run(() => client.GetAsync(url)).Result;
            var apiResult = response.Content.ReadAsStringAsync().Result;
            var result = formatter.JsonDeSerialize<TResult>(apiResult);
            return result;
        }

        public string GetAPIResultString(string url, List<KeyValuePair<string, string>> urlParameters = null, List<KeyValuePair<string, string>> headers = null)
        {
            string parameters = string.Empty;

            if (urlParameters != null)
            {
                parameters = BuildURLParametersString(urlParameters);
                url = string.IsNullOrEmpty(parameters) ? url : url + parameters;
            }

            if (headers != null)
            {
                AddHeaders(headers);
            }

            var response = Task.Run(() => client.GetAsync(url)).Result;
            var apiResult = response.Content.ReadAsStringAsync().Result;
            return apiResult;
        }

        public TResult PostAPI(string url, List<KeyValuePair<string, string>> urlParameters = null, List<KeyValuePair<string, string>> formParameters = null, string jsonString = "", List<KeyValuePair<string, string>> headers = null)
        {
            HttpContent contentPost = null;
            string parameters = string.Empty;
            if (headers != null)
            {
                AddHeaders(headers);
            }
            if (urlParameters != null)
            {
                parameters = BuildURLParametersString(urlParameters);
                url = string.IsNullOrEmpty(parameters) ? url : url + parameters;
            }
            if (formParameters != null)
            {
                var formContent = new FormUrlEncodedContent(formParameters);
                contentPost = formContent;
            }
            else if (!string.IsNullOrEmpty(jsonString))
            {
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                contentPost = content;
            }
            var response = Task.Run(() => client.PostAsync(url, contentPost)).Result;

            IEnumerable<string> responseHeaders;
            response.Headers.TryGetValues("Authorization", out responseHeaders);

            if (responseHeaders != null && responseHeaders.Any())
            {
                AccessToken = responseHeaders.Select(x => x).FirstOrDefault();
            }

            var apiResult = response.Content.ReadAsStringAsync().Result;
            var result = formatter.JsonDeSerialize<TResult>(apiResult);
            return result;
        }

        //public TResult PostAPI(string url, List<KeyValuePair<string, string>> formParameters = null, string jsonString = "", List<KeyValuePair<string, string>> headers = null)
        //{
        //    HttpContent contentPost = null;
        //    if (headers != null)
        //    {
        //        AddHeaders(headers);
        //    }
        //    if (formParameters != null)
        //    {
        //        var formContent = new FormUrlEncodedContent(formParameters);
        //        contentPost = formContent;
        //    }
        //    else if (!string.IsNullOrEmpty(jsonString))
        //    {
        //        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        //        contentPost = content;
        //    }
        //    var response = Task.Run(() => client.PostAsync(url, contentPost)).Result;

        //    IEnumerable<string> responseHeaders;
        //    response.Headers.TryGetValues("Authorization", out responseHeaders);

        //    if (responseHeaders != null && responseHeaders.Any())
        //    {
        //        AccessToken = responseHeaders.Select(x => x).FirstOrDefault();
        //    }

        //    var apiResult = response.Content.ReadAsStringAsync().Result;
        //    var result = formatter.JsonDeSerialize<TResult>(apiResult);
        //    return result;
        //}

        public TResult PutAPI(string url, List<KeyValuePair<string, string>> urlParameters = null, List<KeyValuePair<string, string>> formParameters = null, string jsonString = "", byte[] binaryData = null, List<KeyValuePair<string, Stream>> multiFormParameters = null, List<KeyValuePair<string, string>> headers = null)
        {
            HttpContent contentPost = null;
            string parameters = string.Empty;
            if (headers != null)
            {
                AddHeaders(headers);
            }
            if (urlParameters != null)
            {
                parameters = BuildURLParametersString(urlParameters);
                url = string.IsNullOrEmpty(parameters) ? url : url + parameters;
            }
            if (formParameters != null)
            {
                var formContent = new FormUrlEncodedContent(formParameters);
                contentPost = formContent;
            }
            else if (!string.IsNullOrEmpty(jsonString))
            {
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                contentPost = content;
            }

            if (binaryData != null)
            {
                var binaryContent = new ByteArrayContent(binaryData);
                contentPost = binaryContent;
            }

            if (multiFormParameters != null)
            {
                var multiFormContent = new MultipartFormDataContent();
                foreach (var multiFormParameter in multiFormParameters)
                {
                    multiFormParameter.Value.Position = 0;
                    multiFormContent.Add(new StreamContent(multiFormParameter.Value), multiFormParameter.Key, multiFormParameter.Key);
                }
                contentPost = multiFormContent;
            }

            var response = Task.Run(() => client.PutAsync(url, contentPost)).Result;

            IEnumerable<string> responseHeaders;
            response.Headers.TryGetValues("Authorization", out responseHeaders);

            if (responseHeaders != null && responseHeaders.Any())
            {
                AccessToken = responseHeaders.Select(x => x).FirstOrDefault();
            }

            var apiResult = response.Content.ReadAsStringAsync().Result;
            var result = formatter.JsonDeSerialize<TResult>(apiResult);
            return result;
        }

        public TResult DeleteAPI(string url, List<KeyValuePair<string, string>> urlParameters = null, List<KeyValuePair<string, string>> headers = null)
        {
            string parameters = string.Empty;
            if (urlParameters != null)
            {
                parameters = BuildURLParametersString(urlParameters);
                url = string.IsNullOrEmpty(parameters) ? url : url + parameters;
            }
            if (headers != null)
            {
                AddHeaders(headers);
            }
            var response = Task.Run(() => client.DeleteAsync(url)).Result;
            var apiResult = response.Content.ReadAsStringAsync().Result;
            var result = formatter.JsonDeSerialize<TResult>(apiResult);
            return result;
        }

        private String BuildURLParametersString(List<KeyValuePair<string, string>> parameters)
        {
            UriBuilder uriBuilder = new UriBuilder();
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var urlParameter in parameters)
            {
                query[urlParameter.Key] = urlParameter.Value;
            }
            uriBuilder.Query = query.ToString();
            return uriBuilder.Query;
        }

        private void AddHeaders(List<KeyValuePair<string, string>> headers)
        {
            foreach (var header in headers)
            {
                if (!string.IsNullOrEmpty(header.Value))
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
