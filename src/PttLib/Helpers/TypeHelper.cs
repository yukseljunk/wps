using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace PttLib.Helpers
{
    public class TypeHelper
    {


        public bool IsBasicType(Type type)
        {
            var basicTypesNotPrimitive = new List<Type>() { typeof(String), typeof(DateTime) };
            return type.IsPrimitive || basicTypesNotPrimitive.Contains(type);
        }
        public bool IsBasicType(object obj)
        {
            return IsBasicType(obj.GetType());
        }

        public bool IsArrayType(object obj)
        {
            return IsArrayType(obj.GetType());
        }
        public bool IsArrayType(Type type)
        {
            return !type.IsArray && type.GetGenericArguments().Count() <= 0;
        }

        public int StringToInt(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return 0;
            }

            int outVal;
            Int32.TryParse(input, out outVal);
            return outVal;
        }
        public T StringToEnum<T>(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return default(T);
            }

            if (Enum.IsDefined(typeof(T), input))
            {
                var sourceType = (T)Enum.Parse(typeof(T), input, true);
                return sourceType;
            }
            return default(T);
        }
        public bool StringToBool(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return false;
            }

            bool outVal;
            Boolean.TryParse(input, out outVal);
            return outVal;
        }


        public static bool IsDate(Object obj, CultureInfo cultureInfo = null)
        {
            if (cultureInfo == null) cultureInfo = CultureInfo.GetCultureInfo("en-US");
            string strDate = obj.ToString();
            try
            {
                DateTime dt = DateTime.Parse(strDate, cultureInfo);
                if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// makes dynamical replacement of properties of an object
        /// </summary>
        /// <param name="input">"sometext {class1.prop1} and {class1.prop2}"</param>
        /// <param name="instance">instance of T</param>
        /// <returns>sometext prop1 value and prop2 value when T=class1</returns>
        /// don't use pttrequest.wrappedrequest values as it is disposed or lazy initialized
        public static string FillDynamicProperties<T>(string input, T instance)
        {
            var result = input;
            var typeName = typeof(T).Name;
            foreach (Match match in Regex.Matches(result, @"\{" + typeName + @"\.([^}]*)\}", RegexOptions.IgnoreCase))
            {
                if (match.Groups.Count <= 1) continue;

                var compositeKey = "";
                var property = match.Groups[1].Value;
                if (property.Contains("."))//composite, take the first part
                {
                    var splitted = property.Split(new string[] {"."}, StringSplitOptions.RemoveEmptyEntries);
                    property = splitted[0];
                    compositeKey = splitted[1];
                }
                var propMatching = typeof(T).GetProperties().FirstOrDefault(p => p.Name == property);
                if (propMatching == null) continue;

                var propValue = propMatching.GetValue(instance, null);
                if (compositeKey == "")
                {
                    result = result.Replace("{" + typeName + "." + property + "}", propValue.ToString());
                }
                else
                {
                    var propValueDict = propValue as ConcurrentDictionary<string, object>;
                    if (propValueDict == null) continue;

                    if (!propValueDict.ContainsKey(compositeKey)) continue;

                    result = result.Replace("{" + typeName + "." + property + "." + compositeKey + "}",
                        propValueDict[compositeKey].ToString());
                
                }
            }

            return result;
        }


    }
}
