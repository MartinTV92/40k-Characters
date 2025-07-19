using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JollyRoger.Collections
{
	public class ObservableValue<T> : NotifyPropertyChangedWrapper
	{
		private T _value;
		public T Value { get => _value; set => SetProperty(ref _value, value); }
		public ObservableValue() { }
		public ObservableValue(T value) =>	_value = value;
		public override string ToString() => _value?.ToString() ?? "null";
		public static implicit operator T(ObservableValue<T> wrapper) => wrapper._value;
		public static implicit operator ObservableValue<T>(T value) => new ObservableValue<T>(value);
	}
}