using Godot;
using Godot.Collections;
//using System;

public class LevelManager : Node2D
{
	public Player player;


	//Timers
	[Export]
	public float respawnTime = 10f;

	float respTimer = 0f;

	Array spawnPoints;

	SpawnPoint activeSpawnPoint;
	
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = (Player)GetNode("/root/Game/Player");

		spawnPoints = GetTree().GetNodesInGroup("spawn_points");

		foreach(var sp in spawnPoints)
		{
			if(((SpawnPoint)sp).enabled)
			{
				activeSpawnPoint = (SpawnPoint)sp;
				player.Position = activeSpawnPoint.spawnPosition;
				break;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		respTimer += delta;
		if (respTimer >= respawnTime)
		{
			player.Position = activeSpawnPoint.spawnPosition;
			player.resetSprint();
			respTimer = 0f;
		}
	}

	public void ChangeActiveSpawnPoint(SpawnPoint point)
	{
		activeSpawnPoint.SetEnabled(false);
		activeSpawnPoint = point;
		activeSpawnPoint.SetEnabled(true);
	}

	public void OnNavigationCheck()
    {
		GetTree().CallGroup("enemies", "GetPathToTarget");
    }
}
