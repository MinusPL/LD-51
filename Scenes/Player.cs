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
    
    [Export]
    bool hasWeapon;

    [Export]
    bool hasLamp;


    Color playerColor;
    [Export]
    bool caramelMode = false;

    [Export]
    float caramelChangeSpeed = 0.2f;
    float hue = 0f;

    Vector2 dir;

    AnimatedSprite playerSprite;

    Light2D playerLamp;

    //Wiem wiem, dziwne, ale nie mam lepszego pomyslu na teraz...
    // No i w poziomie raczej nie beda na siebie interakcje nachodzic.
    int interactablesInRange = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        health = maxHealth;
        remainingSprintTime = maxSprintTime;
        playerSprite = (AnimatedSprite)GetNode("playerSprite");
        playerSprite.Animation = "Idle";
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        playerColor = Color.FromHsv(rng.RandfRange(0f, 1f), 0.7f, 1f);
        playerSprite.Modulate = playerColor;
        playerLamp = (Light2D)GetNode("Lamp");
        playerLamp.Visible = hasLamp;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(caramelMode)
        {
            playerColor.h += caramelChangeSpeed * delta;
            if (playerColor.h > 1f) playerColor.h -= 1f;
            playerSprite.Modulate = playerColor;
        }

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


            //attack
            if(Input.IsActionJustPressed("attack") && hasWeapon)
            {

            }
            
            if(dir.Length() > 0.01f)
            {
                Rotation = Mathf.Atan2(dir.y, dir.x);
                MoveAndSlide(Transform.x * actualSpeed);
            }
        }
        else
        {
            playerSprite.Stop();
            playerSprite.Frame = 0;

        }
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

    public void PickUp(PickupType type)
    {
        switch(type)
        {
            case PickupType.sword:
                hasWeapon = true;
                break;
            case PickupType.lamp:
                hasLamp = true;
                playerLamp.Visible = true;
                break;
        }
    }
}
