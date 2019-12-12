using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NGC6543;
using StackableDecorator;


public class PFXMember : PoolMember
{
	[Header("Particle System")]
	
	[Tooltip("If not set, it will try to get component at Awake() or when the PoolMember is initialized.")]
	[SerializeField] ParticleSystem _particleSystem;
	
	[Tooltip("If true, the ParticleSystem will be played automatically on spawn.")]
	[SerializeField] bool _playOnSpawn = true;
	
	[Header("SFX")]
	
	[HelpBox("Sound Effect for this Particle Effect.\n" +
		"If the Particle System is looping, the Sound Effect will also loop. The sync between PFX and SFX may incorrect. Please manually sync them at the moment."
		, below = false)
	, StackableField]
	
	[Tooltip("If true, SFX will be automatically played when the ParticleSystem is played.")]
	[SerializeField] bool _playSFX = false;
	
	[Tooltip("If true, the SFX will be stopped immediately when Stop() is called. Otherwise, it will keep playing until all the particles are dead.")]
	[SerializeField] bool _forceStopSFX = false;
	
	[Tooltip("If not set, it will try to get component at Awake() or when the PoolMember is initialized.")]
	[SerializeField] AudioSource _audioSource;
	
	[SerializeField] SoundData _soundData;
	
	[Tooltip("If negative, random sfx will be played.")]
	[SerializeField] int _soundIndex = -1;
	
	
	bool _isInitialized;
	IEnumerator _particlesAliveCheckCoroutine;
	
	
	//--------------------------------------------------- UNITY_FRAMEWORK
	#region  UNITY_FRAMEWORK
	
	void Reset()
	{
		_particleSystem = GetComponent<ParticleSystem>();
	}
	
	
	void Awake()
	{
		InitializeParticleSystem();
		InitializeSFX();
	}
	
	#endregion // UNITY_FRAMEWORK
	
	
	//--------------------------------------------------- POOLMEMBER
	#region  PoolMember
	
	protected override void OnEnlistedToPool()
	{
		InitializeParticleSystem();
		InitializeSFX();
	}
	
	
	protected override void OnSpawn()
	{
		gameObject.SetActive(true);
		if (_playOnSpawn)
		{
			Play();
		}
	}
	
	
	protected override void OnReturnedToPool()
	{
		gameObject.SetActive(false);
	}


	protected override void OnRemovedFromPool()
	{
		GameObject.Destroy(gameObject);
	}
	
	#endregion	// Pool Member
	
	
	public void SetParticleSystem (ParticleSystem instance)
	{
		_particleSystem = instance;
	}


	void InitializeParticleSystem()
	{
		if (_particleSystem == null)
		{
			_particleSystem = GetComponent<ParticleSystem>();
			if (_particleSystem == null)
			{
				Debug.LogError(gameObject.name + " ParticleSystem is not set!");
				_isInitialized = false;
				return;
			}
		}
		_particlesAliveCheckCoroutine = ParticlesAliveCheck();
		_isInitialized = true;
	}
	
	
	void InitializeSFX()
	{
		if (_playSFX)
		{
			if (_audioSource == null)
			{
				_audioSource = GetComponent<AudioSource>();
				if (_audioSource == null)
				{
					Debug.LogWarning(gameObject.name + " Play SFX is enabled but it has no AudioSource component. Adding new one...");
					_audioSource = gameObject.AddComponent<AudioSource>();
				}
			}
		}
	}
	
	
	/// <summary>
	/// Plays the ParticleSystem and all the children ParticleSystems.
	/// </summary>
	public void Play()
	{
		if (!_isInitialized) InitializeParticleSystem();
		
		if (_particleSystem == null) return;
		
		_particleSystem.Play(true);
		
		if (_playSFX)
		{
			if (_soundIndex < 0)
			{
				if (_particleSystem.main.loop)
				{
					_soundData.PlayLoop(_audioSource);
				}
				else
				{
					_soundData.PlayOneShot(_audioSource);
				}
			}
			else
			{
				if (_particleSystem.main.loop)
				{
					_soundData.PlayLoop(_audioSource, _soundIndex);
				}
				else
				{
					_soundData.PlayOneShot(_audioSource, _soundIndex);
				}
			}
		}
		
		StartCoroutine(_particlesAliveCheckCoroutine);
	}
	
	
	/// <summary>
	/// Stops the Particle System.
	/// </summary>
	public void Stop()
	{
		if (_particleSystem != null)
		{
			_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		}
		if (_playSFX && _forceStopSFX)
		{
			_audioSource.Stop();
		}
		if (IsPoolMember)
		{
			ReturnToPool();
		}
	}
	
	
	IEnumerator ParticlesAliveCheck()
	{
		bool isAlive = true;
		while (isAlive)
		{
			// Check if all the particles are dead.
			// Children ParticleSystems are included.
			isAlive = _particleSystem.IsAlive(true);
			yield return new WaitForSeconds(0.5f);
		}
		// Stop looping SFX
		if (_playSFX && _particleSystem.main.loop)
		{
			_audioSource.Stop();
		}
		// Return to pool
		if (IsPoolMember)
		{
			ReturnToPool();
		}
	}

}
