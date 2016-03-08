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
	GameObject shield;

	float weaponCoolDown;
	[SerializeField]
	float shieldCoolDown;
	[SerializeField]
	float shieldDuration;

	float timeStampShield;
	bool canJump;

	// Player RigidBody Reference
	Rigidbody2D rb;

	// Test Stuff 
	[SerializeField]
	bool freeMove;


	void Start() {
		rb = GetComponent<Rigidbody2D>();
		shield.SetActive (false);
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
			UseShield ();
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
			//if (Input.touchCount > 0) {
			//	if (Input.GetTouch (0).phase == TouchPhase.Began && canJump) {
			//		canJump = false;
			//		rb.velocity = new Vector3 (rb.velocity.x, jumpForce);
			//	}
			//}
			foreach (Touch touch in Input.touches) {
				if (touch.position.x < Screen.width/2 && canJump) {
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
		//if (Input.GetKey (KeyCode.E) && timeStamp <= Time.time) {
		//	timeStamp = Time.time + weaponCoolDown;
		//}
	}

	private void UseShield (){
		if (Input.GetKey (KeyCode.R) && timeStampShield <= Time.time) {
			timeStampShield = Time.time + shieldCoolDown;
			shield.SetActive(true);
			GameManager.Instance.currentPlayerState = GameManager.PlayerStates.Shielded;
			Invoke("DontShield", shieldDuration);
		}
		if (GameManager.Instance.platform == RuntimePlatform.Android || 
		    GameManager.Instance.platform == RuntimePlatform.IPhonePlayer) {
			foreach (Touch touch in Input.touches) {
				if (touch.position.x > Screen.width/2 && timeStampShield <= Time.time) {
					timeStampShield = Time.time + shieldCoolDown;
					shield.SetActive(true);
					GameManager.Instance.currentPlayerState = GameManager.PlayerStates.Shielded;
					Invoke("DontShield", shieldDuration);
				}
			}
		}
	}

	private void DontShield(){
		shield.SetActive(false);
		GameManager.Instance.currentPlayerState = GameManager.PlayerStates.Running;
	}

}
