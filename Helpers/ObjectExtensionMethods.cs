using System.Dynamic;

namespace PitchLogAPI.Helpers
{
    public static class ObjectExtensionMethods
    {
        public static dynamic Split<T>(this T obj, params KeyValuePair<string, object>[] newProperties)
        {
            var propertyInfos = typeof(T).GetProperties();
            dynamic newObject = new ExpandoObject();

            foreach(var property in propertyInfos)
            {
                ((IDictionary<string, object>)newObject)[property.Name] = property.GetValue(obj);
            }

            foreach(var kvPair in newProperties)
            {
                ((IDictionary<string, object>)newObject)[kvPair.Key] = kvPair.Value;
            }

            return newObject;
        }
    }
}
