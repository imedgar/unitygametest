using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
	public List<GameObject> prefabs;
	
	private void Start(){
		InvokeRepeating("SpawnPrefab",0, 2.0f);
	}
	
	private void SpawnPrefab(){
		//This will spawn them at (0,0,0) with (0,0,0) rotation;
		Instantiate(prefabs[Random.Range(0, prefabs.Count)], GetRandomX(), Quaternion.identity); 
	}

	private Vector3 GetRandomX(){

		float randomX = Random.Range (-8.0f, 9.0f);
		Vector3 vectorRandomX = new Vector3 (randomX, 40.0f, 0);

		return vectorRandomX;
	}
}