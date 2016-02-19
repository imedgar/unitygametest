using UnityEngine;
using System.Collections.Generic;

public class PlayerBasics : MonoBehaviour {
	
	//Variables
	[SerializeField]
	float speed;
	[SerializeField]
	float jumpForce;
	[SerializeField]
	public List<GameObject> prefabs;
	[SerializeField]
	float weaponCoolDown;

	float timeStamp;
	bool canJump;

	// Player RigidBody Reference
	Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	void Update() {
	}

	void FixedUpdate(){

		Movement ();
	}

	void Movement() {
		if (Input.GetKey (KeyCode.A)) {
			transform.Translate(Vector3.left * speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Translate(Vector3.right * speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.W) && canJump) {
			canJump = false;
			rb.AddForce(Vector3.up * jumpForce,ForceMode2D.Impulse);
		}
		if (Input.GetKey (KeyCode.E) && timeStamp <= Time.time) {
			Shot();
			timeStamp = Time.time + weaponCoolDown;
		}
	}

	private void Shot(){
		//Instantiate(prefabs[0], transform.position + (transform.forward * 50), transform.rotation); 
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Ground") {
			canJump = true;
		}
	}

}
