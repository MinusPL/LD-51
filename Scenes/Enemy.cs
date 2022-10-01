using Godot;
using System;

public class Enemy : KinematicBody2D
{
    [Export]
    public float moveSpeed = 30f;
    [Export]
    public float visibilityRange = 100f;

    [Export]
    public float AttackCooldown = 5f;
    [Export]
    public float AttackDamage = 20f;

    float cooldownTimer = 0f;

    Navigation2D nav;
    
    
    Player target = null;

    Vector2[] path = { };

    int pathNode = 0;

    bool playerInAttackRange = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        nav = (Navigation2D)GetNode("/root/Game/Level/Map");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(playerInAttackRange && cooldownTimer <= 0f)
        {
            Attack();
        }

        if(cooldownTimer > 0f)
        {
            cooldownTimer -= delta;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if(path.Length > 0 && pathNode < path.Length)
        {
            if (GlobalPosition.DistanceTo(path[pathNode]) < 5f)
            {
                pathNode++;
            }
            else
            {
                Vector2 dir = GlobalPosition.DirectionTo(path[pathNode]);
                MoveAndSlide(dir * moveSpeed);
            }
        }

        
        if (target != null && GlobalPosition.DistanceTo(target.GlobalPosition) <= visibilityRange)
            LookAt(target.GlobalPosition);
        else if(path.Length > 0 && pathNode < path.Length)
            //Smooth that shit, later
            LookAt(path[pathNode]);
    }

    public void _on_Aggro_body_entered(Node other)
    {
        if(other.IsInGroup("player"))
        {
            target = (Player)other;
        }
    }

    public void _on_hurt_body_entered(Node other)
    {
        if (other.IsInGroup("player"))
        {
            playerInAttackRange = true;
        }
    }

    public void _on_hurt_body_exited(Node other)
    {
        if (other.IsInGroup("player"))
        {
            playerInAttackRange = false;
        }
    }

    public void GetPathToTarget()
    {
        if (target != null)
        {
            path = nav.GetSimplePath(GlobalPosition, target.GlobalPosition, false);
            pathNode = 0;
        }
    }

    public virtual void Attack()
    {
        target.Damage(AttackDamage);
        cooldownTimer = AttackCooldown;
    }
}
