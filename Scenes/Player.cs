using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export]
    public float speed;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public float maxHealth = 50f;
    [Export]
    public float health;


    Vector2 dir;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        health = maxHealth;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        dir = new Vector2();
        if (Input.IsActionPressed("move_up")) dir.y -= 1; 
        if (Input.IsActionPressed("move_down")) dir.y += 1; 
        if (Input.IsActionPressed("move_right")) dir.x += 1; 
        if (Input.IsActionPressed("move_left")) dir.x -= 1;

        MoveAndSlide(dir.Normalized() * speed);
    }
}
