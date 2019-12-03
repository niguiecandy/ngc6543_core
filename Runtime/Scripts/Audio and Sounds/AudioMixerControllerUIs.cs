using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if TORYUX
using ToryUX;
#endif
using UnityEngine.UI;

namespace NGC6543
{
	public class AudioMixerControllerUIs : MonoBehaviour 
	{
		[SerializeField] AudioMixerController audioMixerController;
		#if TORYUX
		[Header("ToryUX")]
		[SerializeField] TorySlider tSliderBGMVolume;
		[SerializeField] TorySlider tSliderSFXVolume;
		[SerializeField] TorySlider tSliderVoiceVolume;
		#else
		[Header("UnityEngine.UI")]
		[SerializeField] Slider sliderBGMVolume;
		[SerializeField] Slider sliderSFXVolume;
		[SerializeField] Slider sliderVoiceVolume;
		#endif
		void Start () {
			Initialize();
		}

		void Initialize()
		{
			if (audioMixerController == null)
			{
				Debug.LogWarning("AudioMixerController is null!");
				return;
			}
			#if TORYUX
			if (tSliderBGMVolume != null)
			{
				tSliderBGMVolume.value = audioMixerController.GetBGMVolumeNormalized();
				tSliderBGMVolume.onValueChanged.AddListener(audioMixerController.SetBGMVolume);
			}

			if (tSliderSFXVolume != null)
			{
				tSliderSFXVolume.value = audioMixerController.GetSFXVolumeNormalized();
				tSliderSFXVolume.onValueChanged.AddListener(audioMixerController.SetSFXVolume);
			}

			if (tSliderVoiceVolume != null)
			{
				tSliderSFXVolume.value = audioMixerController.GetVoiceVolumeNormalized();
				tSliderVoiceVolume.onValueChanged.AddListener(audioMixerController.SetVoiceVolume);
			}

			#else

			if (sliderBGMVolume != null)
			{
				sliderBGMVolume.value = audioMixerController.GetBGMVolumeNormalized();
				sliderBGMVolume.onValueChanged.AddListener(audioMixerController.SetBGMVolume);
			}

			if (sliderSFXVolume != null)
			{
				sliderSFXVolume.value = audioMixerController.GetSFXVolumeNormalized();
				sliderSFXVolume.onValueChanged.AddListener(audioMixerController.SetSFXVolume);
			}

			if (sliderVoiceVolume != null)
			{
				sliderSFXVolume.value = audioMixerController.GetVoiceVolumeNormalized();
				sliderVoiceVolume.onValueChanged.AddListener(audioMixerController.SetVoiceVolume);
			}
			#endif
		}

	}
}
