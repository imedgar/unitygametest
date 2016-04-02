using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour
{

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
    public GameObject lastBuildingRef;	
    private float randomTerrain;
	private int lastBuildingHeight;

    public int spawnRange = 16;
	private int randomY; 
	private float spawnYRandom;
	
    // keep track of the last position terrain was generated
	
    private float lastPosition;

	// Inner variables
	
	public float spawnInnerYPos;
	private float innerlastPos;
    private int i;
	public int innerZoneLength;
	private bool lastObstacle = false;
	
	// used to check if terrain can be generated depending on the camera position and lastposition
	private bool canSpawnRoofs = true;
	private bool canSpawnStreets = true;

    void Start()
	{
		// make the lastposition start at start spawn position
		lastPosition = startSpawnPosition;
        StartCoroutine(CoroutineTerrain());
        //InvokeRepeating("Behaviour", 0, 0.1f);
    }
	
	//void Update()
	//{
	//	Behaviour ();
    //}

    IEnumerator CoroutineTerrain()
    {
        while (true)
        {
            if (Time.timeScale == 1)
            {
                yield return new WaitForSeconds(0.1f);
                Behaviour();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    protected void Behaviour(){

        switch (GameManager.Instance.currentState)
        {
			// if the camera is farther than the number last position minus 16 terrain is spawned
			// a lesser number may make the terrain 'pop' into the scene too early
			// showing the player the terrain spawning which would be unwanted
            case GameManager.GameStates.Mainmenu:
            case GameManager.GameStates.Roofs:

                //if (cam.transform.position.x >= lastPosition - spawnRange && canSpawnRoofs == true)

                if (lastBuildingRef.transform.position.x - spawnRange <= spawnRange && canSpawnRoofs == true)
                {
                    // turn off spawning until ready to spawn again
                    canSpawnRoofs = false;
					// SpawnRoofs is called and passed the randomchoice number
					SpawnRoofs();
				}
                break;
			case GameManager.GameStates.InnerZone:
                //if (cam.transform.position.x >= lastPosition - spawnRange && canSpawnRoofs == true)
                if (lastBuildingRef.transform.position.x - spawnRange <= spawnRange && canSpawnStreets == true)
                {
                    // turn off spawning until ready to spawn again
                    canSpawnStreets = false;
                    SpawnInners();
                }
				break;
            default:
                break;
        }
	}
	
	
	// spawn terrain based on the rand int passed by the update method
	void SpawnRoofs()
	{
		// Roofs algorithm 
		randomTerrain = Random.Range(1,10);
        randomY = Random.Range(1, 11);
		
		if (lastBuildingHeight == 0 && randomY > 5) {
			if (!firstTime){
				lastPosition = lastBuildingRef.transform.position.x + ( buildingSize / 2 ) + minDistanceBetweenBuildings;
			} else { firstTime = false; }
		} 
		else{
			if (!firstTime){
                lastPosition = lastBuildingRef.transform.position.x + ( buildingSize / 2 ) + maxDistanceBetweenBuildings;
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
		
			
        if (randomTerrain <= 4)
        {
            currentBuilding = "Base_Pequena";
			buildingSize = ObjectPool.instance.GetObjectSize (currentBuilding);
			lastPosition += buildingSize / 2;
            lastBuildingRef = ObjectPool.instance.GetObjectForType(currentBuilding, true, new Vector3(lastPosition, spawnYRandom, -1), Quaternion.Euler(0, 0, 0));
        }
        else if (randomTerrain > 4 && randomTerrain <= 8)
        {
            currentBuilding = "Base_Mediana";
			buildingSize = ObjectPool.instance.GetObjectSize (currentBuilding);
			lastPosition += buildingSize / 2;
            lastBuildingRef = ObjectPool.instance.GetObjectForType(currentBuilding, true, new Vector3(lastPosition, spawnYRandom, -1), Quaternion.Euler(0, 0, 0));
        }
        else if (randomTerrain > 8 && randomTerrain <= 10)
        {
            currentBuilding = "Base_Larga";
			buildingSize = ObjectPool.instance.GetObjectSize (currentBuilding);
			lastPosition += buildingSize / 2;
            lastBuildingRef = ObjectPool.instance.GetObjectForType(currentBuilding, true, new Vector3(lastPosition, spawnYRandom, -1), Quaternion.Euler(0, 0, 0));
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
		
	// spawn inners
	void SpawnInners ()
	{
		// Inners algorithm 
		
        lastPosition = lastBuildingRef.transform.position.x + ( buildingSize / 2 );
		
		if (i == 0){
			innerZoneLength = Random.Range(3,6);
            // Check distance from last building
            if (lastBuildingHeight == 0 && i == 0)
            {
                lastPosition = lastBuildingRef.transform.position.x + (buildingSize / 2) + minDistanceBetweenBuildings;
            }
            else {
                lastPosition = lastBuildingRef.transform.position.x + (buildingSize / 2) + maxDistanceBetweenBuildings;
            }
            buildingSize = ObjectPool.instance.GetObjectSize ("Inicio_Interior_prueba");
			lastPosition += buildingSize / 2;
            lastBuildingRef = ObjectPool.instance.GetObjectForType ("Inicio_Interior_prueba", true, new Vector3 (lastPosition, spawnInnerYPos, -1), Quaternion.Euler (0, 0, 0));
			lastPosition += buildingSize / 2;
            i+=1;
		} 
		else if (i == innerZoneLength) {
			buildingSize = ObjectPool.instance.GetObjectSize ("Interior_prueba_bloque_final");
			lastPosition += buildingSize / 2;
            lastBuildingRef = ObjectPool.instance.GetObjectForType ("Interior_prueba_bloque_final", true, new Vector3 (lastPosition, spawnInnerYPos, -1), Quaternion.Euler (0, 0, 0));
            lastPosition += buildingSize / 2;
            GameManager.Instance.currentState = GameManager.GameStates.Roofs;
            i = 0;
        } 
		else{
			buildingSize = ObjectPool.instance.GetObjectSize ("Interior_prueba_bloque medio");
			lastPosition += buildingSize / 2;
            lastBuildingRef = ObjectPool.instance.GetObjectForType ("Interior_prueba_bloque medio", true, new Vector3 (lastPosition, spawnInnerYPos, -1), Quaternion.Euler (0, 0, 0));				
			int obstaclePc = Random.Range(1,10);
			if (obstaclePc > 2){
				ObjectPool.instance.GetObjectForType ("obstaculo", true, new Vector3 (lastPosition, 2.66f, -2), Quaternion.Euler (0, 0, 0));
				lastObstacle = true;
			}
			else {
				lastObstacle = false;
			}
            i += 1;
        }
        canSpawnStreets = true;
    }	

}