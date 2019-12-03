using UnityEngine;
using System.Collections;

namespace NGC6543.DebugHelper
{
	public class DoNotRender : MonoBehaviour
	{
		// Attach this script to an object(trigger, boundary, etc..) which doesn't need to be rendered at runtime.
		// This code will destroy the mesh Renderer and mesh filter
		[Tooltip("This script will work when the game is built on a device")]
		public bool build_only = false;
		public bool DestroyOnLoad = false;
		void Start () 
		{
			#if UNITY_EDITOR
			if(build_only) return;
			#endif
			if(DestroyOnLoad) DestroyRenderer();
			else DisableRenderer();
		}

		void DisableRenderer()
		{
			Renderer[] renderers = GetComponentsInChildren<Renderer>();
			for(int i=0 ; i<renderers.Length ; i++)
				renderers[i].enabled = false;
		}

		void DestroyRenderer()
		{
			Renderer[] renderers = GetComponentsInChildren<Renderer>();
			for(int i=0 ; i<renderers.Length ; i++)
				Destroy(renderers[i]);

			MeshFilter[] mfs = GetComponentsInChildren<MeshFilter>();
			for(int i=0 ; i<mfs.Length ; i++)
				Destroy(mfs[i]);
		}

	}
}