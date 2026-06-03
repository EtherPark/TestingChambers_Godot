using Godot;
using System;

public partial class Blaster : Node2D
{
	//--- object properties ---// 
	[Export]
	public int shotCount = 1; //almost exclusive to spreadshot attributes
	private const int shotCount_MIN = 1;
	private const int shotCount_MAX = 15;
	[Export]
	public float Damage = 1f;
	private const float Damage_MIN = 0.5f;
	private const float Damage_MAX = 999f;
	[Export]
	public float Spread = 0f; // the number represents degrees => Mathf.DegToRad(...) when needed
	private const float Spread_MIN = 0f;
	private const float Spread_MAX = 100f;
	[Export]
	public float Rof = 0.2f; //Rate of fire, adjustments should be multiplicative
	private const float Rof_MAX = 0.01f;

	private float fireCooldown = 0f; //timer between shoot() func calls
	private bool isSpreadshot = false; //"am i a shotgun?"
	private bool isFiring = false;

	//--- packed scenes ---//
	[Export]
	public PackedScene ProjectileScn { get; private set; }
	[Export]
	public PackedScene PowerShotgunScn { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PowerShotgun shotgun = PowerShotgunScn.Instantiate<PowerShotgun>();
		shotgun.GlobalPosition += new Vector2(-300,0);
		GetTree().CurrentScene.CallDeferred(Node.MethodName.AddChild, shotgun);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(fireCooldown > 0)
		{
			fireCooldown -= (float)delta;
		}

		if(isFiring && fireCooldown <= 0)
		{
			shoot();
		}
	}

    public override void _Input(InputEvent @event)
	{
		if(@event.IsActionPressed("click"))
		{
			isFiring = true;
		}
		if(@event.IsActionReleased("click"))
		{
			isFiring = false;
		}
	}

	private void shoot()
	{
		Projectile projectile = ProjectileScn.Instantiate<Projectile>();
		projectile.setDamage(Damage);
		Vector2 projectileDirect = GlobalPosition.DirectionTo(GetGlobalMousePosition());

		if(!isSpreadshot)
		{
			projectile.GlobalPosition = GlobalPosition;
			projectile.setDirection(projectileDirect);

			GetTree().CurrentScene.AddChild(projectile);
		}
		else if(isSpreadshot)
		{
			//this should set projectiles in a spread according to the current Spread value. imagine starting from right to left
			//when spreading upward.
			Vector2 spreadDirect;
			float halfAngle = Spread / 2f;
			for(int i = 0; i < shotCount; i++)
			{
				// makes 'i' into a percent value for 'Mathf.Lerp(start, end, i)'
				float pcnt = (float)i / (shotCount - 1);

				float interpAngle = Mathf.Lerp(-halfAngle, halfAngle, pcnt);

				spreadDirect = projectileDirect.Rotated(Mathf.DegToRad(interpAngle));

				shoot(spreadDirect);
			}
		}
		fireCooldown = Rof;
	}

// Helper func for shoot when the blaster becomes a spreadshot, might be an inefficient way to do it but whatever for now
	private void shoot(Vector2 direction)
	{
		Projectile projectile = ProjectileScn.Instantiate<Projectile>();
		projectile.setDamage(Damage);

		projectile.GlobalPosition = GlobalPosition;
		projectile.setDirection(direction);

		GetTree().CurrentScene.AddChild(projectile);
	}

// called when the ApplyShotgun signal is recieved
	public void OnPickupShotgun(float newDanage, float newRof, int addedShots, float newSpread)
	{
		Damage += newDanage;
		if(Damage > Damage_MAX)
			Damage = Damage_MAX;
		
		Rof *= newRof;
		if(Rof < Rof_MAX)
			Rof = Rof_MAX;

		shotCount += addedShots;
		if(shotCount > shotCount_MAX)
			shotCount = shotCount_MAX;

		Spread += newSpread;
		if(Spread > Spread_MAX)
			Spread = Spread_MAX;

		if(shotCount > 1)
		{
			isSpreadshot = true;
		}
	}
}
