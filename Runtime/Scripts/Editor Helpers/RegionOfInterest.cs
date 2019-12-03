using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NGC6543
{
	/// <summary>
	/// 3차원 공간 상에서 관심영역을 설정하고, 그 공간에 대한 접근을 용이하게 해주는 스크립트.
	/// 기존의 Bounds 구조체와 달리 Global Axis 가 아닌 Local Axis(transform) 에 정렬됨.
	/// </summary>
	public class RegionOfInterest : MonoBehaviour 
	{
		public enum REGIONTYPE {SPHERE, SHELL, CUBE}
		
		[SerializeField] REGIONTYPE _regionType = REGIONTYPE.CUBE;
		[SerializeField] bool _showGUIOnEditor = true;
		[SerializeField] bool _showGUIOnPlay = false;
		/*
			CUBE type.
			Defined by Bounds.
		 */
		[SerializeField] Bounds _bound = new Bounds(Vector3.zero, Vector3.one);
		
		/*
			SPHERICAL type.
			Defined by inner radius.
			Inner radius should be equal or greater than 0.
		 */
		
		[SerializeField] float _innerRadius = 1f;
		
		/*
			SHELL type.
			Defined by inner radius and outer radius.
			Outer radius should be equal or greater than inner radius.
			inner radius should be equal or greater than 0.
		 */
		[SerializeField] float _outerRadius = 2f;
		
		
		[SerializeField] Color _gizmoColor = new Color(0,1,0, 0.4f);
		
		public Vector3 GetRandomInside()
		{
			switch (_regionType)
			{
				case REGIONTYPE.CUBE :
					return transform.TransformPoint(_bound.GetRandomInside());
				case REGIONTYPE.SPHERE :
					return transform.TransformPoint(Random.onUnitSphere.normalized * _innerRadius);
				case REGIONTYPE.SHELL :
					return transform.TransformPoint(Random.onUnitSphere.normalized * Random.Range(_innerRadius, _outerRadius));
				default :
					Debug.LogError("THIS CAN'T HAPPEN!");
					return Vector3.zero;
			}
		}
		
		
		void OnDrawGizmos()
		{
			
			if (!Application.isPlaying && !_showGUIOnEditor) return;
			if (Application.isPlaying && !_showGUIOnPlay) return;
			
			Gizmos.color = _gizmoColor;
			Matrix4x4 gizmoMat = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
			Gizmos.matrix = gizmoMat;
            switch (_regionType)
            {
                case REGIONTYPE.CUBE:
                    Gizmos.DrawCube(_bound.center, _bound.size);
					break;
                case REGIONTYPE.SPHERE:
					Gizmos.DrawSphere(Vector3.zero, _innerRadius);
                    break;
                case REGIONTYPE.SHELL:
					Gizmos.DrawSphere(Vector3.zero, _innerRadius);
					Gizmos.DrawSphere(Vector3.zero, _outerRadius);
                    break;
            }
		}
	}
	
#if UNITY_EDITOR
	[CustomEditor(typeof(RegionOfInterest)), CanEditMultipleObjects]
	public class RegionOfInterestEditor : Editor
	{
		RegionOfInterest _component;
		// TransformBounds[] _components;
		
		SerializedProperty regionType;
        RegionOfInterest.REGIONTYPE currentType;
		
		SerializedProperty bound;
		SerializedProperty innerRadius;
		SerializedProperty outerRadius;
		SerializedProperty gizmoColor;
		
		SerializedProperty _showGUIOnEditor;
		SerializedProperty _showGUIOnPlay;
		
		float ir, or;
		
		void OnEnable()
		{
			_component = target as RegionOfInterest;
			// _components = targets as TransformBounds[];
			
			regionType = serializedObject.FindProperty("_regionType");
			
			bound = serializedObject.FindProperty("_bound");
			innerRadius = serializedObject.FindProperty("_innerRadius");
			outerRadius = serializedObject.FindProperty("_outerRadius");
			gizmoColor = serializedObject.FindProperty("_gizmoColor");
			
			_showGUIOnEditor = serializedObject.FindProperty("_showGUIOnEditor");
			_showGUIOnPlay = serializedObject.FindProperty("_showGUIOnPlay");
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			
			EditorGUILayout.PropertyField(regionType, new GUIContent("Region Type"));
			currentType = (RegionOfInterest.REGIONTYPE)regionType.enumValueIndex;
			
			switch (currentType)
			{
				case RegionOfInterest.REGIONTYPE.CUBE :
					EditorGUILayout.PropertyField(bound, new GUIContent("Bounds"));
					break;
				case RegionOfInterest.REGIONTYPE.SPHERE :
					EditorGUI.BeginChangeCheck();
					ir = EditorGUILayout.FloatField("Radius",innerRadius.floatValue);
					if (EditorGUI.EndChangeCheck())
					{
						ir = Mathf.Max(0f, ir);
						innerRadius.floatValue = ir;
					}
					break;
				case RegionOfInterest.REGIONTYPE.SHELL :
					EditorGUI.BeginChangeCheck();
					ir = EditorGUILayout.FloatField("Inner Radius", innerRadius.floatValue);
					or = EditorGUILayout.FloatField("Outer Radius", outerRadius.floatValue);
					if (EditorGUI.EndChangeCheck())
					{
						ir = Mathf.Max(0f, ir);
						or = Mathf.Max(ir, or);
						innerRadius.floatValue = ir;
						outerRadius.floatValue = or;
					}
					break;
			}
			
			EditorGUILayout.PropertyField(gizmoColor, new GUIContent("Gizmo Color"));
			
			_showGUIOnEditor.boolValue = EditorGUILayout.ToggleLeft("Show GUI on Editor", _showGUIOnEditor.boolValue);
			
			_showGUIOnPlay.boolValue = EditorGUILayout.ToggleLeft("Show GUI on Play", _showGUIOnPlay.boolValue);
			
			serializedObject.ApplyModifiedProperties();
		}
	}
#endif
}