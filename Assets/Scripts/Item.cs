using UnityEngine;
using System.Collections;

public class BoxBasics : MonoBehaviour {

	public GameObject playerRef;

	public Vector3 currentPlayerPos;

	// Use this for initialization
	void Start () {
		playerRef = GameObject.FindGameObjectWithTag ("Player");
		currentPlayerPos = playerRef.transform.position;
		Vector3 temp = new Vector3(
			(currentPlayerPos.x + 10),0.1f,0);
		this.transform.position = temp;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Ground") {
			//DestroyObject(gameObject);
			ObjectPool.instance.PoolObject(gameObject);
		}
	}
}
