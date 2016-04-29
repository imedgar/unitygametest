using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	// Player detection
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player"){
			ObjectPool.instance.PoolObject(this.gameObject);
			GameManager.Instance.showCoins = true;
			GameManager.Instance.totalCoins++;
			Invoke ("HideCoinGUI", 4.0f);
		}
    }
	
	void HideCoinGUI () {
		GameManager.Instance.showCoins = false;
	}
}
