using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {

	[SerializeField]
	private float jumpForce;
	
	private Rigidbody2D rbCached; 
	private Animator animationCached;

	private BoxCollider2D colliderCached;
	private Transform transformCached;
	private RaycastHit2D hit;
	private bool onAir;
		
	public delegate bool OnAction();
	public static event OnAction isGrounded;
	
	// Awake
	void Awake () {
		
		this.rbCached = GetComponent<Rigidbody2D> ();
		this.animationCached = GetComponent<Animator> ();
		this.colliderCached = GetComponent<BoxCollider2D> ();
		this.transformCached = GetComponent<Transform> ();

		InputController.onJump += DoJump;
	}
	
	void DoJump (string option) {	

		if(isGrounded ()){
			rbCached.AddForce(Vector3.up * jumpForce,ForceMode2D.Impulse);
		}
		
	}
		
}
