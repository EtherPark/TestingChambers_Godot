using Godot;
using System;

public partial class CharacterBody2d : CharacterBody2D
{
	[Export]
	public Node2D Blaster { get; private set; }
	public float Speed = 5f;

	[Export]
	public PackedScene ProjectileScn { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 inputDirect = Input.GetVector("left", "right", "up", "down");
		Position += inputDirect * Speed;

		MoveAndSlide();
	}

// This calls when you press LMB
// it creates the projectile object within the "world" tree
    public override void _Input(InputEvent @event)
	{
		if(@event.IsActionPressed("click"))
		{
			Projectile projectile = ProjectileScn.Instantiate<Projectile>();
			Vector2 projectileDirect = GlobalPosition.DirectionTo(GetGlobalMousePosition());
			projectile.Position = this.Position;
			projectile.setDirection(projectileDirect);
			GetTree().CurrentScene.AddChild(projectile);
		}
	}

}
