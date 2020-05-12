using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGC6543
{
	public class PriorityQueue<T> where T : IComparable<T>
	{
		/// <summary>
		/// Binary heap tree structure
		/// </summary>
		private List<T> data;
		
		
		//---------------------------------------------------
		//				PROPERTIES
		//---------------------------------------------------
		
		#region PROPERTIES
		
		public T[] Heap { get => data.ToArray(); }
		
		public int Count { get => data.Count; }
		
		#endregion	// PROPERTIES
		
		
		//---------------------------------------------------
		//				CONSTRUCTORS
		//---------------------------------------------------
		
		#region CONSTRUCTORS
		
		/// <summary>
		/// Creates an empty priority queue.
		/// </summary>
		public PriorityQueue()
		{
			this.data = new List<T>();
		}
		
		
		/// <summary>
		/// Creates a priority queue with values.
		/// </summary>
		/// <param name="data">The data will be ordered accordingly.</param>
		public PriorityQueue(T[] data)
		{
			this.data = new List<T>();
			foreach (var datum in data)
			{
				Enqueue(datum);
			}
		}
		
		#endregion	// CONSTRUCTORS
		
		
		
		//---------------------------------------------------
		//				QUEUE IMPLEMENTATION
		//---------------------------------------------------
		
		#region QUEUE IMPLEMENTATION
		
		/// <summary>
		/// Enqueue an item and sort priority queue.
		/// </summary>
		/// <param name="item"></param>
		public void Enqueue(T item)
		{
			data.Add(item);
			var ci = Count - 1;
			
			while (ci > 0)
			{
				var pi = (ci - 1) / 2;
				if (data[ci].CompareTo(data[pi]) >= 0)
				{
					// The child node has lower priority than that of the parent node.
					break;
				}
				
				Swap(ci, pi);
				ci = pi;
			}
		}
		
		
		/// <summary>
		/// Dequeue an item of highest priority from queue.
		/// </summary>
		/// <returns></returns>
		public T Dequeue()
		{
			if (Count == 0) return default;
			
			var dequeued = data[0];
			
			
			// Re-order priority queue
			var last = Count - 1;
			data[0] = data[last];
			data.RemoveAt(last);
			last--;
			var pi = 0;
			
			while (true)
			{
				var ci = pi * 2 + 1;
				if (ci > last) break;
				var right = ci + 1;
				
				// Find the child node of higher priority
				if (right <= last && data[right].CompareTo(data[ci]) < 0)
				{
					ci = right;
				}
				
				if (data[ci].CompareTo(data[pi]) <= 0)
				{
					// The child node has lower priority than that of the parent node
					break;
				}
				
				Swap(ci, pi);
				pi = ci;
			}
			
			return dequeued;
		}
		
		
		/// <summary>
		/// Peek an item of highest priority.
		/// </summary>
		/// <returns></returns>
		public T Peek()
		{
			if (Count == 0) return default;
			
			return data[0];
		}
		
		#endregion	// QUEUE IMPLEMENTATION
		
		
		void Swap(int i, int j)
		{
			var tmp = data[i];
			data[i] = data[j];
			data[j] = tmp;
		}
	}
}
