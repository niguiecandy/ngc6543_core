﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager<T> : MonoBehaviour where T : PoolMember
{
	[Header("Pool Manager Base Info")]
	
	[SerializeField, NotInteractable] int _instancesCount;
	
	Dictionary<int, T> _members = new Dictionary<int, T>();
	
	Dictionary<int, List<T>> _instancesPool = new Dictionary<int, List<T>>();
	
	
	void Awake()
	{
		InitializeInstancesPool();
	}
	
	
	//--------------------------------------------------- 
	
	void InitializeInstancesPool()
	{
		_instancesPool.Clear();
		_instancesCount = 0;
		foreach(var pair in _members)
		{
			_instancesPool.Add(pair.Key, new List<T>());
		}
	}
	
	
	/// <summary>
	/// Returns the Key value of the dictionary.
	/// </summary>
	/// <param name="member"></param>
	/// <returns>-1 if the member doesn't belong to this pool.</returns>
	public int GetKey(T member)
	{
		var instanceID = member.GetInstanceID();
		if (_members.ContainsKey(instanceID))
		{
			return instanceID;
		}
		else
		{
			return -1;
		}
	}
	
	
	/// <summary>
	/// Adds a member into the pool and returns its index.
	/// </summary>
	/// <param name="member"></param>
	/// <returns>Returns the dictionary key</returns>
	public int AddMember(T member)
	{
		if (GetKey(member) == -1)
		{
			var key = member.GetInstanceID();
			_members.Add(key, member);
			_instancesPool.Add(key, new List<T>());
		}
		
		return member.GetInstanceID();
	}
	
	
	/// <summary>
	/// Adds a new instance of given pool index, and return its instance index.
	/// </summary>
	/// <param name="key"></param>
	/// <returns>The instance index</returns>
	int AddInstance(int key)
	{
		T instance = GameObject.Instantiate(_members[key]);
		instance.InitPoolMember(key);
		_instancesPool[key].Add(instance);
		_instancesCount++;
		return _instancesPool[key].Count - 1;
	}
	
	
	/// <summary>
	/// Returns a PoolMember instance. To boost performance, use <see cref="Use(int)"/> instead.
	/// </summary>
	/// <param name="member"></param>
	/// <returns></returns>
	public T Use(T member)
	{
		int key = GetKey(member);
		if (key == -1)
		{
			key = AddMember(member);
		}
		return Use(key);
	}
	
	
	/// <summary>
	/// Returns a PoolMember instance.
	/// </summary>
	/// <param name="key"></param>
	/// <returns>Null if index is out of bound</returns>
	public T Use(int key)
	{
		if (!_members.ContainsKey(key))
		{
			Debug.LogError("The pool doesn't have a pool member with key " + key);
			return null;
		}
		
		int instanceIndex = GetAvailableInstanceIndex(key);
		if (instanceIndex == -1)
		{
			instanceIndex = AddInstance(key);
		}
		
		_instancesPool[key][instanceIndex].Spawn();
		return _instancesPool[key][instanceIndex];
	}
	
	
	/// <summary>
	/// Returns an instance index of the first available instances.
	/// </summary>
	/// <param name="key"></param>
	/// <returns>-1 if no instance is available</returns>
	int GetAvailableInstanceIndex(int key)
	{
		var targetList = _instancesPool[key];
		
		for (int i = 0; i < targetList.Count; i++)
		{
			if (targetList[i].IsAvailable)
			{
				return i;
			}
		}
		
		//UNDONE done in Use(int)
		// if (targetList.Count == 0)
		// {
		// 	return AddInstance(key);
		// }
		
		return -1;
	}
}
