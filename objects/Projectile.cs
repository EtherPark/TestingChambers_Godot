using Godot;
using System;

public partial class Projectile : Area2D
{
	[Export]
	public float Speed = 30f;
	public float Lifetime = 1f;
	private Vector2 Direction;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetTree().CreateTimer(Lifetime).Timeout += QueueFree;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.Position += Direction * Speed * (float)delta;
	}

	public void setDirection(Vector2 direction)
	{
		Direction = direction;
	}
}
