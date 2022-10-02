using Godot;
using System;

public class Gate : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    AnimatedSprite sprite;

    //0 - open, 1 - closing, 2 - closed, 3 - opening
    int state = 0;

    [Export]
    bool activated = false;
    [Export]
    bool flipped = false;

    bool animationPlaying = false;

    CollisionShape2D gateCollider;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        sprite = (AnimatedSprite)GetNode("gateSprite");
        gateCollider = (CollisionShape2D)GetNode("GateCollision/CollisionShape2D");
        if(activated)
        {
            state = 2;
            sprite.Stop();
            sprite.Animation = "open";
            gateCollider.Disabled = true;
        }
        else
        {
            state = 0;
            sprite.Stop();
            sprite.Animation = "closed";
            gateCollider.Disabled = false;
        }
        sprite.FlipH = flipped;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        switch(state)
        {
            case 0:
                if (activated)
                {
                    sprite.SpeedScale = 1f;
                    sprite.Play("moving", true);
                    animationPlaying = true;
                    state = 1;
                }
                break;
            case 1:
                if (!animationPlaying)
                {
                    sprite.Stop();
                    sprite.Animation = "open";
                    state = 2;
                    gateCollider.Disabled = true;
                }
                break;
            case 2:
                if (!activated)
                {
                    sprite.SpeedScale = 0.5f;
                    gateCollider.Disabled = false;
                    sprite.Play("moving", false);
                    animationPlaying = true;
                    state = 3;
                }
                break;
            case 3:
                if (!animationPlaying)
                {
                    sprite.Stop();
                    sprite.Animation = "closed";
                    state = 0;
                }
                break;
        }
    }

    public void OnAnimationFinished()
    {
        animationPlaying = false;
    }

    public void OnActivated(bool flag)
    {
        activated = flag;
    }
}
