using System;
using System.Collections;
using System.Collections.Generic;

namespace JollyRoger.Collections
{
	/// <summary>
	/// A list that will notify of changes to the list itself (add, remove, etc).
	/// For a list that reports on the elements changing <see cref="DeepObservableList"/>.
	/// </summary>
	/// <typeparam name="T">Type of the list</typeparam>
	public class ObservableList<T> : IList<T>
	{
		#region----- VARIABLES -----

		protected readonly List<T> _list = new();
		public event Action ListChanged;

		#endregion

		#region----- CUSTOM BEHAVIOURS-----

		protected void Notify()
		{
			UnityEngine.Debug.Log("Observable List Notify");
			ListChanged?.Invoke();
		}

		public ObservableList() { }

		public ObservableList(List<T> list)
		{
			_list = list;
			Notify();
		}

		#endregion

		#region------ IList<T> -----

		public virtual T this[int index] 
		{ 
			get => _list[index]; 
			set
			{
				_list[index] = value;
				Notify();
			}
		}

		public int Count => _list.Count;

		public bool IsReadOnly => false;

		public virtual void Add(T item)
		{
			UnityEngine.Debug.Log($"Adding item: {item.ToString()}");
			_list.Add(item);
			Notify();
		}

		public virtual void Clear()
		{
			_list.Clear();
			Notify();
		}

		public bool Contains(T item) => _list.Contains(item);

		public virtual void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

		public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

		public int IndexOf(T item) => _list.IndexOf(item);

		public virtual void Insert(int index, T item)
		{
			_list.Insert(index, item);
			Notify();
		}

		public virtual bool Remove(T item)
		{
			var result = _list.Remove(item);
			if(result)
				Notify();
			return result;
		}

		public virtual void RemoveAt(int index)
		{
			_list.RemoveAt(index);
			Notify();
		}

		IEnumerator IEnumerable.GetEnumerator() => _list?.GetEnumerator();

		#endregion
	}
}
