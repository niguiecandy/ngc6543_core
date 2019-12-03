using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if TORYUX || TORY_UX
using ToryUX;
#endif

public class SceneLoader : MonoBehaviour 
{
	[System.Serializable]
	public struct SceneInBuild
	{
		// Will be used as Toggle options for ttglSelectedScene.
		public string name;
		// Scene's build index.
		public int sceneIndexInBuild;
	}

	[SerializeField] SceneInBuild[] scenesInBuild;

	[Header("UIs_ToryUX")]
#if TORYUX || TORY_UX
	[SerializeField] ToryToggle ttglSelectedScene;
	[SerializeField] ToryButton tbtnLoadSelectedScene;
#endif

	const string ppttglOptionIndex = "ttglOptionIndex";
	const string ppSceneIndex = "sceneIndex";
	int ttglOptionIndex, sceneIndex;

	private void Start()
	{
		Initialize();
	}

	void Initialize()
	{
		LoadSettings();

		// ttglSelectedScene
		string[] names = new string[scenesInBuild.Length];
		for (int i = 0; i < names.Length; i++)
		{
			names[i] = scenesInBuild[i].name;
		}

#if TORYUX || TORY_UX
		ttglSelectedScene.options = names;
		ttglSelectedScene.SetOptionIndex(ttglOptionIndex, false);
		ttglSelectedScene.onToggle.AddListener(OnToggle);

		tbtnLoadSelectedScene.onClick.AddListener(LoadSelectedScene);
#endif
	}

	void LoadSettings()
	{
		ttglOptionIndex = PlayerPrefs.GetInt(ppttglOptionIndex, 0);
		sceneIndex = PlayerPrefs.GetInt(ppSceneIndex, scenesInBuild[ttglOptionIndex].sceneIndexInBuild);
	}

	void SaveSettings()
	{
		PlayerPrefs.SetInt(ppttglOptionIndex, ttglOptionIndex);
		PlayerPrefs.SetInt(ppSceneIndex, sceneIndex);
		PlayerPrefs.Save();
	}

	void OnToggle(int _index)
	{
		ttglOptionIndex = Mathf.Clamp(_index, 0, scenesInBuild.Length);
		sceneIndex = scenesInBuild[ttglOptionIndex].sceneIndexInBuild;
		SaveSettings();
	}

	public void LoadSelectedScene()
	{
		SceneManager.LoadScene(sceneIndex);
	}
}