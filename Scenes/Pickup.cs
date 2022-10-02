using Godot;
using System;

public enum PickupType
{
    sword,
    lamp
}

public class Pickup : Node2D
{
    [Export]
    PickupType type = PickupType.sword;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void OnTriggerEntered(Node other)
    {
        if(other.IsInGroup("player"))
        {
            ((Player)other).PickUp(type);
            QueueFree();
        }
    }
}
