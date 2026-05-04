using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ComponentTypeAttribute))]
public class ComponentTypeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Ensure the property is of type Object
		if (property.propertyType == SerializedPropertyType.ObjectReference)
		{
			Component selectedComponent = (Component)property.objectReferenceValue;

			// Get all components attached to the GameObject
			GameObject gameObject = (property.serializedObject.targetObject as MonoBehaviour)?.gameObject;
			if (gameObject != null)
			{
				// Create a list of component types in the game object
				Component[] components = gameObject.GetComponents<Component>();
				string[] componentNames = new string[components.Length];
				for (int i = 0; i < components.Length; i++)
				{
					componentNames[i] = components[i].GetType().Name;
				}

				// Get the index of the selected component in the dropdown
				int selectedIndex = selectedComponent != null ? System.Array.IndexOf(components, selectedComponent) : -1;

				// Create the dropdown for selecting a component
				selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, componentNames);

				// If a component is selected, set it to the property
				if (selectedIndex >= 0 && selectedIndex < components.Length)
				{
					property.objectReferenceValue = components[selectedIndex];
				}
			}
		}
		else
		{
			EditorGUI.LabelField(position, label.text, "Use ComponentTypeAttribute with a Component field.");
		}
	}
}

