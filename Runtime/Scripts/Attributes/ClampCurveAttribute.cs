using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ClampCurveAttribute : PropertyAttribute
{
	public float startTime, endTime;
	
	// public float minValue, maxValue;
	
	public float[] valueRange;
	
	public float[] firstValueRange, lastValueRange;
	
	bool isIntervalClamped = false;
	bool isValueClamped = false;
	bool areFirstLastValuesClamped = false;
	
	public bool IsIntervalClamped { get { return isIntervalClamped; } }
	
	public bool IsValueClamped { get { return isValueClamped; } }
	
	public bool AreFirstLastValuesClamped { get { return areFirstLastValuesClamped; } }
	
	/// <summary>
	/// Clamps the times of keyframes of an AnimationCurve.
	/// </summary>
	/// <param name="minIntervalTime"></param>
	/// <param name="maxIntervalTime"></param>
	public ClampCurveAttribute (float minIntervalTime, float maxIntervalTime)
	{
		ClampInterval(minIntervalTime, maxIntervalTime);
	}

	/// <summary>
	/// Clamps the values of the keyframes of an AnimationCurve.
	/// </summary>
	/// <param name="valueRange">Array length should be 2. Index 0 and 1 will be minimum and maximum values, respectively.</param>
	public ClampCurveAttribute(float[] valueRange)
	{
		ClampValues(valueRange);
	}
	
	/// <summary>
	/// Clamps the values of the first and the last keyframes of an AnimationCurve.
	/// </summary>
	/// <param name="firstValueRange">Array length should be 2. Index 0 and 1 will be minimum and maximum values, respectively.</param>
	/// <param name="lastValueRange"></param>
	public ClampCurveAttribute (float[] firstValueRange, float[] lastValueRange)
	{
		ClampFirstLastValues(firstValueRange, lastValueRange);
	}

	/// <summary>
	/// Clamps the times and values of keyframes inside an AnimationCurve.
	/// </summary>
	/// <param name="minIntervalTime"></param>
	/// <param name="maxIntervalTime"></param>
	/// <param name="valueRange">Array length should be 2. Index 0 and 1 will be minimum and maximum values, respectively.</param>
	public ClampCurveAttribute (float minIntervalTime, float maxIntervalTime, float[] valueRange)
	{
		ClampInterval(minIntervalTime, maxIntervalTime);
		ClampValues(valueRange);
	}
	
	
	void ClampInterval (float minIntervalTime, float maxIntervalTime)
	{
		isIntervalClamped = true;
		this.startTime = minIntervalTime;
		this.endTime = maxIntervalTime;
	}
	
	void ClampValues (float[] valueRange)
	{
		isValueClamped = true;
		this.valueRange = valueRange;
	}
	
	void ClampFirstLastValues(float[] firstValueRange, float[] lastValueRange)
	{
		areFirstLastValuesClamped = true;
		this.firstValueRange = firstValueRange;
		this.lastValueRange = lastValueRange;
	}
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(ClampCurveAttribute))]
public class ClampCurvePropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{	
		EditorGUI.BeginProperty(position, label, property);
		
		if (property.animationCurveValue == null)
		{
			EditorGUI.TextField(position, "!! AnimationCurve is required!");
			EditorGUI.EndProperty();
			return;
		}
		
		EditorGUI.BeginChangeCheck();
		EditorGUI.PropertyField(position, property, label, true);
		
		if (EditorGUI.EndChangeCheck())
		{
			var clamp = attribute as ClampCurveAttribute;
			var curve = property.animationCurveValue;
			var keyframes = curve.keys;
			
			if (clamp.IsIntervalClamped)
			{
				for (int i = 0; i < keyframes.Length; i++)
				{
					keyframes[i].time = Mathf.Clamp(keyframes[i].time, clamp.startTime, clamp.endTime);
				}
			}
			
			if (clamp.IsValueClamped)
			{
				for (int i = 0; i < keyframes.Length; i++)
				{
					keyframes[i].value = Mathf.Clamp(keyframes[i].value, clamp.valueRange[0], clamp.valueRange[1]);
				}
			}
			
			curve.keys = keyframes;
			property.animationCurveValue = curve;
			EditorUtility.SetDirty(property.serializedObject.targetObject);
		}
		
		EditorGUI.EndProperty();
	}
	
}

#endif