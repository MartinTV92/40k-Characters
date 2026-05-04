/*
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using SunJack.UI.Utilities;
using UnityEngine.EventSystems;

[CustomPropertyDrawer(typeof(PropegateMethod))]
public class PropegateMethodDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Get the field for targetTypeName
		SerializedProperty targetTypeProp = property.FindPropertyRelative("targetTypeName");

		// Ensure we are drawing the label
		EditorGUI.PropertyField(position, targetTypeProp, label);

		// Create a list of component types to choose from
		Type[] types = typeof(UIBehaviour).Assembly.GetTypes()
			.Where(t => typeof(UIBehaviour).IsAssignableFrom(t) && !t.IsAbstract)
			.ToArray();

		// Create the dropdown for selecting the component type
		int index = Array.IndexOf(types, Type.GetType(targetTypeProp.stringValue));
		string[] typeNames = types.Select(t => t.Name).ToArray();

		// Add the dropdown to the property field
		index = EditorGUI.Popup(new Rect(position.x, position.y + 20, position.width, position.height), "Select Target Component", index, typeNames);

		// Update the targetTypeName based on the selected type
		if (index >= 0)
		{
			targetTypeProp.stringValue = types[index].AssemblyQualifiedName;
		}
	}
}
// */

