using Godot;
using System;

public class TrapDarts : Trap
{
    Node2D arrowSpawn;
    Node2D arrowTarget;
    PackedScene arrows;

    [Export]
    bool loop = false;
    [Export]
    float rechargeTime = 0.5f;
    float rechargeTimer = 0f;

    bool fire = false;

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        arrowSpawn = (Node2D)GetNode("Spawn");
        arrowTarget = (Node2D)GetNode("Target");
        arrows = ResourceLoader.Load("res://Scenes/Prefabs/Projectiles/Arrow.tscn") as PackedScene;
        if (loop)
            rechargeTimer = rechargeTime;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        if(isActive && !loop)
        {
            fire = true;
            isActive = false;
        }

        if(loop)
        {
            if (rechargeTimer > 0f)
                rechargeTimer -= delta;
            else
            {
                rechargeTimer = rechargeTime;
                fire = true;
            }
        }

        if(fire)
        {
            RigidBody2D arrowsObject = arrows.Instance() as RigidBody2D;
            AddChild(arrowsObject);
            arrowsObject.Position = arrowSpawn.Position;
            arrowsObject.ApplyCentralImpulse(arrowSpawn.Position.DirectionTo(arrowTarget.Position) * 1000f);
            fire = false;
        }
    }



    public override void KillPlayer()
    {
    }
}
