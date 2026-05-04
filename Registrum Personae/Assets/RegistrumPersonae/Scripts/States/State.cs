using System;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace RegistrumPersonae.FinateStateMachine
{
	[Serializable]
	public abstract class State <T> where T : Enum
	{
	    [HorizontalGroup("Events")] public UnityEvent OnEnter;
	    [HorizontalGroup("Events")] public UnityEvent OnExit;

	    protected StateMachine<T> stateMachine;

	    public virtual void Setup(StateMachine<T> sm) => stateMachine = sm;
	    public virtual void Enter() => OnEnter?.Invoke();
	    public virtual void Update() { }
	    public virtual void Exit() => OnExit?.Invoke();
	}
}

