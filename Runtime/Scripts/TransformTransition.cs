using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif
using StackableDecorator;

namespace NGC6543
{
    public class TransformTransition : MonoBehaviour 
    {
		public enum TransitionLoopMode 
		{ 	
			None,
			Loop,
			PingPong
		}
		
		[Header("Transforms")]
        
		[SceneOnly]
		[SerializeField, Label(title = "From")]
		[StackableField]
		Transform _from;
		
		[SceneOnly]
		[SerializeField, Label(title = "To")]
		[StackableField]
		Transform _to;
		
		[SceneOnly]
		[SerializeField, Label(title = "Moving")]
		[StackableField]
		Transform _moving;
		
		
		[Header("Transition")]
		
		[HorizontalGroup("Group1", false, "_transitRotation,_transitScale", order=1)]
		[SerializeField, Label(-1, title = "Position", order=0)]
		[StackableField] bool _transitPosition;
		
		[InGroup("Group1", order = 1)]
		[SerializeField, Label(-1, title = "Rotation", order=0)]
		[StackableField] bool _transitRotation;
		
		[InGroup("Group1", order = 1)]
		[SerializeField, Label(-1, title = "Scale", order=0)]
		[StackableField] bool _transitScale;
		
		[Space]
		
		/// <summary>
		/// When the transition starts,
		/// True : All the initial transform informations will be those of _tFrom
		/// False : All the transform informations will be those of _tMoving's current values.
		/// </summary>
		/// <returns></returns>	
		[SerializeField, Label(title = "Normalized Progress")]
		[StackableField]
		AnimationCurve _normProgCurve;
		
        [SerializeField] float _duration = 5f;
		
		[SerializeField] TransitionLoopMode _loopMode = TransitionLoopMode.None;
		
		[Space]
		
		
		[Label(300f), StackableField]
		[SerializeField] bool _playOnAwake;
		
        [SerializeField, Label(300f, title = "Move 'Moving' to 'From' on Awake?")]
		[StackableField]
		bool _moveTotFromOnAwake = true;
		
		[SerializeField, Label(300f, title = "Move 'Moving' to 'From' on start playing?")]
		[StackableField]
		bool _moveTotFromOnPlay = false;
		
        [SerializeField, Label(300f, title = "Null parent 'Moving' on start playing?")]
		[StackableField]
		bool _bNullParentOnPlay;
		
		[SerializeField, Label(300f, title = "Set 'To' to be a parent of 'Moving' on end?")]
		[StackableField]
		bool _bSetChildOnEnd;   // tMoving.SetParent(tTo)
		
		[Space]
		
		[Header ("Events")]
		
        [SerializeField] UnityEvent _onPlay;
		
		[SerializeField] UnityEvent _onLoopPoint;

		
		#region  PROPERTIES
		
		/// <summary>
		/// Enables or disables the position transition.
		/// </summary>
		/// <value></value>
		public bool PositionTransition
		{
			get { return _transitPosition; }
			set { _transitPosition = value; }
		}
		
		/// <summary>
		/// Enables or disables the rotation transition.
		/// </summary>
		/// <value></value>
		public bool RotationTransition
		{
			get { return _transitRotation; }
			set { _transitRotation = value; }
		}
		
		/// <summary>
		/// Enables or disables the scale transition.
		/// </summary>
		/// <value></value>
		public bool ScaleTransition
		{
			get { return _transitScale; }
			set { _transitScale = value; }
		}
		
        public UnityEvent OnStart{ get{ return _onPlay; } }
        
		public UnityEvent OnLoopPoint{ get{ return _onLoopPoint; } }
		
		#endregion	// PROPERTIES
		
		
		#region UNITY_FRAMEWORK
		
        void Reset()
        {
            _normProgCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        }

        private void Awake()
        {
            if (_moveTotFromOnAwake)
            {
                if (_from != null && _moving != null)
                {
                    if (_transitPosition)    _moving.position = _from.position;
                    if (_transitRotation)    _moving.rotation = _from.rotation;
                    if (_transitScale)  _moving.localScale = _from.localScale;
                }
            }
			if (_playOnAwake)
			{
				Play(_from, _to, _moving, _duration, _loopMode);
			}
        }
		
		#endregion	// UNITY_FRAMEWORK
		
		
        //------------------------------------------------ PlayOneShot
        
        public void PlayOneShot()
        {
			Play(_from, _to, _moving, _duration, TransitionLoopMode.None);
        }

        public void PlayOneShot(Transform tMoving)
        {
			Play(_from, _to, tMoving, _duration, TransitionLoopMode.None);
        }
		
		public void PlayOneShot(float duration)
		{
			Play(_from, _to, _moving, duration, TransitionLoopMode.None);
		}
        
        public void PlayOneShot(Transform tMoving, float duration)
        {
			Play(_from, _to, tMoving, duration, TransitionLoopMode.None);
        }

		//--------------------------------------------------- PlayOneShotReversed
        public void PlayOneShotReversed()
        {
			Play(_to, _from, _moving, _duration, TransitionLoopMode.None);
        }
        
        public void PlayOneShotReversed(Transform tMoving)
        {
			Play(_to, _from, tMoving, _duration, TransitionLoopMode.None);
        }
		
		public void PlayOneShotReversed(float duration)
		{
			Play(_to, _from, _moving, duration, TransitionLoopMode.None);
		}

        public void PlayOneShotReversed(Transform tMoving, float duration)
        {
			Play(_to, _from, tMoving, duration, TransitionLoopMode.None);
        }


		//------------------------------------------------ PlayLoop

		public void PlayLoop()
		{
			Play(_from, _to, _moving, _duration, TransitionLoopMode.Loop);
		}

		public void PlayLoop(Transform tMoving)
		{
			Play(_from, _to, tMoving, _duration, TransitionLoopMode.Loop);
		}

		public void PlayLoop(float duration)
		{
			Play(_from, _to, _moving, duration, TransitionLoopMode.Loop);
		}

		public void PlayLoop(Transform tMoving, float duration)
		{
			Play(_from, _to, tMoving, duration, TransitionLoopMode.Loop);
		}


		//------------------------------------------------ PlayPingpong

		public void PlayPingpong()
		{
			Play(_from, _to, _moving, _duration, TransitionLoopMode.PingPong);
		}

		public void PlayPingpong(Transform tMoving)
		{
			Play(_from, _to, tMoving, _duration, TransitionLoopMode.PingPong);
		}

		public void PlayPingpong(float duration)
		{
			Play(_from, _to, _moving, duration, TransitionLoopMode.PingPong);
		}

		public void PlayPingpong(Transform tMoving, float duration)
		{
			Play(_from, _to, tMoving, duration, TransitionLoopMode.PingPong);
		}


		//--------------------------------------------------- 

		public void Play(Transform tFrom, Transform tTo, Transform tMoving, float duration, TransitionLoopMode loopMode)
        {
			_playing = Playing(tFrom, tTo, tMoving, duration, loopMode);
			StartCoroutine(_playing);
        }
        
        IEnumerator _playing;
        public IEnumerator Playing(Transform from, Transform to, Transform moving, float duration, TransitionLoopMode loopMode)
        {
            _onPlay.Invoke();

            // Get out of any parents
            if (_bNullParentOnPlay) moving.SetParent(null);
            
            Vector3 startPos;
            Quaternion startRot;
            Vector3 startScale;
            if (_moveTotFromOnPlay || loopMode == TransitionLoopMode.Loop)
            {
                startPos = from.position;
                startRot = from.rotation;
                startScale = from.localScale;
            }
            else
            {
                startPos = moving.position;
                startRot = moving.rotation;
                startScale = moving.localScale;
            }
            
            float t = 0f;
            float prog = 0f;
            while(t < duration)
            {
                if (_transitPosition)
                    moving.position = Vector3.LerpUnclamped(startPos, to.position, prog);
                if (_transitRotation)
                    moving.rotation = Quaternion.SlerpUnclamped(startRot, to.rotation, prog);
                if (_transitScale)
                    moving.localScale = Vector3.LerpUnclamped(startScale, to.localScale, prog);
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
                prog = _normProgCurve.Evaluate(t / duration);
            }
            if (_transitPosition)               moving.position = to.position;
            if (_transitRotation)               moving.rotation = to.rotation;
            if (_transitScale)             moving.localScale = to.localScale;
            
            if (_bSetChildOnEnd)     moving.SetParent(to);

            // Loop point reached!
			_onLoopPoint.Invoke();
			switch (loopMode)
			{
				case TransitionLoopMode.Loop :
					Play(from, to, moving, duration, loopMode);
					break;
				case TransitionLoopMode.PingPong :
					Play(to, from, moving, duration, loopMode);
					break;
			}
        }
		
        
        public void Stop()
        {
            if (_playing != null)
            {
                StopCoroutine(_playing);
            }
        }
    }
}