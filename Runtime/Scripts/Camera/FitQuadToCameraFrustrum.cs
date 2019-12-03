using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NGC6543
{
	
	public class FitQuadToCameraFrustrum : MonoBehaviour
	{
		
		public enum DistanceMode
		{
			/// <summary>
			/// Distance from the camera position to camera forward direction.
			/// </summary>
			/// <returns></returns>
			FromCameraPosition,
			
			/// <summary>
			/// Distance from the camera near clip plane to camera forward direction.
			/// </summary>
			/// <returns></returns>
			FromNearClipPlane,
			
			/// <summary>
			/// Distance from the camera far clip plane to camera forward direction.
			/// </summary>
			/// <returns></returns>
			FromFarClipPlane
		}
		
		[SerializeField] Camera _camera;
		
		[Header("Distance From Camera")]
		
		[SerializeField] DistanceMode _distanceMode = DistanceMode.FromCameraPosition;
		
		[Tooltip("The camera forward is the distance direction.")]
		[SerializeField] float _distance = 5f;
		
		[Header("Frustrum")]
		
		[Tooltip("The result Quad size will be multiplied by this value. Use this for 'offset' quad.")]
		[SerializeField] float _scaleFactor = 1f;
		
		[Header("Flags")]
		
		[SerializeField] bool _fitOnStart = true;
		
		[Tooltip("If true, it continuously fits the quad on LateUpdate. Use this option if the Camera field of view changes during play.")]
		[SerializeField] bool _fitOnLateUpdate = false;
		
		
		void Start()
		{
			if (_fitOnStart)
			{
				Fit();
			}
		}

		
		void LateUpdate()
		{
			if (_fitOnLateUpdate)
			{
				Fit();
			}
		}
		
		
		/// <summary>
		/// Fits to a given camera at a given distance and scale factor.
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="distanceMode"></param>
		/// <param name="distance"></param>
		/// <param name="scaleFactor"></param>
		public void FitToCamera(Camera camera, DistanceMode distanceMode, float distance, float scaleFactor = 1.0f)
		{
			Vector2 frustrumSize = Vector2.one;
			float distanceFromCamera = 1f;
			switch (distanceMode)
			{
				case DistanceMode.FromCameraPosition:
					distanceFromCamera = distance;
					frustrumSize = CameraFrustrum.GetFrustrumSizeAt(camera, distanceFromCamera);
					break;
				case DistanceMode.FromNearClipPlane:
					distanceFromCamera = _camera.nearClipPlane + distance;
					frustrumSize = CameraFrustrum.GetFrustrumSizeAt(camera, distanceFromCamera);
					break;
				case DistanceMode.FromFarClipPlane:
					distanceFromCamera = _camera.farClipPlane + distance;
					frustrumSize = CameraFrustrum.GetFrustrumSizeAt(camera, distanceFromCamera);
					break;
			}

			transform.position = camera.transform.position + camera.transform.forward * distanceFromCamera;
			transform.rotation = camera.transform.rotation;
			transform.localScale = new Vector3(frustrumSize.x * scaleFactor, frustrumSize.y * scaleFactor, 1f);
		}
		
		
		/// <summary>
		/// Fits this Quad to the camera.
		/// </summary>
		public void Fit()
		{
			if (_camera == null)
			{
				Debug.LogWarning("Camera is not set!");
				return;
			}
			
			FitToCamera(_camera, _distanceMode, _distance, _scaleFactor);
		}
	}
	
	#if UNITY_EDITOR
	[CustomEditor(typeof(FitQuadToCameraFrustrum))]
	public class FitQuadToCameraFrustrumEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			if (GUILayout.Button("Fit Quad Now"))
			{
				((FitQuadToCameraFrustrum)target).Fit();
			}
		}
	} 
	#endif
}
