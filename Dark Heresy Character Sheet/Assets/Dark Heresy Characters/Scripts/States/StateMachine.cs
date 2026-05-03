using System;
using Sirenix.OdinInspector;

namespace RegistrumPersonae.FinateStateMachine
{
	[Serializable]
	public class StateMachine<T> where T : Enum
	{
		[Serializable]
		public class StateDictionary : UnitySerializedDictionary<T, State<T>> { }
		[ReadOnly, ShowInInspector] public T currentState { get; private set; }
		public StateDictionary states = new();
		public Action<T> OnStateUpdate;
		public Action<T,T> OnStateChanged;

		/// <summary>
		/// Register a state to the state machine. Disallows duplicate state keys.
		/// </summary>
		/// <param name="stateEnum">The key the state is tied to.</param>
		/// <param name="state">The state object.</param>
		public void RegisterState(T stateEnum, State<T> state)
		{
			if(states.ContainsKey(stateEnum))
				return;

			states.Add(stateEnum, state);
			states[stateEnum].Setup(this);
		}

		/// <summary>
		/// Runs the statemachine, executing the current state's actions and other tasks.
		/// </summary>
		public void Run()
		{
			states[currentState].Update();
			OnStateUpdate?.Invoke(currentState);
		}

		/// <summary>
		/// Changes the state machine's current state if it is valid 
		/// and invokes event to alert object of state change (last, current)
		/// </summary>
		/// <param name="state">The desired state to change to. </param>
		public void ChangeState(T state)
		{
			if (states.ContainsKey(state) == false || currentState.Equals(state))
				return;

			if (states.ContainsKey(currentState))
				states[currentState].Exit();

			var lastState = currentState;
			currentState = state;
			states[currentState].Enter();
			OnStateChanged?.Invoke(lastState, currentState);
		}
	}
}