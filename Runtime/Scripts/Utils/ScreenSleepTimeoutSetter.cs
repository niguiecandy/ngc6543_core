using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGC6543
{
	public class ScreenSleepTimeoutSetter : MonoBehaviour
	{
		const string LOG_HEADER = "ScreenSleepTimeoutController : ";
		
		enum ScreenSleepTimeout
		{
			NeverSleep,
			
			SystemSettings
		}
		
		[SerializeField] ScreenSleepTimeout _sleepTimeout = ScreenSleepTimeout.NeverSleep;
		
		
		void Start()
		{
			Debug.Log(LOG_HEADER + "Setting Screen Sleep Timeout to : " + _sleepTimeout);
			switch (_sleepTimeout)
			{
				case ScreenSleepTimeout.NeverSleep :
					Screen.sleepTimeout = SleepTimeout.NeverSleep;
					break;
				case ScreenSleepTimeout.SystemSettings :
					Screen.sleepTimeout = SleepTimeout.SystemSetting;
					break;
			}
		}
		
	}
}