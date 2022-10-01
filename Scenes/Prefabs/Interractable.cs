using Godot;
using System;

public enum InterractableType
{
    ITYPE_SWITCH,
    ITYPE_TIMED
}

public class Interractable : Node2D
{
    [Export]
    InterractableType type = InterractableType.ITYPE_SWITCH;

    [Export]
    float activateTime = 1f;
    [Export]
    float fastRotation = 90f;
    [Export]
    float slowRotation = 30f;
    [Export]
    float resetTime = 3f;

    [Signal]
    public delegate void Activated(bool value);

    int state = 0;
    [Export]
    bool activated = true;
    bool inRange = false;
    [Export]
    bool resetable = true;

    float stateTimer = 0f;

    bool originalActivationState;


    Node2D head;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        head = (Node2D)GetNode("Head");
        EmitSignal("Activated", activated);
        if (type == InterractableType.ITYPE_TIMED)
            state = activated ? 2 : 0;
        originalActivationState = activated;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        //TODO ADD VISUALS
        switch(type)
        {
            case InterractableType.ITYPE_SWITCH:
                    if (Input.IsActionJustPressed("action") && inRange)
                    {
                        activated = !activated;
                        EmitSignal("Activated", activated);
                    }
                    break;
            case InterractableType.ITYPE_TIMED:
                switch(state)
                {
                    //rest, not active
                    case 0:
                        activated = false;
                        if (Input.IsActionJustPressed("action") && inRange)
                        {
                            state = 1;
                            stateTimer = activateTime;
                            EmitSignal("Activated", !activated);
                        }
                        break;
                    //fast spin
                    case 1:
                        activated = true;
                        stateTimer -= delta;
                        head.Rotate(Mathf.Deg2Rad(fastRotation)*delta);
                        
                        if (stateTimer <= 0f)
                            state = 2;

                        break;
                    //rest, active
                    case 2:
                        activated = true;
                        if (Input.IsActionJustPressed("action") && inRange)
                        {
                            state = 3;
                            stateTimer = resetTime;
                        }
                        break;
                    //slow spin
                    case 3:
                        activated = true;
                        head.Rotate(Mathf.Deg2Rad(-slowRotation)*delta);
                        stateTimer -= delta;
                        if (stateTimer <= 0f)
                        {
                            state = 0;
                            EmitSignal("Activated", !activated);
                        }
                        break;
                }
                break;
        }
    }

    public void OnTriggerAreaEntered(Node other)
    {
        if (other.IsInGroup("player"))
        {
            inRange = true;
            GetTree().CallGroup("player", "ShowInterractionBubble", true);
        }
    }

    public void OnTriggerAreaExited(Node other)
    {
        if (other.IsInGroup("player"))
        {
            inRange = false;
            GetTree().CallGroup("player", "ShowInterractionBubble", false);
        }
    }

    public void Reset()
    {
        if(resetable)
        {
            if (type == InterractableType.ITYPE_TIMED)
                state = originalActivationState ? 2 : 0;
            else
                state = 0;
            activated = originalActivationState;
        }
    }
}
