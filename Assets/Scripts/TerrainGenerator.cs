using UnityEngine;
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
    public float minDistanceBetweenBuildings;
    public float maxDistanceBetweenBuildings;
    public int minHighBuildings;
    public int maxHighBuildings;

    private float randomTerrain;
    int lastBuilding;
	
	// keep track of the last position terrain was generated
	float lastPosition;
	
	// Street ground variables
	
	public float streetStartSpawnPos;
	public float streetSpawnYPos;
	float streetlastPos;
	

	public int spawnRange = 16;
	
	// camera reference
	GameObject cam;
	
	// used to check if terrain can be generated depending on the camera position and lastposition
	bool canSpawnRoofs = true;
	bool canSpawnStreets = true;
	
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
		
		if (cam.transform.position.x >= lastPosition - spawnRange && canSpawnRoofs == true)
		{
			// turn off spawning until ready to spawn again
			canSpawnRoofs = false;
			// SpawnRoofs is called and passed the randomchoice number
			SpawnRoofs();
		}
		if (cam.transform.position.x >= streetlastPos - spawnRange && canSpawnStreets == true && GameManager.Instance.currentState == GameManager.GameStates.Street)
		{
			// turn off spawning until ready to spawn again
			canSpawnStreets = false;
			// SpawnRoofs is called and passed the randomchoice number
			SpawnStreets();
		}
	}
	
	// spawn terrain based on the rand int passed by the update method
	void SpawnRoofs()
	{
		// Roofs algorithm 
		randomTerrain = Random.Range(1,10);

        if (lastBuilding == 0 && randomTerrain > 5)
        {
            lastPosition += startSpawnPosition + minDistanceBetweenBuildings;
        }
        else { 
			lastPosition += startSpawnPosition + maxDistanceBetweenBuildings; 
		}
		
        if (randomTerrain <= 5) {
			if (GameManager.Instance.currentState == GameManager.GameStates.Street){
				ObjectPool.instance.GetObjectForType ("building_1", true, new Vector3 (lastPosition, spawnYPos + minHighBuildings, -1), Quaternion.Euler (0, 0, 0));

			}else{
				ObjectPool.instance.GetObjectForType ("building_1", true, new Vector3 (lastPosition, spawnYPos + minHighBuildings, -1), Quaternion.Euler (0, 0, 0));

			}
            lastBuilding = 0;
		} else{
			if (GameManager.Instance.currentState == GameManager.GameStates.Street){
				ObjectPool.instance.GetObjectForType ("building_2", true, new Vector3 (lastPosition, spawnYPos + minHighBuildings, -1), Quaternion.Euler (0, 0, 0));

			}else{
				ObjectPool.instance.GetObjectForType ("building_2", true, new Vector3 (lastPosition, spawnYPos + maxHighBuildings, -1), Quaternion.Euler (0, 0, 0));
			}
			lastBuilding = 1;
		}
		
		// script is now ready to spawn more terrain
		canSpawnRoofs = true;
	}
	
	void SpawnStreets (){
		
		// Street
		
		if (GameManager.Instance.streetsPrepared == 0){streetlastPos += cam.transform.position.x - 20;}
		else{streetlastPos += streetStartSpawnPos;}
		
		ObjectPool.instance.GetObjectForType ("street_ground", true, new Vector3 (streetlastPos, streetSpawnYPos, 0), Quaternion.Euler (0, 0, 0));
		// script is now ready to spawn more terrain
		canSpawnStreets = true;
		GameManager.Instance.streetsPrepared ++;
	}

}