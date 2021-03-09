using System;

namespace EasyNow.Utility.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyAttribute : Attribute
    {
        public PropertyAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}