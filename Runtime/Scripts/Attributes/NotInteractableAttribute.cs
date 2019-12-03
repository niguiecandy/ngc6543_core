using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NotInteractableAttribute : PropertyAttribute {}

public class NotInteractableOnPlayAttribute : PropertyAttribute {}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(NotInteractableAttribute))]
public class NotInteractablePropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		GUI.enabled = false;
		EditorGUI.BeginChangeCheck();
		EditorGUI.PropertyField(position, property, label, true);
		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(property.serializedObject.targetObject);
		}
		GUI.enabled = true;
		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, true);
	}
}

[CustomPropertyDrawer(typeof(NotInteractableOnPlayAttribute))]
public class NotInteractableOnPlayPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		if (Application.isPlaying)
		{
			GUI.enabled = false;
		}
		EditorGUI.BeginChangeCheck();
		EditorGUI.PropertyField(position, property, label, true);
		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(property.serializedObject.targetObject);
		}
		GUI.enabled = true;
		EditorGUI.EndProperty();
	}
	
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, true);
	}
}

#endif