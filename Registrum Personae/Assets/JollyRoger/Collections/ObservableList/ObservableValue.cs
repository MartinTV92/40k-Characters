using System;

namespace JollyRoger.Collections
{
	public class ObservableValue<T> : NotifyPropertyChangedWrapper
	{
		private T _value;

		// Property Changes to go through the definabhle Get and Set 
		public T Value 
		{ 
			get => Get(); 
			set => SetProperty(ref _value, Set(value)); 
		}
		

		public ObservableValue(Func<T> get = null, Func<T,T> set = null) 
		{ 
			Get = get ?? GetValue;
			Set = set ?? SetValue;
		}

		public ObservableValue(T value, Func<T> get = null, Func<T, T> set = null) : this(get, set)
		{
			_value = value;
		}

		public override string ToString() => _value?.ToString() ?? "null";
		public static implicit operator T(ObservableValue<T> wrapper) => wrapper._value;
		public static implicit operator ObservableValue<T>(T value) => new ObservableValue<T>(value);
		
		// New Get and Set stuff
		private Func<T> Get;
		private Func<T,T> Set;

		private T GetValue() => _value;
		private T SetValue(T value) => value;
	}
}

