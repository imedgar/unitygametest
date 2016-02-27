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

	// Test Stuff 
	[SerializeField]
	bool freeMove;

	// Controller Android
	RuntimePlatform platform = Application.platform;

	// Canvas Ref
	GameObject canvas;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		Debug.Log (GameManager.Instance.currentState);
		canvas = GameObject.FindGameObjectWithTag ("UIPanel");
	}

	void Update() {
		if (canStartGameLogic()) {
			if (Input.GetKey (KeyCode.W) && canJump) {
				//rb.AddForce(Vector3.up * jumpForce,ForceMode2D.Impulse);
				canJump = false;
				rb.velocity = new Vector3 (rb.velocity.x, jumpForce);

			}
			if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
				if (Input.touchCount > 0) {
					if (Input.GetTouch (0).phase == TouchPhase.Began && canJump) {
						canJump = false;
						rb.velocity = new Vector3 (rb.velocity.x, jumpForce);
					}
				}
			}
		} else {
			activeGameLogic();
		}
	}

	void FixedUpdate(){
		if (canStartGameLogic()) {
			if (freeMove) {
				Movement ();
			} else {
				transform.Translate(Vector3.right * speed * Time.deltaTime);
			}
		}
	}



	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Ground") {
			canJump = true;
		} else if (coll.gameObject.tag == "DeathDetector") {
			GameManager.Instance.currentState = GameManager.GameStates.Mainmenu;
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	bool canStartGameLogic (){
		if (GameManager.Instance.currentState == GameManager.GameStates.Mainmenu) {
			return false;
		} else {
			return true;
		}
	}

	void activeGameLogic (){
		if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
			if (Input.touchCount > 0) {
				if (Input.GetTouch (0).phase == TouchPhase.Began) {
					GameManager.Instance.currentState = GameManager.GameStates.Ingame;
					canvas.SetActive(false);
				}
			}
		} 
		if (Input.GetKey (KeyCode.Space)) {
			GameManager.Instance.currentState = GameManager.GameStates.Ingame;
			canvas.SetActive(false);
		}
	}
	
	void Movement() {
		if (Input.GetKey (KeyCode.A)) {
			transform.Translate(Vector3.left * speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Translate(Vector3.right * speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.E) && timeStamp <= Time.time) {
			Shot();
			timeStamp = Time.time + weaponCoolDown;
		}
	}
	
	private void Shot(){
		//Instantiate(prefabs[0], transform.position + (transform.forward * 50), transform.rotation); 
	}
}
