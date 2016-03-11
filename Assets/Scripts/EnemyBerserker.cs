using UnityEngine;
using System.Collections;

public class EnemyBerserker : Enemy {

	[SerializeField]
	private float jumpCd;
	[SerializeField]
	private float berserkerSprint;

	private float timeStamp;

	protected override void Collided(Collision2D coll){
		switch (coll.gameObject.tag) {
		case "Player":
			if(GameManager.Instance.currentPlayerState != GameManager.PlayerStates.Shielded){
				GameManager.Instance.currentState = GameManager.GameStates.Mainmenu;
				Application.LoadLevel (Application.loadedLevel);
			} else {
				KillEnemy ();
			}
			break;
		default:
			break;
		}
	}

	protected override void Movement(){
		if ( GameManager.Instance.canStartGameLogic () &&  
		    playerInRange () ) {
			BerserkerRun ();
		}
	}

	protected override void PhysicsStuff(){
		if ( GameManager.Instance.canStartGameLogic ()) {
			//Jump ();
		}
	}

	private void BerserkerRun(){
		transform.Translate ( Vector3.left * speed * Time.deltaTime );
		speed += berserkerSprint;
	}

	protected void Jump(){
		if (!IsGrounded() && timeStamp <= Time.time) {
			rb.velocity = new Vector3 (rb.velocity.x, jumpForce);
			timeStamp = Time.time + jumpCd;
		}
	}
}
