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
	private float minHighBuildings;
    [SerializeField]
	private float maxHighBuildings;
	
	[SerializeField]
	private GameObject lastBuildingRef;	
	private int lastBuildingHeight;
	// keep track of the last position terrain was generated
    private float lastPosition;
	
	private string currentBuilding;
    private float buildingSize;
	
	private int randomY; 
    private float randomTerrain;	
	private int maxRandom = 16;

	// used to check if terrain can be generated depending on the camera position and lastposition
	private bool canSpawnRoofs = true;

    void Start()
	{
        StartCoroutine(CoroutineTerrain());
        InvokeRepeating("Behaviour", 0, 0.2f);
    }

    IEnumerator CoroutineTerrain()
    {
        while (true)
        {
            if (Time.timeScale == 1)
            {
                yield return new WaitForSeconds(0.2f);
                Behaviour();
            }
            yield return new WaitForSeconds(0.2f);
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
	void SpawnRoofs()
	{
		// Roofs algorithm 
		randomTerrain = Random.Range(1,maxRandom);
        randomY = Random.Range(1, 11);
		
        currentBuilding = "Edificio_1"; // Getting this form Object Pool
		buildingSize = ObjectPool.instance.GetObjectSize (currentBuilding); // Get size of current building
		
		lastPosition = lastBuildingRef.transform.position.x + ( buildingSize / 2 ) + minDistanceBetweenBuildings;		

		lastPosition += buildingSize / 2;
		
		// Getting the building from Object Pool and save its reference
        lastBuildingRef = ObjectPool.instance.GetObjectForType(currentBuilding, true, new Vector3(lastPosition, 0, 0), Quaternion.Euler(0, 0, 0));
    
        // script is now ready to spawn more terrain
        canSpawnRoofs = true;

	}

}