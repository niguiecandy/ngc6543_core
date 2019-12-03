using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*
	https://docs.unity3d.com/Manual/FrustumSizeAtDistance.html	
*/

namespace NGC6543
{
	public class CameraFrustrum
	{
		/// <summary>
		/// Returns the frustrum size at the given distance from the given Camera.
		/// Both Perspective and Orthographic cameras are supported.
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="distance">x : frustrum width / y : frustrum height</param>
		/// <returns></returns>
		public static Vector2 GetFrustrumSizeAt(Camera camera, float distance)
		{
			Vector2 result = Vector2.one;
			
			if (camera.orthographic)
			{
				// Orthographic Size equals frustrum height / 2
				result.y = camera.orthographicSize * 2.0f;
				result.x = result.y * camera.aspect;
			}
			else
			{
				result.y = 2f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
				result.x = result.y * camera.aspect;
			}
			return result;
		}
		
		
		/// <summary>
		/// Returns the distance for the given frustrum height.
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="frustrumHeight">The desired frustrum height.</param>
		/// <returns></returns>
		public static float GetDistanceForHeight(Camera camera, float frustrumHeight)
		{
			if (camera.orthographic)
			{
				Debug.LogWarning("The camera " + camera.name + " is orthographic!");
				return -1f;
			}
			return frustrumHeight * 0.5f / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
		}
		
		
		/// <summary>
		/// Returns the field of view for the given frustrum height at the given distance.
		/// </summary>
		/// <param name="distance">The desired distance from a camera.</param>
		/// <param name="frustrumHeight">The desired frustrum height.</param>
		/// <returns></returns>
		public static float GetFieldOfView(float distance, float frustrumHeight)
		{
			return 2f * Mathf.Atan(frustrumHeight * 0.5f / distance) * Mathf.Rad2Deg;
		}
	}
}