using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorMemo : MonoBehaviour
{
	[System.Serializable]
	struct Memo
	{
		[TextArea(1, 3)]
		public string description;
		
		[TextArea(3, 7)]
		public string content;
	}
	
	[SerializeField] Memo[] _memos;
}
