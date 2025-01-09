using System;

namespace JollyRoger.Attributes
{
    internal class InstanceTypeAttribute : Attribute
    {
        public Type Type { get; private set; }

        public InstanceTypeAttribute(Type type)
        {
            Type = type;
        }
    }
}
