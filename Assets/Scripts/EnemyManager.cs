using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
	public List<GameObject> prefabs;
	[SerializeField]
	public bool spawnEnemyPlatypus;
	[SerializeField]
	public bool spawnEnemyBerserker;
    [SerializeField]
    public int distanceToPlayerSpawn;

    public GameObject playerRef;

    int spawnPositionX;
	private RaycastHit2D hit;

	private void Start(){
		playerRef = GameObject.FindGameObjectWithTag ("Player");
		if (spawnEnemyPlatypus) {
			InvokeRepeating("SpawnPrefabPlatypus",0, 6.0f);
		}
		if (spawnEnemyBerserker) {
			InvokeRepeating("SpawnPrefabBerserker",2, 5.0f);
		}
	}
	
	private void SpawnPrefabPlatypus(){
		if (GameManager.Instance.canStartGameLogic ()) {
			ObjectPool.instance.GetObjectForType ("Platypus", true, new Vector3 (playerRef.transform.position.x + 24, 9, 0)
                , Quaternion.Euler (0, 0, 0));
		}
	}
	private void SpawnPrefabBerserker(){
		if (GameManager.Instance.canStartGameLogic ()) {
            spawnPositionX = GetSpawnPositionOverBuilding();
            if (spawnPositionX != 0)
            {
                ObjectPool.instance.GetObjectForType("Berserker", true, new Vector3(spawnPositionX, 6, 0), Quaternion.Euler(0, 0, 0));
            }
		}
	}
	
	private int GetSpawnPositionOverBuilding (){
        int positionX = 0;
        Vector2 playerPosition = new Vector2(playerRef.transform.position.x + distanceToPlayerSpawn, 10);
		for (int i = (int)playerRef.transform.position.x + distanceToPlayerSpawn; 
            i < Mathf.Round(playerRef.transform.position.x + distanceToPlayerSpawn) + 50; 
            i++ )
        {
			hit = Physics2D.Raycast(playerPosition, Vector2.down, 100, 1 << LayerMask.NameToLayer("Ground"));
            if (hit) {
                if (hit.collider.tag == "Ground"){
                    positionX = i;
					break;
				}
			} 
		}
		return positionX;
	}

	private Vector3 GetRandomX(float minX, float maxX, float yAxis){

		float randomX = Random.Range (minX, maxX);
		Vector3 vectorRandomX = new Vector3 (randomX, yAxis, 0);

		return vectorRandomX;
	}
}