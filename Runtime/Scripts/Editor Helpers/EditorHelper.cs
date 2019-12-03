using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace NGC6543.EditorUtils
{
	#if UNITY_EDITOR
	
	/// <summary>
	/// Makes it easy to draw Transform handles on a Scene view.
	/// </summary>
	public class TransformHandlesDrawer
	{
		/*
			Example
			
			EditorGUI.BeginChangeCheck();
			pos = TransformHandleDrawers.DrawPositionHandle((Vector3)pos, (Quaternion)rot);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(obj, "Translate");
				SetValue(obj, "position", pos);
			}
		*/

		/// <summary>
		/// [EDITOR_ONLY]
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <param name="localRotation">If Tool Handle Rotation is set to Global, the local Rotation is ignored.</param>
		/// <returns></returns>
		public static Vector3 DrawPositionHandle(Vector3 worldPosition, Quaternion localRotation)
		{
			switch (Tools.pivotRotation)
			{
				case PivotRotation.Global:
					return Handles.PositionHandle(worldPosition, Quaternion.identity);
				case PivotRotation.Local:
					return Handles.PositionHandle(worldPosition, localRotation);
			}
			return Vector3.zero;
		}


		/// <summary>
		/// [EDITOR_ONLY]
		/// </summary>
		/// <param name="localRotation">If Tool Handle Rotation is set to Global, the local Rotation is ignored.</param>
		/// <param name="worldPosition"></param>
		/// <returns></returns>
		public static Quaternion DrawRotationHandle(Quaternion localRotation, Vector3 worldPosition)
		{
			switch (Tools.pivotRotation)
			{
				case PivotRotation.Global:
					return Handles.RotationHandle(Quaternion.identity, worldPosition);
				case PivotRotation.Local:
					return Handles.RotationHandle(localRotation, worldPosition);
			}
			return Handles.RotationHandle(localRotation, worldPosition);
		}

		/// <summary>
		/// [EDITOR_ONLY]
		/// </summary>
		/// <param name="localScale">If Tool Handle Rotation is set to Global, the local Rotation is ignored.</param>
		/// <param name="worldPosition"></param>
		/// <param name="localRotation"></param>
		/// <returns></returns>
		public static Vector3 DrawScaleHandle(Vector3 localScale, Vector3 worldPosition, Quaternion localRotation)
		{
			return Handles.ScaleHandle(localScale, worldPosition, localRotation, HandleUtility.GetHandleSize(worldPosition));
		}
	}
	
	
	/// <summary>
	/// Gets or sets a value from an object's field or property using reflection
	/// </summary>
	public class ReflectedValue
	{
		/*
			Example
		
			var pos = ReflectedValue.GetValue(_attachPointTransforms[i].objectReferenceValue, "position");
		*/


		/// <summary>
		/// [EDITOR_ONLY] Get a value from a source object using Reflection.
		/// </summary>
		/// <param name="source">SerializedProperty.objectReferenceValue</param>
		/// <param name="name">Field or Property name. Public/NonPublic/Instance/IgnoreCase</param>
		/// <returns>Null if source is null, or if no field or property was found.</returns>
		public static object GetValue(object source, string name)
		{
			if (source == null)
			{
				Debug.Log("GetValue failed : The source is null");
				return null;
			}

			var sourceType = source.GetType();
			// Debug.Log("Type : " + sourceType.Name);

			// Check if the name can be found as a field
			var field = sourceType.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
			if (field != null)
			{
				// Debug.Log("field has value");
				return field.GetValue(source);
			}
			else
			{
				// Check if the name can be found as a property
				var property = sourceType.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
				if (property != null)
				{
					// Debug.Log("property has value");
					return property.GetValue(source, null);
				}
				Debug.Log("no field or property of given name " + name);
				return null;
			}
		}

		/// <summary>
		/// [EDITOR_ONLY] Sets a value to a target object using Reflection.
		/// </summary>
		/// <param name="target">SerializedProperty.objectReferenceValue</param>
		/// <param name="name">Field or Property name.abstract Public/NonPublic/Instance/IgnoreCase</param>
		/// <param name="value"></param>
		/// <returns>False if target is null or if the target doesn't have field or property of given name. True otherwise.</returns>
		public static bool SetValue(object target, string name, object value)
		{
			if (target == null)
			{
				Debug.LogError("SetValue failed : The target is null");
				return false;
			}
			var targetType = target.GetType();

			var field = targetType.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
			if (field != null)
			{
				field.SetValue(target, value);
				return true;
			}
			else
			{
				var property = targetType.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
				if (property != null)
				{
					if (property.CanWrite)
					{
						property.SetValue(target, value, null);
						return true;
					}
					else
					{
						Debug.LogError(target + " : property " + name + " is read only!");
						return false;
					}
				}
				Debug.LogWarning("No field or property of given name " + name);
				return false;
			}
		}
	}
	
	
	public class ReflectedMethod
	{
		public static object ExecuteMethod(object target, string methodName)
		{
			return ExecuteMethod(target, methodName, null);
		}
		
		public static object ExecuteMethod(object target, string methodName, params object[] parameters)
		{
			if (target == null)
			{
				Debug.LogError("ExecuteMethod failed : The target is null");
				return null;
			}

			var targetType = target.GetType();
			var method = targetType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (method == null)
			{
				Debug.LogError("No method of given name " + methodName + " was found.");
				return null;
			}
			else
			{
				return method.Invoke(target, parameters);
			}
		}
	}
	#endif
}