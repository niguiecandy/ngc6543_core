using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NGC6543.DebugHelper
{
    /// <summary>
    /// Visualizes variable Collider components. Useful when the gameobject acts as an invisible guide or barrier.
    /// !! CapsuleCollider is not implemented!
    /// </summary>
	public class ColliderVisualizer : MonoBehaviour 
    {
		// Renderers
        //[SerializeField] Renderer[] renderers;
        //[SerializeField] Color gizmoColor_Renderers = new Color(0, 0, 1, 0.5f);
        // Colliders
        [SerializeField] Collider[] colliders;
        [SerializeField] Color gizmoColor_Colliders = new Color(0, 1, 0, 0.5f);
        [SerializeField] bool doNotVisualizeInEditMode = false;
        [SerializeField] bool doNotVisualizeInPlayMode = true;

		private void Reset()
		{
			//HasRenderers();
            HasColliders();
		}

		//public bool HasRenderers()
        //{
        //    renderers = GetComponents<Renderer>();
        //    return renderers != null;
        //}

        public bool HasColliders()
        {
            colliders = GetComponentsInChildren<Collider>();
            return colliders != null;
        }

        #if UNITY_EDITOR
		private void OnDrawGizmos()
		{
            if (!Application.isPlaying && !doNotVisualizeInEditMode)
            {
                VisualizeColliders();
                return;
            }
            if (Application.isPlaying && !doNotVisualizeInPlayMode)
            {
                VisualizeColliders();
                return;
            }
		}
		BoxCollider bc;
		SphereCollider sc;
        MeshCollider mc;
        CapsuleCollider cc;

        void VisualizeColliders()
        {
            Gizmos.color = gizmoColor_Colliders;


            // Store current matrix
            Matrix4x4 prevMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            for (int i = 0; i < colliders.Length; i++)
            {
				if (colliders[i] == null)
				{
					continue;
				}
                if (!colliders[i].enabled)
                {
                    continue;
                }
                if (colliders[i] is BoxCollider)
                {
                    bc = colliders[i] as BoxCollider;
                    Gizmos.DrawCube(bc.center, bc.size);
                    continue;
                }
                if (colliders[i] is SphereCollider)
                {
                    sc = colliders[i] as SphereCollider;
                    Gizmos.DrawSphere(sc.center, sc.radius);
                    continue;
                }
                if (colliders[i] is MeshCollider)
                {
                    mc = colliders[i] as MeshCollider;
                    if (mc.sharedMesh != null)
                    {
                        Gizmos.DrawMesh(mc.sharedMesh);
                    }
                    continue;
                }
                //if (colliders[i] is CapsuleCollider)
                //{
                //    // NOT IMPLEMENTED YET
                //    continue;
                //}
            }
            Gizmos.matrix = prevMatrix;
        }
        #endif
	}

    #if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ColliderVisualizer))]
    public class ColliderVisualizerEditor : Editor
    {
		private void OnEnable()
		{
			
		}

        string updateResult = "";
        public override void OnInspectorGUI()
		{
            EditorGUILayout.HelpBox("Collider visualizer\n" +
                                    "Visualizes colliders using Gizmos. Useful when the gameobject acts as an invisible guide or barrier.\n" +
                                    "Sphere/Box/Mesh colliders are supported", MessageType.Info);
			base.OnInspectorGUI();

            // Automatically updates colliders
            ((ColliderVisualizer)target).HasColliders();
            return;

            if (GUILayout.Button(new GUIContent("Update", "Updates colliders")))
            {
                if (((ColliderVisualizer)target).HasColliders())
                {
                    updateResult = "Colliders updated!";
                }
                else
                {
                    updateResult = "There is no collider component!";
                }
            }
            EditorGUILayout.LabelField(new GUIContent(updateResult));
		}
	}
    #endif
}
