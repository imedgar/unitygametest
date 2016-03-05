using UnityEngine;
using System.Collections;

public class EnemyBerserker : Enemy {

	[SerializeField]
	private float jumpCd;
	[SerializeField]
	private float berserkerSprint;

	private float timeStamp;

	protected override void Collided(Collision2D coll){
		if (coll.gameObject.tag == "Ground") {
			canJump = true;
		} else if (coll.gameObject.tag == "Player") {
			GameManager.Instance.currentState = GameManager.GameStates.Mainmenu;
			Application.LoadLevel (Application.loadedLevel);
		} else if (coll.gameObject.tag == "EdgeCollider" &&  
		           playerInRange ()) {
			rb.velocity = new Vector3 (rb.velocity.x, jumpForce);
		}
	}

	protected override void Movement(){
		if ( GameManager.Instance.canStartGameLogic () &&  
		    playerInRange () ) {
			BerserkerRun ();
		}
	}

	protected override void PhysicsStuff(){
		//if ( GameManager.Instance.canStartGameLogic () &&  
		//    ( playerRef.transform.position.x - transform.position.x) < visionRange ) {
		//	Jump ();
		//}
	}

	private void BerserkerRun(){
		transform.Translate ( Vector3.left * speed * Time.deltaTime );
		speed += berserkerSprint;
	}

	protected void Jump(){
		if (canJump && timeStamp <= Time.time) {
			canJump = false;
			rb.velocity = new Vector3 (rb.velocity.x, jumpForce);
			timeStamp = Time.time + jumpCd;
		}
	}



}
