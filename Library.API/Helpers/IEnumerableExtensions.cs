using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Library.API.Helpers
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<T>(this IEnumerable<T> source, string fields)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var expandoObjectList = new List<ExpandoObject>();

            /*
             Using Reflection. It's expensive, so rather than doing it for each object in the list, we do
             it once and reuse the results. After all, part of the reflection is on the type of the object (T), not on the instance.
             
             */
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                //all the public properties should be in the ExpandoObject
                var propertyInfos = typeof(T)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                // only the public properties that match the fields should be in the ExpandoObject    

                //the field are separated by ",", so we split it
                var fieldsAfterSplit = fields.Split(",");
                foreach (var field in fieldsAfterSplit)
                {
                    //each field might contain leading or trailing spaces, so we trim it.
                    var propertyName = field.Trim();


                    var propertyInfo = typeof(T)
                        .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                        throw new Exception($"Property {propertyName} wasn't found on {typeof(T)}");

                    propertyInfoList.Add(propertyInfo);
                }
            }


            foreach (T sourceObject in source)
            {
                //this object will hold the selected properties and values
                var dataShapedObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }

                expandoObjectList.Add(dataShapedObject);
            }

            return expandoObjectList;
        }
    }
}
