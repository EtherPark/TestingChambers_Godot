using Godot;
using System;

public partial class IdleState : State
{
    [Signal] public delegate void TransitionEventHandler(string stateName);

    private Vector2 direction;
    private Timer restTime;
    private Enemy enemy;

    public override void ready()
    {
        restTime = GetNode<Timer>("Rest Time");
        restTime.OneShot = true;
        enemy = fsm.GetParent<Enemy>();
        direction = new Vector2 (0,0);
    }

    public override void enter()
    {
        enemy.setSpeed(0f);
        enemy.setDirection(direction);
        restTime.Start();
    }

    public override void exit()
    {
        base.exit();
    }

    public override void update(float delta)
    {
        if(restTime.IsStopped())
        {
            fsm.transitionTo("Wander");
        }
    }

    public override void physicsUpdate(float delta)
    {
        base.physicsUpdate(delta);
    }

}
