using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGC6543.DebugHelper
{
	public class CameraDepthSwap : MonoBehaviour 
	{
		[SerializeField] Camera cam;
		[SerializeField] float depthBefore;
		[SerializeField] float depthAfter;
		
		float prevDepth;
		
		void Start () 
		{
			if (cam == null)
			{
				cam = GetComponent<Camera>();
				if (cam == null)
				{
					Debug.LogWarning("[CameraDepthSwap] Camera is not specified!");
					return;
				}
			}
			cam.depth = depthBefore;
			prevDepth = cam.depth;
			Debug.Log("[CameraDepthSwap] Camera depth set to " + cam.depth);
		}
		#if UNITY_EDITOR || UNITY_STANDALONE
		void Update()
		{
			if (cam != null)
			{
				if (Input.GetKeyDown(KeyCode.C))
				{
					Swap();
				}
			}
		}
		#endif
		
		public void Swap()
		{
			if (cam != null)
			{
				if (cam.depth - depthBefore < float.Epsilon)
				{
					SwapToAfter();
				}
				else
				{
					SwapToBefore();
				}
			}
		}
		
		public void SwapToBefore()
		{
			SetCameraDepth(depthBefore);
		}

		public void SwapToAfter()
		{
			SetCameraDepth(depthAfter);
		}
		
		public void SetCameraDepth(float _depth)
		{
			if (cam != null)
			{
				cam.depth = _depth;
			}
		}
	}
}
