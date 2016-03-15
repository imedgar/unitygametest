using UnityEngine;
using System.Collections;

public class EnemyAir : Enemy {

	private float timeStamp;
	[SerializeField]
	private float flyCd;

	protected override void PhysicsStuff(){
		Fly ();
	}
	protected override void Movement(){
		if ( PlayerInRange () ) {
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
