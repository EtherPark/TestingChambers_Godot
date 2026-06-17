using Godot;
using System;

//I never bothered with a diff name because i didnt think i'd keep evloving the testing project lmao
//player just called character body node but with a lowercase d (teehee)
public partial class CharacterBody2d : CharacterBody2D
{
	[Export] private float Speed = 750f;
	public Node2D Blaster { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 inputDirect = Input.GetVector("left", "right", "up", "down");
		Velocity = inputDirect * Speed;

		MoveAndSlide();

		
	}
}
