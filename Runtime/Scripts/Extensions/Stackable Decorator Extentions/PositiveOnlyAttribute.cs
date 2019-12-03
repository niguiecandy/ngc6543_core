using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackableDecorator;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PositiveOnlyAttribute : StackableFieldAttribute
{
	public PositiveOnlyAttribute()
	{
		
	}
	
	#if UNITY_EDITOR

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label, bool includeChildren)
	{
		return EditorGUIUtility.singleLineHeight;
	}
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
	{
		Debug.Log(property.propertyType);
		if (property.propertyType == SerializedPropertyType.Integer || property.propertyType == SerializedPropertyType.Float)
		{
			label = EditorGUI.BeginProperty(position, label, property);
			
			switch (property.propertyType)
			{
				case SerializedPropertyType.Integer :
					property.intValue = EditorGUI.IntField(position, property.intValue);
					if (property.intValue < 0f)
					{
						property.intValue = 0;
					}
					break;
				case SerializedPropertyType.Float :
					property.floatValue = EditorGUI.FloatField(position, property.floatValue);
					if (property.floatValue < 0f)
					{
						property.floatValue = 0f;
					}
					break;
			}
			
			EditorGUI.EndProperty();
		}
		else
		{
			EditorGUI.LabelField(position, label.text, "Use with Integer or Float.");
			return;
		}
	}

#endif // UNITY_EDITOR
}
