using UnityEngine;
using System.Collections;

public class EnemyAir : Enemy {

	private float timeStamp;
	[SerializeField]
	private float flyCd;

	protected override void PhysicsStuff(){
		if ( GameManager.Instance.canStartGameLogic () ) {
			Fly ();
		}
	}
	protected override void Movement(){
		if ( GameManager.Instance.canStartGameLogic () &&  
		    playerInRange () ) {
			MoveForward ();
		}
	}

	protected void Fly(){
		if (timeStamp <= Time.time) {
			rb.velocity = new Vector3 (rb.velocity.x, jumpForce);
			timeStamp = Time.time + flyCd;
		}
	}
}
