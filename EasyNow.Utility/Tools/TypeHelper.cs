using EasyNow.Utility.Extensions;

namespace EasyNow.Utility.Tools
{
    public static class TypeHelper
    {
        public static object GetStaticProperty(string typeName, string property)
        {
            var type = typeName.GetTypeByName();
            if (type == null)
            {
                return null;
            }

            return type.GetStaticProperty(property);
        }
    }
}