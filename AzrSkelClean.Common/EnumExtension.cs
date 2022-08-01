using System;
using System.Collections.Generic;
using System.Text;

namespace AzrSkelClean.Common
{
    public static class EnumExtension
    {
        public static int ToInteger<T>(this T value)
        {
            return Convert.ToInt32(value);
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static byte ToByte<T>(this T value)
        {
            return Convert.ToByte(value);
        }

        public static string ToStringValue<T>(this T value)
        {
            return ((T)value).ToString();
        }
    }
}
