
/* MinMaxRangeAttribute.cs
* by Eddie Cameron – For the public domain
* —————————-
* Use a MinMaxRange class to replace twin float range values (eg: float minSpeed, maxSpeed; becomes MinMaxRange speed)
* Apply a [MinMaxRange( minLimit, maxLimit )] attribute to a MinMaxRange instance to control the limits and to show a
* slider in the inspector
*/

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MinMaxRangeAttribute : PropertyAttribute
{
	public float minLimit, maxLimit;

	public MinMaxRangeAttribute( float minLimit, float maxLimit )
	{
		this.minLimit = minLimit;
		this.maxLimit = maxLimit;
	}
}

[System.Serializable]
public class MinMaxRange
{
	public float min , max;
	public float Width{get{return (max - min);}}

	public float GetRandom()
	{
		return Random.Range( min, max );
	}

	/// <summary>
	/// Get the value from the normalized input value
	/// </summary>
	/// <param name="value01">Value between 0 and 1.</param>
	public float Get01(float value01){
		if(value01 <= 0f) return min;
		if(value01 >= 1f) return max;
		return min + Width * value01;
	}
	/// <summary>
	/// Get the inverted value from the normalized input value.
	/// </summary>
	/// <param name="value01">Value between 0 and 1.</param>
	public float GetInverted(float value01){
		if(value01 <= 0f) return max;
		if(value01 >= 1f) return min;
		return max - Width * value01;
	}

	/// <summary>
	/// Returns a clamped value from the given input
	/// </summary>
	/// <param name="value">Value.</param>
	public float Clamp(float value){
		return Mathf.Min(Mathf.Max(min, value), max);
	}

	/// <summary>
	/// Returns a clamped and normalized(0~1) value from the given input
	/// </summary>
	/// <param name="value">Value.</param>
	public float Clamp01(float value){
		if(value < min) return 0f;
		if(value > max) return 1f;
		return (value - min)/(max - min);
	}

	/// <summary>
	/// Returns a clamped and inverted normalized(0~1) value from the given input
	/// </summary>
	/// <returns>The inverted01.</returns>
	/// <param name="value">Value.</param>
	public float ClampInverted01(float value){
		if(value < min) return 1f;
		if(value > max) return 0f;
		return (max - value)/(max - min);
	}

	/// <summary>
	/// Check if the input value is in range.
	/// </summary>
	/// <returns><c>true</c>, the value is in range, <c>false</c> otherwise.</returns>
	/// <param name="value">Value.</param>
	public bool IsInRange(float value){
		if(value < min || value > max) return false;
		return true;
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer( typeof( MinMaxRangeAttribute))]
public class MinMaxRangeDrawer : PropertyDrawer
{
	public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
	{
		return base.GetPropertyHeight( property, label ) + 16;
	}

	// Draw the property inside the given rect
	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
	{
		// Now draw the property as a Slider or an IntSlider based on whether it’s a float or integer.
		if ( property.type != "MinMaxRange" )
			Debug.LogWarning( "Use only with MinMaxRange type" );
		else
		{
			var range = attribute as MinMaxRangeAttribute;
			var minValue = property.FindPropertyRelative( "min" );
			var maxValue = property.FindPropertyRelative( "max" );
			var newMin = minValue.floatValue;
			var newMax = maxValue.floatValue;
			var xDivision = position.width * 0.33f;
			var yDivision = position.height * 0.5f;
			if(property.hasMultipleDifferentValues) EditorGUI.showMixedValue = true;
			EditorGUI.BeginChangeCheck();
			EditorGUI.LabelField( new Rect( position.x, position.y, xDivision, yDivision )
			                     , label );

			EditorGUI.LabelField( new Rect( position.x, position.y + yDivision, position.width, yDivision )
			                     , range.minLimit.ToString( "0.##" ) );
			EditorGUI.LabelField( new Rect( position.x + position.width - 28f, position.y + yDivision, position.width, yDivision )
			                     , range.maxLimit.ToString( "0.##" ) );
			EditorGUI.MinMaxSlider( new Rect( position.x + 24f, position.y + yDivision, position.width - 48f, yDivision )
			                       , ref newMin, ref newMax, range.minLimit, range.maxLimit );

			EditorGUI.LabelField( new Rect( position.x + xDivision, position.y, xDivision, yDivision )
			                     , "From: " );
			newMin = Mathf.Clamp( EditorGUI.FloatField( new Rect( position.x + xDivision + 30, position.y, xDivision - 30, yDivision )
			                                           , newMin )
			                     , range.minLimit, newMax );
			EditorGUI.LabelField( new Rect( position.x + xDivision * 2f, position.y, xDivision, yDivision )
			                     , "To: " );
			newMax = Mathf.Clamp( EditorGUI.FloatField( new Rect( position.x + xDivision * 2f + 24, position.y, xDivision - 24, yDivision )
			                                           , newMax )
			                     , newMin, range.maxLimit );

			if(EditorGUI.EndChangeCheck()){
				minValue.floatValue = newMin;
				maxValue.floatValue = newMax;
			}
		}
	}
}
#endif