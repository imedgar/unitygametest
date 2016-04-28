using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	// Player detection
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player"){
			ObjectPool.instance.PoolObject(this.gameObject);
		}
    }
}
