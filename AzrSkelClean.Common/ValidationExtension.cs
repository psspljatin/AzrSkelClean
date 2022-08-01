using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace AzrSkelClean.Common
{
    public static class ValidationExtension
    {
        public static string IsValid(this object obj, out Dictionary<string, string> resultsOutput)
        {
            string errMsgsResponse = null;
            resultsOutput = null;
            string errMsgList = null;
            var instance = obj.GetType().Assembly.CreateInstance((obj.GetType().FullName + "Validator"));
            var validator = (IValidator)instance;
            if (validator != null)
            {
                var valContext = new ValidationContext<object>(obj);
                var results = validator.Validate(valContext);
                if (!results.IsValid)
                {
                    resultsOutput = new Dictionary<string, string>();
                    foreach (var item in results.Errors)
                    {
                        var i = 0;
                        var popName = item.PropertyName;
                        while (resultsOutput.Keys.Contains(popName))
                        {
                            popName = popName + i;
                            i++;
                        }
                        resultsOutput.Add(popName, item.ErrorMessage);
                        errMsgList += item.ErrorMessage + ";";
                    }
                }
            }
            if (errMsgList != null)
            {
                errMsgList = errMsgList.TrimEnd(';');
                errMsgsResponse = errMsgList;
            }
            return errMsgsResponse;
        }

        public static Exception ThrowValidationMessage(this string message)
        {
            var ex = new Exception(message);
            ex.Data.Add("status_code", HttpStatusCode.BadRequest);
            return ex;
        }

        public static Exception ThrowException(this HttpStatusCode code, string defaultMsg = "Server Exception while executing a request.")
        {
            var ex = new Exception(defaultMsg);
            ex.Data.Add("status_code", code);
            return ex;
        }

        public static string[] BuildBucketData(string path)
        {
            var str = new string[2];
            var splitfllePath = path.Split(new[] { ":\\" }, StringSplitOptions.RemoveEmptyEntries);
            string bucketName;
            var keyName = string.Empty;
            if (splitfllePath.Length == 1)
            {
                bucketName = splitfllePath[0];

            }
            else
            {
                bucketName = splitfllePath[0];
                keyName = path.Replace(bucketName + ":\\", "").TrimEnd(new[] { '\\' });
                keyName = keyName.Replace("\\", "/");
            }
            str[0] = bucketName.ToLowerInvariant();
            str[1] = keyName.ToLowerInvariant();
            return str.Where(s => s.Trim().Length > 0).ToArray();
        }
    }
}
