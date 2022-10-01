using Godot;
using System;

public class TrapWallCrush : Trap
{
    [Export]
    float wallsSpeed = 50f;
    [Export]
    Vector2 wall1_startPos;
    [Export]
    Vector2 wall1_endPos;
    [Export]
    Vector2 wall2_startPos;
    [Export]
    Vector2 wall2_endPos;

    Node2D wall1;
    Node2D wall2;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        wall1 = (Node2D)GetNode("wall1");
        wall2 = (Node2D)GetNode("wall2");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        switch (state)
        {
            //opened
            case 0:
                if (isActive) state = 1;
                break;
            //close
            case 1:
                {
                    //wall1
                    Vector2 wall1Direction = wall1_startPos.DirectionTo(wall1_endPos);
                    Vector2 wall2Direction = wall2_startPos.DirectionTo(wall2_endPos);

                    wall1.Position += wall1Direction * wallsSpeed * delta;
                    wall2.Position += wall2Direction * wallsSpeed * delta;

                    if (wall1.Position.DistanceTo(wall1_endPos) < 1f)
                    {
                        wall1.Position = wall1_endPos;
                        wall2.Position = wall2_endPos;
                        state = 2;
                    }
                    armed = false;
                }
                break;
            //closed
            case 2:
                if (!isActive) state = 3;
                break;
            //open
            case 3:
                {
                    Vector2 wall1Direction = wall1_endPos.DirectionTo(wall1_startPos);
                    Vector2 wall2Direction = wall2_endPos.DirectionTo(wall2_startPos);

                    wall1.Position += wall1Direction * wallsSpeed * delta;
                    wall2.Position += wall2Direction * wallsSpeed * delta;

                    if (wall1.Position.DistanceTo(wall1_startPos) < 1f)
                    {
                        wall1.Position = wall1_startPos;
                        wall2.Position = wall2_startPos;
                        state = 0;
                    }
                }
                armed = true;
                break;
        }
    }

    public override void KillPlayer()
    {

    }

    public void OnWallAreaBodyEntered(Node other)
    {
        if(other.IsInGroup("player"))
        {
            ((Player)other).Damage(float.MaxValue);
        }
    }
}
