using UnityEngine;
using System.Collections.Generic;

public class Player: MonoBehaviour {
	
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

	// Test Stuff 
	[SerializeField]
	bool freeMove;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		Debug.Log (GameManager.Instance.currentState);
	}

	void Update() {
		if (GameManager.Instance.canStartGameLogic()) {
			Jump ();
		} else {
			GameManager.Instance.activeGameLogic ();
		}
	}

	void FixedUpdate(){
		if (GameManager.Instance.canStartGameLogic()) {
			if (freeMove) {
				Movement ();
			} else {
				MoveForward();
			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		CollisionEvents (coll);
	}
	
	void Movement() {
		if (Input.GetKey (KeyCode.A)) {
			MoveBackward();
		}
		if (Input.GetKey (KeyCode.D)) {
			MoveForward();
		}
	}

	void MoveBackward() {
		transform.Translate(Vector3.left * speed * Time.deltaTime);
	}
	void MoveForward() {
		transform.Translate(Vector3.right * speed * Time.deltaTime);
	}

	void Jump (){
		if (Input.GetKey (KeyCode.W) && canJump) {
			//rb.AddForce(Vector3.up * jumpForce,ForceMode2D.Impulse);
			canJump = false;
			rb.velocity = new Vector3 (rb.velocity.x, jumpForce);
			
		}
		if (GameManager.Instance.platform == RuntimePlatform.Android || 
		    GameManager.Instance.platform == RuntimePlatform.IPhonePlayer) {
			if (Input.touchCount > 0) {
				if (Input.GetTouch (0).phase == TouchPhase.Began && canJump) {
					canJump = false;
					rb.velocity = new Vector3 (rb.velocity.x, jumpForce);
				}
			}
		}
	}

	void CollisionEvents(Collision2D coll){
		if (coll.gameObject.tag == "Ground") {
			canJump = true;
		} else if (coll.gameObject.tag == "DeathDetector") {
			GameManager.Instance.currentState = GameManager.GameStates.Mainmenu;
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	private void Shot(){
		if (Input.GetKey (KeyCode.E) && timeStamp <= Time.time) {
			timeStamp = Time.time + weaponCoolDown;
		}
		//Instantiate(prefabs[0], transform.position + (transform.forward * 50), transform.rotation); 
	}
}
