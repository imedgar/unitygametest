﻿using UnityEngine;
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
		if (target.transform.position.y < -4){
			gameObject.transform.position = new Vector3(target.transform.position.x + xOffset, 
			(target.transform.position.y + 3) + yOffset, 
			target.transform.position.z + zOffset);
		}
		else{
			gameObject.transform.position = new Vector3(target.transform.position.x + xOffset, 
			4 + yOffset, 
			target.transform.position.z + zOffset);
			if (GameManager.Instance.playerEnteredInnerZone && tag.Equals("MainCamera")){
				if (xOffset > 4){
					xOffset -= 1.5f * Time.deltaTime; 
				}
				
			} else if (!GameManager.Instance.playerEnteredInnerZone && tag.Equals("MainCamera")){
				if (xOffset < 7){
					xOffset += 1f * Time.deltaTime; 
				}
			}

		}
	}
}