using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	//private Random rand = new Random();
	private Vector2 direction;
	private float speed = 300f;
	private float Rof = 0.3f;
	private float fireCooldown = 0f;
	private StateMachine fsmConnect;
	
	public override void _Ready()
	{
		fsmConnect = GetNode<StateMachine>("State Machine");
	}

	public override void _Process(double delta)
	{
		
	}

	public void setSpeed(float newSpeed)
	{
		speed = newSpeed;
	}

	public void setDirection(Vector2 newDirection)
	{
		direction = newDirection;
	}

	public void updateVelocity()
	{
		Velocity = speed * direction;
	}

	public void toShootingState()
	{
		
	}
	
}
