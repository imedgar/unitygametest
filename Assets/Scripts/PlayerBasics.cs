using UnityEngine;
using System.Collections.Generic;

public class PlayerBasics : MonoBehaviour {
	
	//Variables
	[SerializeField]
	float speed;
	[SerializeField]
	float jumpForce;
	[SerializeField]
	private GameObject groundCheck;
	public List<GameObject> prefabs;
	[SerializeField]
	float weaponCoolDown;
	float timeStamp;
	bool canJump;

	void Start() {
		groundCheck = GameObject.FindWithTag("Ground");
	}

	void Update() {
		//if (Input.GetKeyDown ("w") && !isGrounded()) {
		//	transform.Translate(Vector3.up * ( speed * jumpForce ) * Time.deltaTime);
		//}
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
			transform.Translate(Vector3.up * ( speed * jumpForce ) * Time.deltaTime);
			transform.Translate(Vector3.right * ( speed * 2 ) * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.E) && timeStamp <= Time.time) {
			Shot();
			timeStamp = Time.time + weaponCoolDown;
		}
	}

	private void Shot(){
		Instantiate(prefabs[0], transform.position + (transform.forward * 50), transform.rotation); 
	}

	public bool isGrounded(){

		bool result =  Physics2D.Linecast(transform.position , groundCheck.transform.position , 1 << LayerMask.NameToLayer("Ground"));
		if (result) {
			Debug.DrawLine(transform.position , groundCheck.transform.position , Color.green, 0.5f, false);
		}
		else {
			Debug.DrawLine(transform.position , groundCheck.transform.position , Color.red, 0.5f, false);
		}
		return result;
	}
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Ground") {
			canJump = true;
		}
	}

}
