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
				GameManager.Instance.GameRestart ();
			} else {
				KillEnemy ();
			}
			break;
		default:
			break;
		}
	}
	protected override void Behaviour(string whichUpdate, GameManager.GameStates currentGameState){
	   	if (GameManager.Instance.CanStartGameLogic())
        {
            switch (currentGameState)
            {
                case GameManager.GameStates.Mainmenu:
                    break;
                case GameManager.GameStates.Roofs:
                    // Update Loop Stuff
                    if (whichUpdate.Equals("Update"))
                    {
						
                        PhysicsStuff ();
                    }
                    // FixedUpdate Loop Stuff
                    else if (whichUpdate.Equals("FixedUpdate"))
                    {
					
						BackToPool ();
						Movement ();
                    }
                    break;
                case GameManager.GameStates.Street:
                    break;
                default:
                    break;
            }
		}
	}
	
	protected override void Movement(){
		if ( PlayerInRange () ) {
			
			BerserkerRun ();
		}
	}

	protected override void PhysicsStuff(){
		if ( PlayerInRange() ) {
			
            Jump();
		}
	}

	private void BerserkerRun(){
		transform.Translate ( Vector3.left * speed * Time.deltaTime );
		speed += berserkerSprint;
	}

	protected void Jump(){
		if ( !IsGrounded() && timeStamp <= Time.time ) {
			rb.velocity = new Vector3 (rb.velocity.x, jumpForce);
			timeStamp = Time.time + jumpCd;
		}
	}
}
