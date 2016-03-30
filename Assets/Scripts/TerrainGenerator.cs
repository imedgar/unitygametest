using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour
{

	public static TerrainGenerator instance;
	
	// starting position for terrain, number found from tweaking in the editor
	public float startSpawnPosition = 6.0f;
	private bool firstTime = true;
	// y position that all terrain will be spawned
	// my terrain is all joined at the same level
	// you can change this if here and the spawn method
	// if you need terrain at different heights
	public float spawnYPos = 0.0f;
    public float minDistanceBetweenBuildings;
    public float maxDistanceBetweenBuildings;
    public float minHighBuildings;
    public float maxHighBuildings;
	
	private string currentBuilding;
    private float buildingSize;
	
    private float randomTerrain;
	private int lastBuildingHeight;
	
	// keep track of the last position terrain was generated
	private float lastPosition;
	private int randomY; 
	private float spawnYRandom;
	
	// Street ground variables
	
	//public float streetStartSpawnPos;
	//public float streetSpawnYPos;
	//private float streetlastPos;
	
	// Inner variables
	
	public float spawnInnerYPos;
	private float innerlastPos;

	public int spawnRange = 16;
	
	// camera reference
	private GameObject cam;
	
	// used to check if terrain can be generated depending on the camera position and lastposition
	private bool canSpawnRoofs = true;
	private bool canSpawnStreets = true;
	
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
		Behaviour (GameManager.Instance.currentState);
	}
	
	protected void Behaviour(GameManager.GameStates currentGameState){
        switch (currentGameState)
        {
			// if the camera is farther than the number last position minus 16 terrain is spawned
			// a lesser number may make the terrain 'pop' into the scene too early
			// showing the player the terrain spawning which would be unwanted
            case GameManager.GameStates.Mainmenu:
            case GameManager.GameStates.Roofs:
				if (cam.transform.position.x >= lastPosition - spawnRange && canSpawnRoofs == true)
				{
					// turn off spawning until ready to spawn again
					canSpawnRoofs = false;
					// SpawnRoofs is called and passed the randomchoice number
					SpawnRoofs(currentGameState);
				}
                break;
            //case GameManager.GameStates.Street:
			//	if (cam.transform.position.x >= streetlastPos - spawnRange && canSpawnStreets == true)
			//	{
			//		// turn off spawning until ready to spawn again
			//		canSpawnRoofs = false;
			//		// SpawnRoofs is called and passed the randomchoice number
			//		SpawnRoofs(currentGameState);
			//		// turn off spawning until ready to spawn again
			//		canSpawnStreets = false;
			//		// SpawnRoofs is called and passed the randomchoice number
			//		SpawnStreets();
			//	}			
            //    break;
			case GameManager.GameStates.InnerZone:
				SpawnInners ();
				break;
            default:
                break;
        }
	}
	
	
	// spawn terrain based on the rand int passed by the update method
	void SpawnRoofs(GameManager.GameStates spawnMode)
	{
		// Roofs algorithm 
		randomTerrain = Random.Range(1,10);
        randomY = Random.Range(1, 10);
		
		if (lastBuildingHeight == 0 && randomY > 5) {
			if (!firstTime){
				lastPosition += ( buildingSize / 2 ) + minDistanceBetweenBuildings;
			} else { firstTime = false; }
		} 
		else{
			if (!firstTime){
			lastPosition += ( buildingSize / 2 ) + maxDistanceBetweenBuildings;
			} else { firstTime = false; }
		}
		
		// Roofs Height random
		
        if (randomY <= 5)
        {
            spawnYRandom = spawnYPos + minHighBuildings;
        }
        else {
            spawnYRandom = spawnYPos + maxHighBuildings;;
        }
		
			
        if (randomTerrain <= 3)
        {
            currentBuilding = "Base_Pequena";
			buildingSize = ObjectPool.instance.GetObjectSize (currentBuilding);
			lastPosition += buildingSize / 2;
            ObjectPool.instance.GetObjectForType(currentBuilding, true, new Vector3(lastPosition, spawnYRandom, -1), Quaternion.Euler(0, 0, 0));
        }
        else if (randomTerrain > 3 && randomTerrain <= 6)
        {
            currentBuilding = "Base_Mediana";
			buildingSize = ObjectPool.instance.GetObjectSize (currentBuilding);
			lastPosition += buildingSize / 2;
            ObjectPool.instance.GetObjectForType(currentBuilding, true, new Vector3(lastPosition, spawnYRandom, -1), Quaternion.Euler(0, 0, 0));
        }
        else if (randomTerrain > 6 && randomTerrain <= 10)
        {
            currentBuilding = "Base_Larga";
			buildingSize = ObjectPool.instance.GetObjectSize (currentBuilding);
			lastPosition += buildingSize / 2;
            ObjectPool.instance.GetObjectForType(currentBuilding, true, new Vector3(lastPosition, spawnYRandom, -1), Quaternion.Euler(0, 0, 0));
        }

		if (randomY <= 5)
        {
			lastBuildingHeight = 0;
        }
        else {
			lastBuildingHeight = 1;
        }

        // script is now ready to spawn more terrain
        canSpawnRoofs = true;
	}
	
	// Concept 
	//void SpawnStreets (){
	//	
	//	// Street
	//	if (GameManager.Instance.streetsPrepared == 0){
	//		streetlastPos += cam.transform.position.x - 20;
	//	}
	//	else{
	//		streetlastPos += streetStartSpawnPos;
	//	}
	//	
	//	ObjectPool.instance.GetObjectForType ("street_ground", true, new Vector3 (streetlastPos, streetSpawnYPos, 0), Quaternion.Euler (0, 0, 0));
	//	// script is now ready to spawn more terrain
	//	canSpawnStreets = true;
	//	GameManager.Instance.streetsPrepared ++;
	//}
	
	// spawn inners
	void SpawnInners ()
	{
		// Inners algorithm 
		
		// Check distance from last building
		if (lastBuildingHeight == 0) {
			lastPosition += minDistanceBetweenBuildings;
		}
		else {
			lastPosition += maxDistanceBetweenBuildings;
		}
		
		bool lastObstacle = false;
		
		for (int i = 0; i < 12; i++){
			if (i == 0){
				buildingSize = ObjectPool.instance.GetObjectSize ("building_3_init");
				lastPosition += buildingSize / 2;
				ObjectPool.instance.GetObjectForType ("building_3_init", true, new Vector3 (lastPosition, spawnInnerYPos, -1), Quaternion.Euler (0, 0, 0));
				lastPosition += buildingSize / 2;
			} 
			else if (i == 11) {
				buildingSize = ObjectPool.instance.GetObjectSize ("building_3_final");
				lastPosition += buildingSize / 2;				
				ObjectPool.instance.GetObjectForType ("building_3_final", true, new Vector3 (lastPosition, spawnInnerYPos, -1), Quaternion.Euler (0, 0, 0));
				lastPosition += buildingSize / 2;
			} 
			else{
				buildingSize = ObjectPool.instance.GetObjectSize ("building_3");
				lastPosition += buildingSize / 2;
				ObjectPool.instance.GetObjectForType ("building_3", true, new Vector3 (lastPosition, spawnInnerYPos, -1), Quaternion.Euler (0, 0, 0));				
				int obstaclePc = Random.Range(1,10);
				if (obstaclePc > 5 && !lastObstacle){
					ObjectPool.instance.GetObjectForType ("obstacle_1", true, new Vector3 (lastPosition, 4, -2), Quaternion.Euler (0, 0, 0));
					Debug. Log ("Obstacle!!");
					lastObstacle = true;
				}
				else {
					lastObstacle = false;
				}
				lastPosition += buildingSize / 2;
			}
		}

		GameManager.Instance.currentState = GameManager.GameStates.Roofs;
	}	
	
	void spaceBetweenBuildings (){
		
	}
	
}