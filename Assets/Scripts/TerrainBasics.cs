using UnityEngine;
using System.Collections;

public class TerrainBasics : MonoBehaviour {

	public GameObject camRef;
	public Vector3 spawnPos;
	// Use this for initialization
	void Start () {
		// pair camera to camera reference
		camRef = GameObject.Find("Main Camera");

		//spawnPos = new Vector3 (TerrainGenerator.instance.getLastPosition(), TerrainGenerator.instance.getSpawnYPos(), 0.0f);
		//this.transform.position = spawnPos;
	}
	
	// Update is called once per frame
	void Update () {
		if ((camRef.transform.position.x) - (gameObject.transform.position.x + 0.1) >= 25)
		{
			DestroyObject(gameObject);
		}
	}

	void DestroyTerrain () {
		Debug.Log((camRef.transform.position.x) - (gameObject.transform.position.x + 0.1));
		ObjectPool.instance.PoolObject(gameObject);
	}
}
