using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace NGC6543
{
	
	public class AudioMixerController : MonoBehaviour 
	{
		/// <summary>
		/// The Audio Mixer. Only works on the AudioMixer created by NGC6543
		/// </summary>
		[SerializeField] AudioMixer mixer;

		[Space]
		/// <summary>
		/// The volume range in dB. Setting normalized volume will be interpolated into this range.
		/// </summary>
		[SerializeField, MinMaxRange(-80f, 0f)] MinMaxRange volumeRange_dB;

		[Space]
		[Header("Basic Audio Sources")]
		[SerializeField] AudioSource bgmAudioSource;
		[SerializeField] AudioSource sfxAudioSource;
		[SerializeField] AudioSource voiceAudioSource;
		
		//=== Properties
		public AudioMixer Mixer{ get{ return Mixer; } }
		public AudioSource BGM{ get{ return bgmAudioSource; } }
		public AudioSource SFX{ get{ return sfxAudioSource; } }
		public AudioSource Voice{ get{ return voiceAudioSource; } }

		//===
		[SerializeField, HideInInspector] GameObject bgmGO, sfxGO, voiceGO;
		const string ppBGMVol = "BGMVol";
		const string ppSFXVol = "SFXVol";
		const string ppVoiceVoldB = "VoiceVol";
		
		/// <summary>
		/// Normalized volumes
		/// </summary>
		float bgmVol, sfxVol, voiceVol;

		//=== Flags
		/// <summary>
		/// true : LoadSettings() is called.
		/// </summary>
		bool isLoaded;

		private void Reset()
		{
			if (bgmGO == null)
			{
				bgmGO = new GameObject("BGM Audio Source");
				bgmGO.transform.SetParent(transform);
				bgmGO.transform.localPosition = Vector3.zero;
				bgmGO.transform.localRotation = Quaternion.identity;
				bgmGO.transform.localScale = Vector3.one;
				bgmAudioSource = bgmGO.AddComponent<AudioSource>();
			}
			if (sfxGO == null)
			{
				sfxGO = new GameObject("SFX Audio Source");
				sfxGO.transform.SetParent(transform);
				sfxGO.transform.localPosition = Vector3.zero;
				sfxGO.transform.localRotation = Quaternion.identity;
				sfxGO.transform.localScale = Vector3.one;
				sfxAudioSource = sfxGO.AddComponent<AudioSource>();
			}
			if (voiceGO == null)
			{
				voiceGO = new GameObject("Voice Audio Source");
				voiceGO.transform.SetParent(transform);
				voiceGO.transform.localPosition = Vector3.zero;
				voiceGO.transform.localRotation = Quaternion.identity;
				voiceGO.transform.localScale = Vector3.one;
				voiceAudioSource = voiceGO.AddComponent<AudioSource>();
			}
			volumeRange_dB = new MinMaxRange();
			volumeRange_dB.min = -80f;
			volumeRange_dB.max = 0f;
		}

		private void Awake()
		{
			if (!isLoaded)
			{
				LoadSettings();
			}
		}

		//------------------------------------------------ Load / Save volumes
		void LoadSettings()
		{
			isLoaded = true;
			// Get normalized volumes from PlayerPrefs
			bgmVol = PlayerPrefs.GetFloat(ppBGMVol, 1f);
			sfxVol = PlayerPrefs.GetFloat(ppSFXVol, 1f);
			voiceVol = PlayerPrefs.GetFloat(ppVoiceVoldB, 1f);
			// Update the AudioMixer
			SetBGMVolume(bgmVol);
			SetSFXVolume(sfxVol);
			SetVoiceVolume(voiceVol);
		}
		
		void SaveSettings()
		{
			PlayerPrefs.SetFloat(ppBGMVol, bgmVol);
			PlayerPrefs.SetFloat(ppSFXVol, sfxVol);
			PlayerPrefs.SetFloat(ppVoiceVoldB, voiceVol);
		}
		
		public void AssignOutputAudioMixerGroup()
		{
			if (mixer == null) 
			{
				Debug.LogError("Audio Mixer is not assigned!");
				return;
			}
			if (bgmAudioSource != null)
				bgmAudioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[1];
			if (sfxAudioSource != null)
				sfxAudioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[1];
			if (voiceAudioSource != null)
				voiceAudioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Voice")[1];
		}

		//------------------------------------------------ Volume Control
		/// <summary>
		/// Returns the normalized BGM volume.
		/// </summary>
		/// <returns>The normalized BGM Volume.</returns>
		public float GetBGMVolumeNormalized()
		{
			if (!isLoaded) LoadSettings();
			return bgmVol;
		}
		
		/// <summary>
		/// Returns the normalized SFX volume.
		/// </summary>
		/// <returns>The normalized SFX volume.</returns>
		public float GetSFXVolumeNormalized()
		{
			if (!isLoaded) LoadSettings();
			return sfxVol;
		}
		
		/// <summary>
		/// Returns the normalized voice volume.
		/// </summary>
		/// <returns>The normalized voice volume.</returns>
		public float GetVoiceVolumeNormalized()
		{
			if (!isLoaded) LoadSettings();
			return voiceVol;
		}
		
		/// <summary>
		/// Sets the BGM Volume.
		/// </summary>
		/// <param name="_normalizedVolume">Normalized volume(0~1).</param>
		public void SetBGMVolume(float _normalizedVolume)
		{
			bgmVol = Mathf.Clamp01(_normalizedVolume);
			mixer.SetFloat("BGM", volumeRange_dB.Get01(bgmVol));
			SaveSettings();
		}
		
		/// <summary>
		/// Sets the SFX Volume.
		/// </summary>
		/// <param name="_normalizedVolume">Normalized volume(0~1).</param>
		public void SetSFXVolume(float _normalizedVolume)
		{
			sfxVol = Mathf.Clamp01(_normalizedVolume);
			mixer.SetFloat("SFX", volumeRange_dB.Get01(sfxVol));
			SaveSettings();
		}
		
		/// <summary>
		/// Sets the voice volume.
		/// </summary>
		/// <param name="_normalizedVolume">Normalized volume(0~1).</param>
		public void SetVoiceVolume(float _normalizedVolume)
		{
			voiceVol = Mathf.Clamp01(_normalizedVolume);
			mixer.SetFloat("Voice", volumeRange_dB.Get01(voiceVol));
			SaveSettings();
		}
	}
	#if UNITY_EDITOR
	[CustomEditor(typeof(AudioMixerController))]
	public class AudioMixerControllerEditor : Editor
	{
		SerializedProperty mixer;

		private void OnEnable()
		{
			mixer = serializedObject.FindProperty("mixer");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (mixer.serializedObject != null)
			{
				if(GUILayout.Button(new GUIContent("Assign outputMixerGroups to AudioSources")))
				{
					((AudioMixerController)target).AssignOutputAudioMixerGroup();
				}
			}
		}
	}
#endif
}