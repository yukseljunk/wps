using System;
using System.Configuration;
using System.Reflection;
using PttLib.Helpers;

namespace PttLib.TourInfo
{
    public class TourInfoConverter:ITourInfoConverter
    {
        private readonly string _fullName;
        private Assembly _assembly;
        private Type _type;
        private MethodInfo _method;


        /// <summary>
        /// convention: [ConvertAssembly,]ConvertType,ConvertMethod
        /// </summary>
        /// <param name="fullName"></param>
        public TourInfoConverter(string fullName)
        {
            _fullName = fullName;
            ResolveMethod(fullName);
        }

        private void ResolveMethod(string fullName)
        {

            var paths = fullName.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
            if (paths.Length < 2)
            {
                throw new ConfigurationErrorsException("convention: [ConvertAssembly,]ConvertType,ConvertMethod is not followed");
            }

            try
            {
                var typeNameIndex = 0;
                if (paths.Length == 2)
                {
                    _assembly = typeof (TourInfoConverter).Assembly;
                }
                else
                {
                    _assembly = Assembly.Load(paths[0].Trim());
                    typeNameIndex++;
                }
                _type = _assembly.GetType(paths[typeNameIndex].Trim());
                _method = _type.GetMethod(paths[typeNameIndex + 1].Trim());
            }
            catch(Exception exception)
            {
                Logger.LogExceptions(exception);
            }
        }
        
        public string MethodFullName { 
            get
            {
                return _fullName;
            } 
        }
        
        public object Convert(object input)
        {
            object obj = Activator.CreateInstance(_type);
            // Execute the method.
            return _method.Invoke(obj, new []{input});    
        }

        public ITourInfoConverter Clone()
        {
            var clone = new TourInfoConverter(_fullName);
            return clone;
        }
    }
}