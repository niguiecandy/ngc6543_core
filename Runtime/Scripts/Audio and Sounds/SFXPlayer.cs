using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
	[SerializeField] SoundData _sound;
	
	[Tooltip("Negative value : Plays a random sound.")]
	[SerializeField] int _index = -1;
	
	[SerializeField] AudioSource _audioSource;
	
	[SerializeField] bool _loop;
	[SerializeField] bool _playOnStart;
	[SerializeField] bool _playOnEnable;
	
	
	//---------------------------------------------------
	//					UNITY_FRAMEWORK
	//---------------------------------------------------
	#region UNITY_FRAMEWORK
	
    void Reset()
	{
		GetOrAddAudioSource();
	}
	
    void Awake()
    {
		GetOrAddAudioSource();
    }
	
	void Start()
	{
		if (_playOnStart) AutoPlay();
	}
	
	void OnEnable()
	{
		if (_playOnEnable) AutoPlay();
	}
	
	#endregion	// UNITY_FRAMEWORK
	
	
	void GetOrAddAudioSource()
	{
		if (_audioSource != null) return;
		_audioSource = GetComponent<AudioSource>();
		if (_audioSource == null)
		{
			Debug.LogWarning("SFXPlayer requires an AudioSource component. Adding new one...");
			_audioSource = gameObject.AddComponent<AudioSource>();
		}
	}
	
	void AutoPlay()
	{
		if (_loop)
		{
			if (_index < 0)	PlayLoop();
			else PlayLoop(_index);
		}
		else
		{
			if (_index < 0) PlayOneShot();
			else PlayOneShot(_index);
		}
	}


	//---------------------------------------------------
	//				PUBLIC_METHODS_PLAY&STOP
	//---------------------------------------------------
	#region PUBLIC_METHODS_PLAY&STOP

	public void PlayOneShot()
	{
		if (_sound == null) return;
		_sound.PlayOneShot(_audioSource);
	}
	
	public void PlayOneShot(int index)
	{
		if (_sound == null) return;
		_sound.PlayOneShot(_audioSource, index);
	}
	
	public void PlayLoop()
	{
		if (_sound == null) return;
		_sound.PlayLoop(_audioSource);
	}
	
	public void PlayLoop(int index)
	{
		if (_sound == null) return;
		_sound.PlayLoop(_audioSource, index);
	}
	
	public void Stop()
	{
		_audioSource.Stop();
	}

	#endregion // PUBLIC_METHODS_PLAY&STOP


	//---------------------------------------------------
	//		PUBLIC_METHODS_AUDIOSOURCE
	//---------------------------------------------------
	#region PUBLIC_METHODS_AUDIOSOURCE
	// TODO
	#endregion	// PUBLIC_METHODS_AUDIOSOURCE
}
