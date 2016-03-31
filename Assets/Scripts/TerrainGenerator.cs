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
    public Rect scissorRect = new Rect(0, 0, 1, 1);

    void Start()
	{
		// make the lastposition start at start spawn position
		lastPosition = startSpawnPosition;
		// pair camera to camera reference
		cam = GameObject.Find("Main Camera");
        //StartCoroutine(CoroutineTerrain());
        //InvokeRepeating("Behaviour", 0, 0.25f);
    }
	
	void Update()
	{
		Behaviour ();
        SetScissorRect (Camera.main, scissorRect);

    }

    public static void SetScissorRect(Camera cam, Rect r)
    {
        if (r.x < 0)
        {
            r.width += r.x;
            r.x = 0;
        }

        if (r.y < 0)
        {
            r.height += r.y;
            r.y = 0;
        }

        r.width = Mathf.Min(1 - r.x, r.width);
        r.height = Mathf.Min(1 - r.y, r.height);

        cam.rect = new Rect(0, 0, 1, 1);
        cam.ResetProjectionMatrix();
        Matrix4x4 m = cam.projectionMatrix;
        cam.rect = r;
        Matrix4x4 m1 = Matrix4x4.TRS(new Vector3(r.x, r.y, 0), Quaternion.identity, new Vector3(r.width, r.height, 1));
        Matrix4x4 m2 = Matrix4x4.TRS(new Vector3((1 / r.width - 1), (1 / r.height - 1), 0), Quaternion.identity, new Vector3(1 / r.width, 1 / r.height, 1));
        Matrix4x4 m3 = Matrix4x4.TRS(new Vector3(-r.x * 2 / r.width, -r.y * 2 / r.height, 0), Quaternion.identity, Vector3.one);
        cam.projectionMatrix = m3 * m2 * m;
    }

    //IEnumerator CoroutineTerrain()
    //{
    //    GUI.depth = 2;
    //    while (true)
    //    {
    //        if (Time.timeScale == 1)
    //        {
    //            yield return new WaitForSeconds(0.5f);
    //            Behaviour();
    //        }
    //        yield return new WaitForSeconds(0.5f);
    //    }
    //}

    protected void Behaviour(){

        switch (GameManager.Instance.currentState)
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
					SpawnRoofs(GameManager.Instance.currentState);
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
		
			
        if (randomTerrain <= 4)
        {
            currentBuilding = "Base_Pequena";
			buildingSize = ObjectPool.instance.GetObjectSize (currentBuilding);
			lastPosition += buildingSize / 2;
            ObjectPool.instance.GetObjectForType(currentBuilding, true, new Vector3(lastPosition, spawnYRandom, -1), Quaternion.Euler(0, 0, 0));
        }
        else if (randomTerrain > 4 && randomTerrain <= 8)
        {
            currentBuilding = "Base_Mediana";
			buildingSize = ObjectPool.instance.GetObjectSize (currentBuilding);
			lastPosition += buildingSize / 2;
            ObjectPool.instance.GetObjectForType(currentBuilding, true, new Vector3(lastPosition, spawnYRandom, -1), Quaternion.Euler(0, 0, 0));
        }
        else if (randomTerrain > 8 && randomTerrain <= 10)
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
			lastPosition += ( buildingSize / 2 ) + minDistanceBetweenBuildings;
		}
		else {
			lastPosition += ( buildingSize / 2 ) + maxDistanceBetweenBuildings;
		}
		
		bool lastObstacle = false;
		
		for (int i = 0; i < 10; i++){
			if (i == 0){
				buildingSize = ObjectPool.instance.GetObjectSize ("Inicio_Interior_prueba");
				lastPosition += buildingSize / 2;
				ObjectPool.instance.GetObjectForType ("Inicio_Interior_prueba", true, new Vector3 (lastPosition, spawnInnerYPos, -1), Quaternion.Euler (0, 0, 0));
				lastPosition += buildingSize / 2;
			} 
			else if (i == 9) {
				buildingSize = ObjectPool.instance.GetObjectSize ("Interior_prueba_bloque_final");
				lastPosition += buildingSize / 2;				
				ObjectPool.instance.GetObjectForType ("Interior_prueba_bloque_final", true, new Vector3 (lastPosition, spawnInnerYPos, -1), Quaternion.Euler (0, 0, 0));
			} 
			else{
				buildingSize = ObjectPool.instance.GetObjectSize ("Interior_prueba_bloque medio");
				lastPosition += buildingSize / 2;
				ObjectPool.instance.GetObjectForType ("Interior_prueba_bloque medio", true, new Vector3 (lastPosition, spawnInnerYPos, -1), Quaternion.Euler (0, 0, 0));				
				int obstaclePc = Random.Range(1,10);
				if (obstaclePc > 4 && !lastObstacle){
					ObjectPool.instance.GetObjectForType ("obstaculo", true, new Vector3 (lastPosition, 2.66f, -2), Quaternion.Euler (0, 0, 0));
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

}