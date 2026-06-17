using Godot;
using System;

public partial class State : Node
{
	/*
	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
	}
	*/
	public StateMachine fsm;

	public virtual void enter() {}
	public virtual void exit() {}

	public virtual void ready() {}
	public virtual void update(float delta) {}
	public virtual void physicsUpdate(float delta) {}
	public virtual void handleInput(InputEvent @event) {}
}
