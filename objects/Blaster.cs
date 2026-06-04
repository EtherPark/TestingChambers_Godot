using Godot;
using System;

public partial class Blaster : Node2D
{
	//--- object properties ---//
	[Export]
	private float flightSpeed = 2000f;
	private float flight_MAX = 5000f;
	[Export]
	private int shotCount = 1;
	private const int shotCount_MIN = 1;
	private const int shotCount_MAX = 15;
	[Export]
	private float Damage = 1f;
	private const float Damage_MIN = 0.5f;
	private const float Damage_MAX = 999f;
	[Export]
	private float Spread = 0f; // the number represents degrees => Mathf.DegToRad(...) when needed
	private const float Spread_MIN = 0f;
	private const float Spread_MAX = 100f;
	[Export]
	private float Rof = 0.2f; //Rate of fire, adjustments should be multiplicative
	private const float Rof_MAX = 0.01f;

	private float fireCooldown = 0f; //timer between shoot() func calls
	private bool isSpreadshot = false; //"am i a shotgun?"
	private bool isFiring = false;

	//--- packed scenes ---//
	[Export]
	public PackedScene ProjectileScn { get; private set; }
	/*
	[Export]
	public PackedScene PowerShotgunScn { get; private set; }
	*/

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		/*
		PowerShotgun shotgun = PowerShotgunScn.Instantiate<PowerShotgun>();
		shotgun.GlobalPosition += new Vector2(-300,0);
		GetTree().CurrentScene.CallDeferred(Node.MethodName.AddChild, shotgun);
		*/
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
		Vector2 projectileDirect = GlobalPosition.DirectionTo(GetGlobalMousePosition());

		if(!isSpreadshot)
		{
			Projectile projectile = ProjectileScn.Instantiate<Projectile>();
			projectile.setDamage(Damage);
			projectile.setSpeed(flightSpeed);

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
		projectile.setSpeed(flightSpeed);

		projectile.GlobalPosition = GlobalPosition;
		projectile.setDirection(direction);

		GetTree().CurrentScene.AddChild(projectile);
	}

// called when the ApplyShotgun signal is recieved
	public void OnPickupShotgun(float newDanage, float newRof, int addedShots, float newSpread)
	{
		isSpreadshot = true;

		Damage += newDanage;
		if(Damage > Damage_MAX)
			Damage = Damage_MAX;
		
		Rof *= newRof;

		shotCount += addedShots;
		if(shotCount > shotCount_MAX)
			shotCount = shotCount_MAX;

		Spread += newSpread;
		if(Spread > Spread_MAX)
			Spread = Spread_MAX;
	}

	public void OnPickupMachinegun(float newDamage, float newRof, int adjustShots)
	{
		Damage += newDamage;
		if(Damage < Damage_MIN)
			Damage = Damage_MIN;

		Rof *= newRof;
		if(Rof < Rof_MAX)
			Rof = Rof_MAX;

		shotCount += adjustShots;
		if(shotCount <= shotCount_MIN)
		{
			shotCount = shotCount_MIN;
			isSpreadshot = false;
		}

	}

	public void OnPickupSniper(float newDamage, float newRof, float newSpread, float newSpeed, int adjustShots)
	{
		Damage += newDamage;
		if(Damage > Damage_MAX)
			Damage = Damage_MAX;

		Rof *= newRof;
		
		Spread += newSpread;
		if(Spread < Spread_MIN)
			Spread = Spread_MIN;
		
		flightSpeed += newSpeed;
		if(flightSpeed > flight_MAX)
			flightSpeed = flight_MAX;

		shotCount += adjustShots;
		if(shotCount <= shotCount_MIN)
		{
			shotCount = shotCount_MIN;
			isSpreadshot = false;
		}
	}
}
