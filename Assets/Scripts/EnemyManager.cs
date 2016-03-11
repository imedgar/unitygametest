using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
	public List<GameObject> prefabs;
	[SerializeField]
	public bool spawnEnemyPlatypus;
	[SerializeField]
	public bool spawnEnemyBerserker;

	public GameObject playerRef;
	
	private RaycastHit2D hit;

	private void Start(){
		playerRef = GameObject.FindGameObjectWithTag ("Player");
		if (spawnEnemyPlatypus) {
			InvokeRepeating("SpawnPrefabPlatypus",0, 6.0f);
		}
		if (spawnEnemyBerserker) {
			InvokeRepeating("SpawnPrefabBerserker",3, 5.0f);
		}
	}
	
	private void SpawnPrefabPlatypus(){

		if (GameManager.Instance.canStartGameLogic ()) {
			ObjectPool.instance.GetObjectForType ("Platypus", true, new Vector3 (playerRef.transform.position.x + 24, 9, 0), Quaternion.Euler (0, 0, 0));
		}
	}
	private void SpawnPrefabBerserker(){
		
		if (GameManager.Instance.canStartGameLogic ()) {
			ObjectPool.instance.GetObjectForType ("Berserker", true, new Vector3 (playerRef.transform.position.x + 40, 6, 0), Quaternion.Euler (0, 0, 0));
		}
	}
	
	private int GetSpawnPositionOverBuilding (){

		int positionX = 0; 
		
		for (int i = (int)playerRef.transform.position.x + 24; i < Mathf.Round(playerRef.transform.position.x + 24) + 50; i++ ){
				hit = Physics2D.Raycast(transform.position, Vector2.down, 100, 1 << LayerMask.NameToLayer("Ground"));
				Debug.Log (hit.collider.tag);
				if (hit) {  
				
					if(hit.collider.tag == "Ground"){
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