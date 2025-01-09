using System;
using UnityEngine;

namespace JollyRoger.Attributes
{ 
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class TypeSelectorAttribute : PropertyAttribute
	{
		public Type BaseType;

		public TypeSelectorAttribute(Type baseType)
		{
			BaseType = baseType;
		}
	}
}