using UnityEngine;
using System.Collections;

public class DeathDetector : MonoBehaviour {

	GameObject playerRef;

	// Use this for initialization
	void Start () {
		playerRef = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = new Vector3(playerRef.transform.position.x ,
		                                 -3,
		                                 playerRef.transform.position.z);
	
	}
}
