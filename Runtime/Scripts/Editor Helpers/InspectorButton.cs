using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NGC6543
{
	public class InspectorButton : MonoBehaviour
	{
		[System.Serializable]
		struct Actions
		{
			public string name;
			public UnityEngine.Events.UnityEvent unityEvents;
			[HideInInspector]
			public bool foldout;
		}
		
		[SerializeField] Actions[] _actions;
		
		
		public void InvokeAll()
		{
			foreach (var action in _actions)
			{
				action.unityEvents.Invoke();
			}
		}
		
		public void Invoke(int index)
		{
			if (index < 0 || index >= _actions.Length)
			{
				Debug.LogError("Index is out of bound.");
				return;
			}
			_actions[index].unityEvents.Invoke();
		}
	}
	
	
	#if UNITY_EDITOR
	
	[CustomEditor(typeof(InspectorButton))]
	public class InspectorButtonEditor : Editor
	{
		const string _actionsPropName = "_actions";
		SerializedProperty _actions;
		
		InspectorButton _component;
		
		Color _buttonColor = new Color(0.1f, 0.8f, 0.2f, 1f);
		
		void OnEnable()
		{
			_component = target as InspectorButton;
			_actions = serializedObject.FindProperty(_actionsPropName);
		}
		
		public override void OnInspectorGUI()
		{
			if (!Application.isPlaying)
			{
				EditorGUILayout.HelpBox("Invoke buttons are shown when in Play Mode.", MessageType.Info);
			}
			
			serializedObject.Update();
			
			var cachedBackgroundColor = GUI.backgroundColor;
			
			EditorGUI.BeginChangeCheck();
			_actions.isExpanded = EditorGUILayout.Foldout(_actions.isExpanded, "Actions");
			if (_actions.isExpanded)
			{
				EditorGUI.indentLevel++;
				_actions.arraySize = EditorGUILayout.DelayedIntField("Size", _actions.arraySize);
				for (int i = 0; i < _actions.arraySize; i++)
				{
					var action = _actions.GetArrayElementAtIndex(i);
					var name = action.FindPropertyRelative("name");
					var foldout = action.FindPropertyRelative("foldout");
					// foldout.boolValue = EditorGUILayout.Foldout(foldout.boolValue, i.ToString() + ":" + action.FindPropertyRelative("name").stringValue);
					
					EditorGUILayout.PropertyField(action, new GUIContent(name.stringValue), true);
					if (Application.isPlaying)
					{
						GUI.backgroundColor = _buttonColor;
						if (GUILayout.Button("Invoke " + name.stringValue))
						{
							_component.Invoke(i);
						}
						GUI.backgroundColor = cachedBackgroundColor;
					}
				}
				EditorGUI.indentLevel--;
			}
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
	#endif
}
