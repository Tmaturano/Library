using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Library.API.Helpers
{
    public static class ObjectExtensions
    {
        public static ExpandoObject ShapeDataObject<T>(this T source, string fields)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var dataShapedObject = new ExpandoObject();

            //if fields wasn't specified, return all the fields of the given object
            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(T)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var propertyInfo in propertyInfos)
                {
                    var propertyValue = propertyInfo.GetValue(source);

                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }

                return dataShapedObject;
            }

            var fieldsAfterSplit = fields.Split(",");
            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();

                var propertyInfo = typeof(T)
                    .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                    throw new Exception($"Property {propertyName} wasn't found on {typeof(T)}");

                var propertyValue = propertyInfo.GetValue(source);

                ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
            }

            return dataShapedObject;
        }
    }
}
