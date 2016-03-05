using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public GameObject target;
	public float xOffset;
	public float yOffset;
	public float zOffset;

	void Start (){
		target = GameObject.FindGameObjectWithTag ("Player");
	}

	void LateUpdate() {
		//Debug.Log (target.transform.position.x);
		gameObject.transform.position = new Vector3(target.transform.position.x + xOffset,
		                                            4 + yOffset,
		                                      target.transform.position.z + zOffset);
	}
}