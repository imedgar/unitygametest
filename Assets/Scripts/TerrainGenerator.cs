﻿using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour
{

	public static TerrainGenerator instance;
	
	// starting position for terrain, number found from tweaking in the editor
	public float startSpawnPosition = 6.0f;
	// y position that all terrain will be spawned
	// my terrain is all joined at the same level
	// you can change this if here and the spawn method
	// if you need terrain at different heights
	public float spawnYPos = 0.0f;
	public int spawnRange = 16;
	public float randomTerrain;

	// keep track of the last position terrain was generated
	float lastPosition;

	// camera reference
	GameObject cam;

	// used to check if terrain can be generated depending on the camera position and lastposition
	bool canSpawn = true;

	void Awake ()
	{
		instance = this;
	}

	
	void Start()
	{
		// make the lastposition start at start spawn position
		lastPosition = startSpawnPosition;
		// pair camera to camera reference
		cam = GameObject.Find("Main Camera");
	}
	
	void Update()
	{
		// if the camera is farther than the number last position minus 16 terrain is spawned
		// a lesser number may make the terrain 'pop' into the scene too early
		// showing the player the terrain spawning which would be unwanted
		if (cam.transform.position.x >= lastPosition - spawnRange && canSpawn == true)
		{
			// turn off spawning until ready to spawn again
			canSpawn = false;

			// SpawnTerrain is called and passed the randomchoice number
			SpawnTerrain();
		}
	}
	
	// spawn terrain based on the rand int passed by the update method
	void SpawnTerrain()
	{
		randomTerrain = Random.Range(1,13);
		if (randomTerrain <= 4) {
			ObjectPool.instance.GetObjectForType ("PlatformTerrain2", true, new Vector3 (lastPosition, spawnYPos, 0), Quaternion.Euler (0, 0, 0));
		} else if (randomTerrain >= 4 && randomTerrain <= 7) {
			ObjectPool.instance.GetObjectForType ("PlatformTerrain3", true, new Vector3 (lastPosition, spawnYPos, 0), Quaternion.Euler (0, 0, 0));
		} else if (randomTerrain >= 7 && randomTerrain <= 10) {
			ObjectPool.instance.GetObjectForType ("PlatformTerrain", true, new Vector3 (lastPosition, spawnYPos, 0), Quaternion.Euler (0, 0, 0));
		} else {
			ObjectPool.instance.GetObjectForType ("PlatformTerrain4", true, new Vector3 (lastPosition, spawnYPos, 0), Quaternion.Euler (0, 0, 0));
		}
		lastPosition += startSpawnPosition;

		// script is now ready to spawn more terrain
		canSpawn = true;
	}

}