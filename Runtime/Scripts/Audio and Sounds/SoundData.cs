using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New Sound Data.asset",menuName = "NGC6543/New Sound Data")]
public class SoundData : ScriptableObject
{
	[Tooltip("The target AudioMixerGroup that AudioClips will be played through.")]
	[SerializeField] AudioMixerGroup _targetAudioMixerGroup;
	
    [SerializeField] AudioClip[] files;
	
	// v10.1.0
	[System.Obsolete("Obsolete. Use TargetAudioMxierGroup instead.", true)]
    public string audioMixerGroupName;
	
    #if UNITY_EDITOR
    [SerializeField] string memo;
    #endif
    
	/// <summary>
	/// Target AudioMixerGroup that AudioClips will be played through.
	/// </summary>
	/// <value></value>
	public AudioMixerGroup TargetAudioMixerGroup { get { return _targetAudioMixerGroup; } }
	
	
    bool HasAudioClips()
    {
		return files.Length != 0;
    }
    
	/// <summary>
	/// Returns wether an AudioClip of given index is playable or not.
	/// </summary>
	/// <param name="index">The index of an AudioClip to be played.</param>
	/// <returns></returns>
    bool IsPlayable(int index)
    {
        if (!HasAudioClips())
        {
            return false;
        }
        else
        {
            if (index < 0 || index >= files.Length)
            {
                
                return false;
            }
            else if (files[index] == null)
            {
                return false;
            }
            return true;
        }
	}

	/// <summary>
	/// Plays a random AudioClip.
	/// </summary>
	/// <param name="source">Target AudioSource</param>
	/// <param name="_override">If false, the AudioClip won't be played if the AudioSource is playing another AudioClip.</param>
	/// <returns>-1 if there is no AudioClip. Otherwise, returns the length of the AudioClip in seconds.</returns>
	public float Play(AudioSource source, bool _override = false)
    {
        // Handling Exception
        if (!HasAudioClips()) return -1f;

        if(source.isPlaying && !_override){
            return (source.clip.length - source.time); // returns the remaining time
        }

        //plays random
        int i=0;
        if(files.Length>1) i = Random.Range(0, files.Length);
        source.clip = files[i];
        source.Play();
        return files[i].length;
    }

	/// <summary>
	/// Plays an AudioClip of given index.
	/// </summary>
	/// <param name="source">Target AudioSource</param>
	/// <param name="index">AudioClip index to be played.</param>
	/// <param name="_override">If false, the AudioClip won't be played if the AudioSource is playing another AudioClip.</param>
	/// <returns>-1 if AudioClip of given index cannot be played. Otherwise, returns the length of the AudioClip in seconds.</returns>
	public float Play(AudioSource source, int index, bool _override = false)
    {
        // Handling Exception
        if (!IsPlayable(index)) return -1f;

        if(source.isPlaying && !_override){
            return (source.clip.length - source.time); // returns the remaining time
        }
        source.clip = files[index];
        source.Play();
        return files[index].length;
    }

	/// <summary>
	/// Plays a random AudioClip. The AudioClip will be played one time independent of the settings on AudioSource.
	/// </summary>
	/// <param name="source"></param>
	/// <returns>-1 if there is no AudioClip. Otherwise, returns the length of the AudioClip in seconds.</returns>
	public float PlayOneShot(AudioSource source)
    {
        // Handling Exception
        if (!HasAudioClips()) return -1f;
		int index = Random.Range(0, files.Length);
        source.PlayOneShot(files[index]);
		return files[index].length;
    }

	/// <summary>
	/// Plays an AudioClip of given index. The AudioClip will be played one time independent of the settings on AudioSource.
	/// </summary>
	/// <param name="source"></param>
	/// <param name="index"></param>
	/// <returns>-1 if AudioClip of given index cannot be played. Otherwise, returns the length of the AudioClip in seconds.</returns>
	public float PlayOneShot(AudioSource source, int index)
    {
        // Handling Exception
        if (!IsPlayable(index)) return -1f;
        source.PlayOneShot(files[index]);
		return files[index].length;
    }

    /// <summary>
    /// Loops a random AudioClip.
    /// </summary>
    /// <param name="source">Source.</param>
    /// <param name="_override">Force it to play audio even if the source is playing another audio.</param>
    public void PlayLoop(AudioSource source, bool _override = true)
    {
        // Handling Exception
        if (!HasAudioClips()) return;
        
        if(source.isPlaying && !_override) return;
        source.clip = files[0];
        source.loop = true;
        source.Play();
        return ;
    }
	
	/// <summary>
	/// Loops an AudioClip of given index.
	/// </summary>
	/// <param name="source"></param>
	/// <param name="index"></param>
	/// <param name="_override"></param>
    public void PlayLoop(AudioSource source, int index, bool _override = true)
    {
        // Handling Exception
        if (!HasAudioClips()) return;

        if(source.isPlaying && !_override) return;
        source.clip = files[index];
        source.loop = true;
        source.Play();
        return ;
    }
}
