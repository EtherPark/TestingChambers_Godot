using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine : Node
{
	[Export] public NodePath initialState;

	private Dictionary<string, State> _states;
	private State currentState;

	public override void _Ready()
	{
		_states = new Dictionary<string, State>();
		foreach (Node node in GetChildren())
		{
			if(node is State s)
			{
				_states[node.Name] = s;
				s.fsm = this;
				s.ready();
				s.exit();
			}
		}
		currentState = GetNode<State>(initialState);
		currentState.enter();
	}

	public override void _Process(double delta)
	{
		currentState.update((float)delta);
	}

    public override void _PhysicsProcess(double delta)
    {
        currentState.physicsUpdate((float)delta);  
    }

    public override void _UnhandledInput(InputEvent @event)
	{
		currentState.handleInput(@event);
	}

	public void transitionTo(string key)
	{
		//check: does state exist or are we still in the same state?
		if(!_states.ContainsKey(key) || currentState == _states[key])
			return;
		
		currentState.exit();
		currentState = _states[key];
		currentState.enter();
	}


}
