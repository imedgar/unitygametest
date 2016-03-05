﻿using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
	public List<GameObject> prefabs;
	[SerializeField]
	public bool spawnBoxes;
	[SerializeField]
	public bool spawnEnemyBerserker;

	public GameObject playerRef;

	private void Start(){
		playerRef = GameObject.FindGameObjectWithTag ("Player");
		if (spawnBoxes) {
			InvokeRepeating("SpawnPrefabBoxes",0, 2.0f);
		}
		if (spawnEnemyBerserker) {
			InvokeRepeating("SpawnPrefabBerserker",3, 5.0f);
		}
	}
	
	private void SpawnPrefabBoxes(){
		//Instantiate(prefabs[0], GetRandomX(-8.0f, 9.0f, 40.0f), Quaternion.identity);
		//ObjectPool.instance.GetObjectForType ("Box", true);
	}
	private void SpawnPrefabBerserker(){
		//Instantiate(prefabs[1], GetRandomX(-3.0f, 9.0f, -4.07f), Quaternion.identity);
		//ObjectPool.instance.GetObjectForType ("Target", true);
		if (GameManager.Instance.canStartGameLogic ()) {
			ObjectPool.instance.GetObjectForType ("Berserker", true, new Vector3 (playerRef.transform.position.x + 24, 1, 0), Quaternion.Euler (0, 0, 0));
		}
	}

	private Vector3 GetRandomX(float minX, float maxX, float yAxis){

		float randomX = Random.Range (minX, maxX);
		Vector3 vectorRandomX = new Vector3 (randomX, yAxis, 0);

		return vectorRandomX;
	}
}