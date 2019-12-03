using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMember : MonoBehaviour
{	
	[Header("Pool Member info")]
	
	// [SerializeField, NotInteractable] PoolManager<PoolMember> _manager;
	[SerializeField, NotInteractable] int _poolIndex = -1;
	
	//=== Flags
	[SerializeField, NotInteractable] bool _isPoolMember = false;
	
	[SerializeField, NotInteractable] bool _isAvailable = true;
	
	
	//=== Properties
	
	public int PoolIndex{ get{ return _poolIndex; } }
	
	/// <summary>
	/// Is this instance a member of a Pool?
	/// </summary>
	/// <value></value>
	public bool IsPoolMember{ get{ return _isPoolMember; } }
	
	/// <summary>
	/// Is this instance available?
	/// </summary>
	/// <value></value>
	public bool IsAvailable{ get{ return _isAvailable; } }
	
	//--------------------------------------------------- 
	
	public virtual void InitPoolMember(int poolIndex)
	{
		// _manager = manager;
		_poolIndex = poolIndex;
		_isPoolMember = true;
		_isAvailable = true;
	}
	
	/// <summary>
	/// Called when this instance is used by PoolManager.
	/// </summary>
	public virtual void Spawn()
	{
		_isAvailable = false;
	}
	
	/// <summary>
	/// Call when this instance is no longer needed.
	/// </summary>
	public virtual void ReturnToPool()
	{
		_isAvailable = true;
	}
}
