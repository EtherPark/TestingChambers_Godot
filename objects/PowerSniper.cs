using Godot;
using System;

public partial class PowerSniper : Area2D
{
	[Signal]
	public delegate void ApplySniperEventHandler(float newDamage, float newRof, float newSpread, float newSpeed, int adjustShots);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnBodyEntered(Node2D body)
	{
		if(body is CharacterBody2d player)
		{
			Blaster gun = player.GetNode<Blaster>("Blaster");
			ApplySniper += gun.OnPickupSniper;
			EmitSignal(SignalName.ApplySniper, 3f, 2.1f, -25f, 500f, -2);
			QueueFree();
		}
	}
}
