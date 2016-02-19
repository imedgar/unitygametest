using UnityEngine;
using System.Collections;

public class TerrainBasics : MonoBehaviour {

	public GameObject camRef;

	// Use this for initialization
	void Start () {
		// pair camera to camera reference
		camRef = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
		if ((camRef.transform.position.x) - (gameObject.transform.position.x + 0.1) >= 25)
		{
			DestroyTerrain();
		}
	}

	void DestroyTerrain () {
		ObjectPool.instance.PoolObject(gameObject);
	}
}
