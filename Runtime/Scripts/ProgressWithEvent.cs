using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ProgressWithEvent : MonoBehaviour
{
	[System.Serializable]
	public class FloatEvent : UnityEvent<float> { }
	/// <summary>
	/// ProgressEvent
	/// - rawThreshold : Event position as raw value. The events will be invoked based on this value.
	/// - normalizedThreshold : Event position as normalized value. If the normalizedThreshold doesn't match the rawThreshold (!=rawThreshold/maxRawThreshold), the progress will be lerped to match the normalizedThreshold.
	/// </summary>
	[System.Serializable]
	public struct ProgressEvent
	{
		/// <summary>
		/// Description for the Inspector.
		/// </summary>
		[SerializeField] string description;
		
		/// <summary>
		/// The raw progress value for threshold.
		/// </summary>
		public float rawThreshold;
		
		/// <summary>
		/// The normalized progress value for threshold.
		/// </summary>
		[Range(0f, 1f)] public float normalizedThreshold;
		
		/// <summary>
		/// Invoke if this.rawThreshold has been reached from lower value.
		/// </summary>
		public UnityEvent onReachFromLowToHigh;
		
		/// <summary>
		/// Invoke if this.rawThreshold has been reached from higher value.
		/// </summary>
		public UnityEvent onReachFromHighToLow;
	}
	
	
	[SerializeField] float _targetRawProgress = 0f;
	
	[SerializeField] float _maxRawProgress = 100f;
	
	[SerializeField] float _smoothTime = 0.3f;
	
	[SerializeField, NotInteractable] float _dampedRawProgress = 0f;
	
	[Tooltip("The value will be lerped accordinly to match the Normalized Thresholds in Progress Events.")]
	[SerializeField, NotInteractable] float _normalizedProgress = 0f;
	
	[Header("Events")]
	
	[SerializeField] List<ProgressEvent> _progressEvents = new List<ProgressEvent>();
	
	List<int> tmp = new List<int>();
	
	// temporary variables for SmoothDamp
	float _prevProgress = 0f;
	float _refVel;
	
	
	//=== PROPERTIES
	
	public float MaxRawProgress 
	{ 
		get { return _maxRawProgress; } 
		set { _maxRawProgress = value; } 
	}
	
	public float NormalizedProgress
	{
		get	{ return _normalizedProgress; }
	}
	
	public event System.Action<float> OnNormalizedProgressUpdate = (float a) => {};

	public FloatEvent NormalizedProgressUpdated;
	
	#region UNITY_FRAMEWORK
	
	void Update()
	{
		_targetRawProgress = Mathf.Clamp(_targetRawProgress, 0f, _maxRawProgress);
		_dampedRawProgress = Mathf.SmoothDamp(_dampedRawProgress, _targetRawProgress, ref _refVel, _smoothTime);
		if (Mathf.Abs(_refVel) < 0.1f) _dampedRawProgress = _targetRawProgress;
		Evaluate();
		_prevProgress = _dampedRawProgress;
	}
	
	void OnDestroy()
	{
		// if (_scoreManager != null)
		// 	_scoreManager.UnregisterObserver(this);
	}
	
	#endregion	// UNITY_FRAMEWORK
	
	/// <summary>
	/// Adds a new ProgressEvent to the list. The list will be adjusted to an ascending order of normalizedProgressThreshold.
	/// </summary>
	/// <param name="pEvent"></param>
	public void AddProgressEvent(ProgressEvent pEvent)
	{
		if (_progressEvents.Count == 0)
		{
			_progressEvents.Add(pEvent);
		}
		else
		{
			bool inserted = false;
			for (int i = 0; i < _progressEvents.Count; i++)
			{
				if (pEvent.normalizedThreshold < _progressEvents[i].normalizedThreshold)
				{
					_progressEvents.Insert(i, pEvent);
					inserted = true;
					break;
				}
			}
			if (!inserted)
			{
				_progressEvents.Add(pEvent);
			}
		}
	}
	
	public void RemoveProgressEvent(ProgressEvent pEvent)
	{
		if (_progressEvents.Count == 0) return;
		_progressEvents.Remove(pEvent);
		_progressEvents.TrimExcess();
	}

	
	/// <summary>
	/// Sets the raw progress value.
	/// </summary>
	/// <param name="progress">The value will be clamped into [0, maxRawProgress]</param>
	public void SetRawProgress (int progress)
	{
		SetRawProgress((float)progress);
	}
	
	/// <summary>
	/// Sets the raw progress value.
	/// </summary>
	/// <param name="progress">The value will be clamped into [0, maxRawProgress]</param>
	public void SetRawProgress (float progress)
	{
		_targetRawProgress = Mathf.Clamp(progress, 0f, _maxRawProgress);
	}
	
	void Evaluate()
	{
		// Normalized Progress
		if (_progressEvents.Count != 0)
		{
			float n = -1f;
			float lowerValue = 0f;
			float lowerValueNorm = 0f;
			float upperValueNorm = 0f;
			for (int i = 0 ; i < _progressEvents.Count ; i++)
			{
				if (_dampedRawProgress < _progressEvents[i].rawThreshold)
				{
					upperValueNorm = _progressEvents[i].normalizedThreshold;
					n = GetNormalizedProgress(_dampedRawProgress-lowerValue,lowerValue, _progressEvents[i].rawThreshold, lowerValueNorm, upperValueNorm);
					n = float.IsNaN(n) ? upperValueNorm : n;
					break;
				}
				else
				{
					lowerValue = _progressEvents[i].rawThreshold;
					lowerValueNorm = _progressEvents[i].normalizedThreshold;
				}
			}
			
			if ( n < 0f)
			{
				lowerValue = _progressEvents[_progressEvents.Count-1].rawThreshold;
				lowerValueNorm = _progressEvents[_progressEvents.Count-1].normalizedThreshold;
				upperValueNorm = 1f;
				n = GetNormalizedProgress(_dampedRawProgress-lowerValue, lowerValue, _maxRawProgress, lowerValueNorm, upperValueNorm);
				n = float.IsNaN(n) ? upperValueNorm : n;
			}
			
			_normalizedProgress = n;
		}
		else
		{
			_normalizedProgress = _dampedRawProgress / _maxRawProgress;
		}

		OnNormalizedProgressUpdate(_normalizedProgress);
		NormalizedProgressUpdated.Invoke(_normalizedProgress);
		
		// Score event
		for (int i = 0; i < _progressEvents.Count; i++)
		{
			if (_dampedRawProgress >= _prevProgress)
			{
				if (CrossScoreFromLowToHigh(_prevProgress, _dampedRawProgress, _progressEvents[i].rawThreshold))
				{
					_progressEvents[i].onReachFromLowToHigh.Invoke();
				}
			}
			else
			{
				if (CrossScoreFromHighToLow(_prevProgress, _dampedRawProgress, _progressEvents[i].rawThreshold))
				{
					_progressEvents[i].onReachFromHighToLow.Invoke();
				}
			}
		}
		
	}
	
	float GetNormalizedProgress(float value, float lowerValue, float upperValue, float lowerValueNorm, float upperValueNorm)
	{
		return value / (upperValue - lowerValue) * (upperValueNorm - lowerValueNorm) + lowerValueNorm;
	}

	bool CrossScoreFromLowToHigh(float valueBefore, float valueAfter, float threshold)
	{
		return valueAfter >= threshold
			&& valueBefore < threshold;
	}

	bool CrossScoreFromHighToLow(float valueBefore, float valueAfter, float threshold)
	{
		return valueAfter < threshold
			&& valueBefore >= threshold;
	}
}
