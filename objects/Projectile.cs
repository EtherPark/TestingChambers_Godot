using Godot;
using System;

public partial class Projectile : CharacterBody2D
{
	[Export]
	public float Flight = 2000f;
	public float Lifetime = 1f;
	public float Damage = 1f;
	private Vector2 Direction;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetTree().CreateTimer(Lifetime).Timeout += QueueFree;
	}

	public override void _Process(double delta)
	{
		Vector2 Vel = Direction * Flight;
		KinematicCollision2D collision = MoveAndCollide(Vel * (float)delta);
		if(collision != null)
		{
			Node collider = collision.GetCollider() as Node;
			//GD.Print("You hit ", collider?.Name);

			if(collision.GetCollider() is DestructWall walls)
			{
				Vector2I HitPosition = walls.LocalToMap(collision.GetPosition() - collision.GetNormal());
				walls.DamageToWall(HitPosition, Damage);
				//GD.Print(HitPosition);
			}
			QueueFree();
		}
		
	}

	public void setDirection(Vector2 direction)
	{
		Direction = direction;
	}
}
