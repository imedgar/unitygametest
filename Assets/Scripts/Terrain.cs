﻿using UnityEngine;
using System.Collections;

public class Terrain : MonoBehaviour {

	private GameObject camRef;
	private BoxCollider2D boxCol;
	// Use this for initialization
	void Start () {
		// pair camera to camera reference
		camRef = GameObject.Find("Main Camera");
		boxCol = gameObject.GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((camRef.transform.position.x) - (gameObject.transform.position.x + 0.1) >= 25)
		{
			DestroyTerrain();
		}
		if (GameManager.Instance.currentState == GameManager.GameStates.Street && gameObject.tag == "Ground"){
			boxCol.enabled = false;
		} else if (GameManager.Instance.currentState != GameManager.GameStates.Street && gameObject.tag == "Ground"){
			boxCol.enabled = true;
		}
	}

	void DestroyTerrain () {
		ObjectPool.instance.PoolObject(gameObject);
	}
}
