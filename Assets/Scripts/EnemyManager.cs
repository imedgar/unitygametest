using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
	public List<GameObject> prefabs;
	[SerializeField]
	public bool spawnBoxes;
	[SerializeField]
	public bool spawnTargets;

	private void Start(){
		if (spawnBoxes) {
			InvokeRepeating("SpawnPrefabBoxes",0, 2.0f);
		}
		if (spawnTargets) {
			InvokeRepeating("SpawnPrefabTargets",3, 5.0f);
		}

	}
	
	private void SpawnPrefabBoxes(){
		Instantiate(prefabs[0], GetRandomX(-8.0f, 9.0f, 40.0f), Quaternion.identity);
	}
	private void SpawnPrefabTargets(){
		Instantiate(prefabs[1], GetRandomX(-3.0f, 9.0f, -4.07f), Quaternion.identity);
	}

	private Vector3 GetRandomX(float minX, float maxX, float yAxis){

		float randomX = Random.Range (minX, maxX);
		Vector3 vectorRandomX = new Vector3 (randomX, yAxis, 0);

		return vectorRandomX;
	}
}