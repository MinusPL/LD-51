using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export]
    public float speed = 100f;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public float maxHealth = 50f;
    [Export]
    public float health;

    bool alive = true;

    [Export]
    float maxSprintTime = 5f;
    [Export]
    float sprintSpeed= 200f;

    float remainingSprintTime = 0f;

    bool hasWeapon;

    Vector2 dir;

    AnimatedSprite playerSprite;

    //Wiem wiem, dziwne, ale nie mam lepszego pomyslu na teraz...
    // No i w poziomie raczej nie beda na siebie interakcje nachodzic.
    int interactablesInRange = 0;
    Node2D speechBubble;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        health = maxHealth;
        speechBubble = (Node2D)GetNode("SpeechBubble");
        speechBubble.Visible = false;
        remainingSprintTime = maxSprintTime;
        playerSprite = (AnimatedSprite)GetNode("playerSprite");
        playerSprite.Animation = "Idle";
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (alive)
        {
            float actualSpeed = speed;
            dir = new Vector2();
            if (Input.IsActionPressed("move_up")) dir.y -= 1;
            if (Input.IsActionPressed("move_down")) dir.y += 1;
            if (Input.IsActionPressed("move_right")) dir.x += 1;
            if (Input.IsActionPressed("move_left")) dir.x -= 1;

            if(dir.Length() > 0.01f)
            {
                playerSprite.Animation = "walk";
                playerSprite.Play();
                playerSprite.SpeedScale = 1f;
            }
            else
            {
                playerSprite.Animation = "Idle";
            }

            if(Input.IsActionPressed("sprint") && remainingSprintTime > 0f)
            {
                remainingSprintTime -= delta;
                actualSpeed = sprintSpeed;
                playerSprite.SpeedScale = 2f;
            }

            Vector2 mouseGlobalPos = GlobalPosition.DirectionTo(GetGlobalMousePosition()).Normalized();

            //float angle = Mathf.Atan2(mouseGlobalPos.y, mouseGlobalPos.x) + Mathf.Atan2(dir.y, dir.x);

            Rotation = Mathf.Atan2(dir.y, dir.x);
            //GD.Print(Mathf.Rad2Deg(angle));
            //LookAt(GetGlobalMousePosition());
            
            if(dir.Length() > 0.01f)
            {
                MoveAndSlide(Transform.x * actualSpeed);
            }
        }
        else
        {
            playerSprite.Stop();
            playerSprite.Frame = 0;

        }
    }

    public void ShowInterractionBubble(bool flag)
    {
        interactablesInRange += flag ? 1 : -1;
        speechBubble.Visible = interactablesInRange > 0;
    }

    public void Damage(float value)
    {
        if (alive)
        {
            health -= value;
            if (health <= 0f)
            {
                health = 0f;
                alive = false;
                //Die
                GD.Print("You ded");
            }
        }
    }

    public void resetSprint()
    {
        remainingSprintTime = maxSprintTime;
    }
}
