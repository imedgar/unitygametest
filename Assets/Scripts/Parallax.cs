using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	public float velocity = 0f;
	public GameObject target;
	public float offsetY;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {


				if (target.transform.position.y < -4){
		gameObject.transform.position = new Vector3 (target.transform.position.x + 9, 
			-3 + 2 + offsetY, 
			transform.position.z);
		}
		else{
		gameObject.transform.position = new Vector3 (target.transform.position.x + 9, 
			4 + 2 + offsetY, 
			transform.position.z);

		}

		if (GameManager.Instance.currentState != GameManager.GameStates.Mainmenu) {

			GetComponent<Renderer> ().material.mainTextureOffset = new Vector2 ((Time.time * velocity) % 1, 0);
		}
	}
}
