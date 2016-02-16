using UnityEngine;
using System.Collections;

public class ShotBasics : MonoBehaviour {

	//Variables
	[SerializeField]
	float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.right * speed * Time.deltaTime);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Target") {
			DestroyObject(gameObject);
			DestroyObject(coll.gameObject);
		}
	}
}
