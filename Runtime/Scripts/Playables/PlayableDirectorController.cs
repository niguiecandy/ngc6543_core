using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NGC6543
{
	[RequireComponent(typeof(PlayableDirector))]
	public class PlayableDirectorController : MonoBehaviour
	{
		[SerializeField, HideInInspector]
		PlayableDirector _director;

		void Reset()
		{
			_director = GetComponent<PlayableDirector>();
		}

		void Awake()
		{
			_director = GetComponent<PlayableDirector>();
		}

		public void Play()
		{
			Debug.Log("Playing " + _director.name + " Timeline...");
			_director.Play();
		}

		public void Pause()
		{
			Debug.Log(_director.name + " Timeline has been paused.");
			_director.Pause();
		}

		public void Stop()
		{
			Debug.Log(_director.name + " Timeline has been stopped.");
			_director.Stop();
		}

		public void Rewind()
		{
			Debug.Log(_director.name + " Timeline has been rewinded.");
			_director.Rewind();
		}


		public void StopAndRewind()
		{
			Debug.Log(_director.name + " Timeline has been stopped and rewinded.");
			_director.StopAndRewind();
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(PlayableDirectorController))]
	public class DEV_TimelinePlayerEditor : Editor
	{
		PlayableDirectorController _component;
		Color _playButtonColor = new Color(0.1f, 0.9f, 0.1f, 1);
		Color _pauseButtonColor = new Color(0.9f, 0.8f, 0.2f, 1);
		Color _stopButtonColor = new Color(0.9f, 0.1f, 0.1f, 1);


		void OnEnable()
		{
			_component = target as PlayableDirectorController;
		}

		public override void OnInspectorGUI()
		{
			if (!Application.isPlaying)
			{
				GUILayout.Label("Not in Play mode.");
				GUI.enabled = false;
			}

			EditorGUILayout.BeginHorizontal();

			var cachedColor = GUI.backgroundColor;
			GUI.backgroundColor = _playButtonColor;
			if (GUILayout.Button("Play"))
			{
				_component.Play();
			}

			GUI.backgroundColor = _pauseButtonColor;
			if (GUILayout.Button("Pause"))
			{
				_component.Pause();
			}

			GUI.backgroundColor = _stopButtonColor;
			if (GUILayout.Button("Stop"))
			{
				_component.Stop();
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();

			GUI.backgroundColor = _pauseButtonColor;
			if (GUILayout.Button("Rewind"))
			{
				_component.Rewind();
			}

			GUI.backgroundColor = _stopButtonColor;
			if (GUILayout.Button("Stop and Rewind"))
			{
				_component.StopAndRewind();
			}

			EditorGUILayout.EndHorizontal();

			GUI.backgroundColor = cachedColor;
		}
	}
#endif
}