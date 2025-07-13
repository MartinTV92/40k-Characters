using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace JollyRoger.Collections
{
    /// <summary>
    /// A deeply obserable list that will notify on the same changes observable list
    /// as well as when changes in elements happen. Requires more setup.
    /// </summary>
    public class DeepObservableList<T> : ObservableList<T> where T : INotifyPropertyChanged
    {
		#region----- VARIABLES -----

        public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region----- OVERRIDES -----

		public override T this[int index]
		{
			get => base[index];
			set
			{
				UnsubscribeFromItem(base[index]);
				base[index] = value;
				SubscribeToItem(value);
			}
		}

		public override void Add(T item)
		{
			base.Add(item);
			SubscribeToItem(item);
		}

		public override bool Remove(T item)
		{
			var result = base.Remove(item);
			if(result)
				UnsubscribeFromItem(item);

			return result;
		}

		public override void Clear()
		{
			foreach(var item in _list)
				UnsubscribeFromItem(item);

			base.Clear();
		}

		public override void Insert(int index, T item)
		{
			base.Insert(index, item);
			SubscribeToItem(item);
		}

		public override void RemoveAt(int index)
		{
			UnsubscribeFromItem(_list[index]);
			base.RemoveAt(index);
		}

		#endregion

		#region----- CUSTOM METHODS -----

		public DeepObservableList() => ListChanged += ResubscribeAll;

		public DeepObservableList(List<T> list) : base(list)
		{
			ListChanged += ResubscribeAll;
		}

		private void ResubscribeAll()
		{
			foreach(var item in _list)
				SubscribeToItem(item);
		}

		private void SubscribeToItem(T item)
		{
			UnityEngine.Debug.Log($"Subscribing {item.ToString()}");
			item.PropertyChanged -= OnItemChanged;
			item.PropertyChanged += OnItemChanged;
		}

		private void UnsubscribeFromItem(T item) => item.PropertyChanged -= OnItemChanged;

		private void OnItemChanged(object sender, PropertyChangedEventArgs e) => Notify();

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected bool SetProperty(ref T field, T value, string propertyName)
        {
            if(EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
			OnPropertyChanged(propertyName);
			return true;
        }

		#endregion

	}
}
