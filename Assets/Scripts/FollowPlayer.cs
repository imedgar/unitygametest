using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public GameObject target;
	public float xOffset;
	public float yOffset;
	public float zOffset;

	void LateUpdate() {
		gameObject.transform.position = new Vector3(target.transform.position.x + xOffset,
		                                      4,
		                                      target.transform.position.z + zOffset);
	}
}