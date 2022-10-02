using Godot;
using System;

public enum ExtensionLength
{
    tile,
    tile_half,
    two_tiles,
}

public class TrapWallCrush : Trap
{
    [Export]
    ExtensionLength type = ExtensionLength.tile;
    [Export]
    float wallSpeed = 200f;

    float moveLength = 0f;

    Node2D wall;

    Vector2 prevPos;

    bool shouldKill = false;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        wall = (Node2D)GetNode("wall");

        switch(type)
        {
            case ExtensionLength.tile:
                moveLength = 64f;
                break;
            case ExtensionLength.tile_half:
                moveLength = 96f;
                break;
            case ExtensionLength.two_tiles:
                moveLength = 128f;
                break;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        switch (state)
        {
            //opened
            case 0:
                if (isActive)
                {
                    prevPos = wall.Position;
                    shouldKill = true;
                    state = 1;
                }
                break;
            //close
            case 1:
                {
                    wall.Position += wallSpeed * new Vector2(0f,1f) * delta;
                    if (wall.Position.DistanceTo(prevPos) >= moveLength)
                    {
                        prevPos += new Vector2(0f, moveLength);
                        wall.Position = prevPos;
                        shouldKill = false;
                        state = 2;
                    }
                    armed = false;
                }
                break;
            //closed
            case 2:
                if (!isActive)
                {
                    prevPos = wall.Position;
                    shouldKill = true;
                    state = 3;
                }
                break;
            //open
            case 3:
                wall.Position -= wallSpeed * new Vector2(0f, 1f) * delta;
                if (wall.Position.DistanceTo(prevPos) >= moveLength)
                {
                    prevPos -= new Vector2(0f, moveLength);
                    wall.Position = prevPos;
                    shouldKill = false;
                    state = 2;
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
        if(other.IsInGroup("player") && shouldKill)
        {
            ((Player)other).Damage(float.MaxValue);
        }
    }
}
