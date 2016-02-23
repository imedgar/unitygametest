﻿using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	public float velocity = 0f;
	public GameObject target;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position = new Vector3(target.transform.position.x + 9,
		                                            4,
		                                            transform.position.z);
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2 ((Time.time * velocity) % 1, 0);
	}
}
