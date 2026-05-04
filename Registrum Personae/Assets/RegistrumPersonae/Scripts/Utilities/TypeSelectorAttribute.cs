using System;
using UnityEngine;

namespace RegistrumPersonae.Attributes
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

