using Godot;
using System;

public class TrapSpikes : Trap
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    Node2D spikes;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        spikes = (Node2D)GetNode("spikes");
        spikes.Visible = false;
        isActive = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        spikes.Visible = isActive;
    }

}
