using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NGC6543.DebugHelper
{
	public class MoveWithKeyboard : MonoBehaviour 
	{
		const float defaultSpd = 1f;	// Keycode.LeftAlt
		const float slowSpd = 0.2f;	// Keycode.LeftControl
		const float fastSpd = 2f;		// Keycode.LeftShift

		// Translation
		float forward, strafe, upDown, translationMultiplier;

		// Rotation
		float lookVertical, lookHorizontal;
		
		// Update is called once per frame
		void Update () {
			if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl))
			{
				EvaluateInput();
				Translate();
				Rotate();
			}
		}
		
		void EvaluateInput()
		{
			// Translation
			forward = Input.GetAxis("Vertical");
			strafe = Input.GetAxis("Horizontal");
			upDown = (Input.GetKey(KeyCode.Q) ? 1 : 0 ) + (Input.GetKey(KeyCode.E) ? -1 : 0);

			translationMultiplier = Input.GetKey(KeyCode.LeftControl) ? slowSpd
			                             :(Input.GetKey(KeyCode.LeftShift) ? fastSpd : defaultSpd);


			// Rotation
			lookVertical = Input.GetAxis("Mouse Y");
			lookHorizontal = Input.GetAxis("Mouse X");
		}

		void Translate()
		{
			transform.Translate(strafe * translationMultiplier, 
			                    upDown * translationMultiplier, 
			                    forward * translationMultiplier, Space.Self);
		}
		
		void Rotate()
		{
			transform.Rotate(0, lookHorizontal, 0, Space.World);
			transform.Rotate(-lookVertical, 0, 0, Space.Self);
		}
	}

	#if UNITY_EDITOR
	[CustomEditor(typeof(MoveWithKeyboard)), CanEditMultipleObjects]
	public class MoveWithKeyboardEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			EditorGUILayout.HelpBox("Move with Keyboard\n" +
			                        "Press Modifiers below\n" +
			                        "Left Option(Alt) : Default speed\n" +
			                        "Left Shift : Fast\tLeft Control : Slow\n" +
			                        "Modifier + WSAD(↑↓←→) : Translation(XZ)\n" +
			                        "Modifier + Q/E : Translation(Y)\n" +
			                        "Modifier + Mouse Movement : Rotation", MessageType.Info); 
			base.OnInspectorGUI();
		}
	}
	#endif
}
