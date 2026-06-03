using Godot;
using System;
using System.ComponentModel;

public partial class PowerShotgun : Area2D
{
	[Signal]
	public delegate void ApplyShotgunEventHandler(float newDamage, float newRof, int addedShots, float newSpread);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnPlayerEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnPlayerEntered(Node2D body)
	{
		if(body is CharacterBody2d player)
		{
			Blaster gun = player.GetNode<Blaster>("Blaster");
			ApplyShotgun += gun.OnPickupShotgun;
			EmitSignal(SignalName.ApplyShotgun, 0.5f, 2f, 3, 30f);
			QueueFree();
		}
	}
}
