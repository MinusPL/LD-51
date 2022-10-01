using Godot;
using System;

public class Trap : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public bool isActive = false;
    public bool armed = true;
    public int state = 0;

    public Player player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public override void _PhysicsProcess(float delta)
    {

    }

    public void _on_Trigger_area_entered(Area2D area)
    {
        if (area.GetParent() != null && area.GetParent().IsInGroup("player"))
        {
            //activate trap!
            if (armed)
            {
                isActive = true;
            }   player = (Player)area.GetParent();
            KillPlayer();
        }
    }

    public virtual void KillPlayer()
    {
        if(armed)
            player.Damage(float.MaxValue);
    }

    public void OnActivate(bool flag)
    {
        isActive = flag;
    }
}
