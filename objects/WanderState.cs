using Godot;
using System;
using System.ComponentModel;

public partial class WanderState : State
{
    private const int LEFT_DEGREES = 180;

    [Signal] public delegate void TransitionEventHandler(string stateName);

    private Random rand = new Random();
    private int coinFlip;
    private Vector2 direction;
    private Timer walkTime;
    private Enemy enemy;

    public override void ready()
    {
        walkTime = GetNode<Timer>("Walk Time");
        walkTime.OneShot = true;
        enemy = fsm.GetParent<Enemy>();
        coinFlip = rand.Next(2) == 0 ? -1 : 1;
        direction = new Vector2 (0, 0);
        //direction = enemy.Position.Rotated(Mathf.DegToRad(coinFlip * LEFT_DEGREES));
    }

    public override void enter()
    {
        direction.X = coinFlip;
        coinFlip = rand.Next(2) == 0 ? -1 : 1;
        enemy.setSpeed(50f);
        enemy.setDirection(direction);
        enemy.updateVelocity();
        walkTime.Start();
    }

    public override void exit()
    {
        GD.Print("fine i think");
    }

    public override void update(float delta)
    {
        if(walkTime.IsStopped())
        {
            fsm.transitionTo("Idle");
        }
    }

    public override void physicsUpdate(float delta)
    {
        enemy.MoveAndSlide();
        //coinFlip = rand.Next() % 2;
        //direction = enemy.GlobalPosition.Rotated(Mathf.DegToRad(coinFlip * LEFT_DEGREES));
    }

}
