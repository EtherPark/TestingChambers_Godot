using Godot;
using System;

public partial class Vision : Area2D
{
	[Signal] public delegate void EngagePlayerEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += onPlayerSpotted;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void onPlayerSpotted(Node body)
	{
		if(body is CharacterBody2d player)
		{
			GD.Print("see you");
		}
	}
}
