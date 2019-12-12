using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolMember : MonoBehaviour
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

	/// <summary>
	/// Invoked once by PoolManager when this instance was created.
	/// </summary>
	/// <param name="poolIndex"></param>
	public void InitPoolMember(int poolIndex)
	{
		// _manager = manager;
		_poolIndex = poolIndex;
		_isPoolMember = true;
		_isAvailable = true;
		OnEnlistedToPool();
	}


	/// <summary>
	/// Invoked by PoolManager when this instance is spawned.
	/// </summary>
	public void Spawn()
	{
		_isAvailable = false;
		OnSpawn();
	}
	
	
	/// <summary>
	/// Call when this instance is no longer needed.
	/// </summary>
	public void ReturnToPool()
	{
		_isAvailable = true;
		OnReturnedToPool();
	}
	
	
	/// <summary>
	/// Invoked by PoolManager when this instance is removed from a pool.
	/// </summary>
	public void RemoveFromPool()
	{
		_isPoolMember = false;
		OnRemovedFromPool();
	}
	
	
	/// <summary>
	/// Should implement what to do when this instance has been enlisted to a pool.
	/// </summary>
	protected abstract void OnEnlistedToPool();
	
	
	/// <summary>
	/// Should implement what to do when this instance has been spawned.
	/// </summary>
	protected abstract void OnSpawn();
	
	
	/// <summary>
	/// Should implement what to do when this instance has returned to pool.
	/// </summary>
	protected abstract void OnReturnedToPool();
	
	
	/// <summary>
	/// Should implement what to do when this instance is removed from pool.
	/// </summary>
	protected abstract void OnRemovedFromPool();
}
