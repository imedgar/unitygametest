using UnityEngine;
using System.Collections;

public class TerrainManager : MonoBehaviour {

	private bool firstTime = true;
	
	[SerializeField]	
	private Transform playerPositionCached;	
	
	[SerializeField]
	private int spawnRange = 16;
	
	[SerializeField]
    private float minDistanceBetweenBuildings;
    [SerializeField]
	private float maxDistanceBetweenBuildings;
    [SerializeField]
	private float heightDistanceBetweenBuildings;

	
	[SerializeField]
	private GameObject lastBuildingRef;	
	private int lastBuildingHeight = 1;
	// keep track of the last position terrain was generated
    private float lastPosition;
	
	private string currentBuilding;
    private float buildingSize;
	
	private int randomY; 
    private float randomTerrain;
	private int maxRandomX = 10;

	// used to check if terrain can be generated depending on the camera position and lastposition
	private bool canSpawnRoofs = true;

    void Start()
	{
        StartCoroutine(CoroutineTerrain());
    }

    IEnumerator CoroutineTerrain()
    {
        while (true)
        {
            if (Time.timeScale >= 1)
            {
               yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.2f));
               Behaviour();
            }
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.2f));
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
                if (lastPosition - playerPositionCached.position.x <= spawnRange && canSpawnRoofs == true)
                {
                    // turn off spawning until ready to spawn again
                    canSpawnRoofs = false;
					// SpawnRoofs is called and passed the randomchoice number
					SpawnRoofs();
				}
                break;
            default:
                break;
        }
	}
	
	
	// spawn terrain based on the rand int passed by the update method
	void SpawnRoofs() {
		
		randomTerrain = Random.Range(1,maxRandomX);
		
		// if last building was low cannot be high
        randomY = Random.Range(1, (lastBuildingHeight == 1) ? 3 : 4);
		
		float height = 1;
		
		// Random building
		if(randomTerrain <= 5) {
			currentBuilding = "Edificio_1"; // Getting this form Object Pool
		} else {
			currentBuilding = "Edificio_1"; // Getting this form Object Pool
		}
		
		// Random height 
		if (randomY == 1) {
			height = 0f;
		} else if (randomY == 2){
			height = heightDistanceBetweenBuildings;
		} else if (randomY == 3){
			height = heightDistanceBetweenBuildings * 2;
		}
		
        
		buildingSize = ObjectPool.instance.GetObjectSize (currentBuilding); // Get size of current building
		
		lastPosition = lastBuildingRef.transform.position.x + ( buildingSize / 2 ) + 
				((randomY <= lastBuildingHeight) ? maxDistanceBetweenBuildings : minDistanceBetweenBuildings);		

		lastPosition += buildingSize / 2;
		
		// Getting the building from Object Pool and save its reference
        lastBuildingRef = ObjectPool.instance.GetObjectForType(currentBuilding, true, new Vector3(lastPosition, height, 0), Quaternion.Euler(0, 0, 0));
    	lastBuildingHeight = randomY;
		
        // script is now ready to spawn more terrain
        canSpawnRoofs = true;

	}

}