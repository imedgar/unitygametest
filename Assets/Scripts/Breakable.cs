﻿using UnityEngine;
using System.Collections;

public class Breakable : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        Debug.Log("TOUCHED!");
        ObjectPool.instance.PoolObject(gameObject);
    }

}
