using Godot;
using System;

public class SpawnPoint : Node2D
{
    [Export]
    public Color disabledColor = Colors.Tan;
    [Export]
    public Color enabledColor = Colors.Limegreen;
    [Export]
    public bool enabled = false;

    public Vector2 spawnPosition;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    bool inRange = false;

    public override void _Ready()
    {
        Modulate = enabled ? enabledColor : disabledColor;
        spawnPosition = ((Node2D)GetNode("SpawnPosition")).GlobalPosition;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("action") && inRange)
        {
            ((LevelManager)GetNode("/root/Game/LevelManager")).ChangeActiveSpawnPoint(this);
        }

    }

    public void OnTriggerAreaEntered(Node other)
    {
        if(other.IsInGroup("player"))
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

    public void SetEnabled(bool isEnabled)
    {
        enabled = isEnabled;
        Modulate = enabled ? enabledColor : disabledColor;
    }
}
