using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RegistrumPersonae.Attributes;

[CustomPropertyDrawer(typeof(TypeSelectorAttribute))]
public class TypeSelectorDrawer : PropertyDrawer
{
	private bool isDropdownOpen = false;
	private string searchString = string.Empty;
	private List<string> filteredTypeNames = new List<string>();
	private Vector2 scrollPosition = Vector2.zero;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		TypeSelectorAttribute typeSelector = (TypeSelectorAttribute)attribute;

		// Get all types that inherit from the specified base type
		List<Type> types = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(assembly => assembly.GetTypes())
			.Where(type => typeSelector.BaseType.IsAssignableFrom(type) && !type.IsAbstract)
			.OrderBy(type => type.Name)
			.ToList();

		// Create a list of type names
		List<string> typeNames = types.Select(t => t.FullName).ToList();
		typeNames.Insert(0, "None"); // Add "None" option

		// Current value
		string currentTypeName = property.stringValue;
		int currentIndex = typeNames.IndexOf(currentTypeName);
		if (currentIndex == -1) currentIndex = 0;

		// Main dropdown button
		if (EditorGUI.DropdownButton(position, new GUIContent(currentTypeName ?? "None"), FocusType.Keyboard))
		{
			isDropdownOpen = !isDropdownOpen;
			searchString = string.Empty; // Reset search
			filteredTypeNames = new List<string>(typeNames); // Reset filter
		}

		// Dropdown logic
		if (isDropdownOpen)
		{
			// Position for dropdown window
			Rect dropdownRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, 300);
			GUI.Box(dropdownRect, GUIContent.none);

			GUILayout.BeginArea(dropdownRect);
			EditorGUILayout.BeginVertical();

			// Search box
			EditorGUI.BeginChangeCheck();
			searchString = EditorGUILayout.TextField("Search", searchString);
			if (EditorGUI.EndChangeCheck())
			{
				// Filter the type names based on search
				filteredTypeNames = typeNames
					.Where(name => string.IsNullOrEmpty(searchString) || name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
					.ToList();
			}

			// Scrollable dropdown
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			foreach (var typeName in filteredTypeNames)
			{
				if (GUILayout.Button(typeName, EditorStyles.popup))
				{
					// Set selected type
					property.stringValue = typeName == "None" ? string.Empty : typeName;
					isDropdownOpen = false; // Close dropdown
				}
			}
			EditorGUILayout.EndScrollView();

			EditorGUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return isDropdownOpen ? EditorGUIUtility.singleLineHeight * 2 : EditorGUIUtility.singleLineHeight;
	}
}