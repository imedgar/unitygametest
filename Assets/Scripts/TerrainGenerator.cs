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
    private int lastBuilding;
	
	// keep track of the last position terrain was generated
	private float lastPosition;
	
	// Street ground variables
	
	public float streetStartSpawnPos;
	public float streetSpawnYPos;
	private float streetlastPos;
	
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
            case GameManager.GameStates.Street:
				if (cam.transform.position.x >= streetlastPos - spawnRange && canSpawnStreets == true)
				{
					// turn off spawning until ready to spawn again
					canSpawnRoofs = false;
					// SpawnRoofs is called and passed the randomchoice number
					SpawnRoofs(currentGameState);
					// turn off spawning until ready to spawn again
					canSpawnStreets = false;
					// SpawnRoofs is called and passed the randomchoice number
					SpawnStreets();
				}			
                break;
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

        if (lastBuilding == 0 && randomTerrain > 5)
        {
            lastPosition += startSpawnPosition + minDistanceBetweenBuildings;
        }
        else { 
			lastPosition += startSpawnPosition + maxDistanceBetweenBuildings; 
		}
		
        if (randomTerrain <= 5) {
			ObjectPool.instance.GetObjectForType ("building_1", true, new Vector3 (lastPosition, spawnYPos + minHighBuildings, -1), Quaternion.Euler (0, 0, 0));
            lastBuilding = 0;
		} else{
			if (spawnMode == GameManager.GameStates.Street){
				ObjectPool.instance.GetObjectForType ("building_2", true, new Vector3 (lastPosition, spawnYPos + minHighBuildings, -1), Quaternion.Euler (0, 0, 0));

			}else{
				ObjectPool.instance.GetObjectForType ("building_2", true, new Vector3 (lastPosition, spawnYPos + maxHighBuildings, -1), Quaternion.Euler (0, 0, 0));
			}
			lastBuilding = 1;
		}
		
		// script is now ready to spawn more terrain
		canSpawnRoofs = true;
	}
	
	// Concept 
	void SpawnStreets (){
		
		// Street
		if (GameManager.Instance.streetsPrepared == 0){
			streetlastPos += cam.transform.position.x - 20;
		}
		else{
			streetlastPos += streetStartSpawnPos;
		}
		
		ObjectPool.instance.GetObjectForType ("street_ground", true, new Vector3 (streetlastPos, streetSpawnYPos, 0), Quaternion.Euler (0, 0, 0));
		// script is now ready to spawn more terrain
		canSpawnStreets = true;
		GameManager.Instance.streetsPrepared ++;
	}
	
	// spawn inners
	void SpawnInners ()
	{
		// Inners algorithm 
		
		// Check distance from last building
		if (lastBuilding == 0) {
			lastPosition += 9f;
		}
		else {
			lastPosition += 12f;
		}
		
		bool lastObstacle = false;
		
		for (int i = 0; i < 8; i++){
			if (i == 0){
				ObjectPool.instance.GetObjectForType ("building_3_init", true, new Vector3 (lastPosition, spawnInnerYPos, -1), Quaternion.Euler (0, 0, 0));
			} 
			else if (i == 7) {
				ObjectPool.instance.GetObjectForType ("building_3_final", true, new Vector3 (lastPosition, spawnInnerYPos, -1), Quaternion.Euler (0, 0, 0));
			} 
			else{
				ObjectPool.instance.GetObjectForType ("building_3", true, new Vector3 (lastPosition, spawnInnerYPos, -1), Quaternion.Euler (0, 0, 0));				
				int obstaclePc = Random.Range(1,10);
				if (obstaclePc < 9 && obstaclePc > 5 && !lastObstacle){
					ObjectPool.instance.GetObjectForType ("obstacle_1", true, new Vector3 (lastPosition, 4, -2), Quaternion.Euler (0, 0, 0));
					Debug. Log ("Obstacle!!");
					lastObstacle = true;
				} else {
					lastObstacle = false;
				}
			}
			lastPosition = lastPosition + 4.7f;
		}
		lastPosition -= 5f;
		GameManager.Instance.currentState = GameManager.GameStates.Roofs;
	}	

}