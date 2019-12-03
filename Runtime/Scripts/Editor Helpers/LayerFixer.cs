using UnityEngine;
using System.Collections;

namespace NGC6543.DebugHelper
{
	[ExecuteInEditMode]
	public class LayerFixer : MonoBehaviour 
	{

		public string layerName = "Default";
		int crntLayer;
		int desiredLayer;

		#if UNITY_EDITOR
		// Update is called once per frame
		void Update () 
		{
			if(!Application.isPlaying)
			{
				crntLayer = gameObject.layer;
				desiredLayer = LayerMask.NameToLayer(layerName);
				if(crntLayer != desiredLayer)
				{
					Debug.LogWarning(gameObject.name + " in " + transform.root.name + "'s layer is fixed to " + 
					             layerName);
					gameObject.layer = desiredLayer;
				}
			}
		}
		#endif
	}
}