using Godot;
using System;

public class Block : Node2D
{
    [Signal]
    public delegate void Activated(bool value);
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    CollisionShape2D col;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        col = (CollisionShape2D)GetNode("collider/shape");   
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    public void OnActivated(bool flag)
    {
        Visible = flag;
        col.Disabled = !flag;
    }
}
