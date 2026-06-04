using Godot;
using System;

public partial class PowerMachinegun : Area2D
{
	[Signal]
	public delegate void ApplyMachinegunEventHandler(float newDamage, float newRof, int adjustShots);

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
			ApplyMachinegun += gun.OnPickupMachinegun;
			EmitSignal(SignalName.ApplyMachinegun, -0.25f, 0.75f, -1);
			QueueFree();
		}
	}
}
