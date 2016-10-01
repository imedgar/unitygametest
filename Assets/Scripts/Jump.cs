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
	
	// Awake
	void Awake () {
		
		this.rbCached = GetComponent<Rigidbody2D> ();
		this.animationCached = GetComponent<Animator> ();
		this.colliderCached = GetComponent<BoxCollider2D> ();
		this.transformCached = GetComponent<Transform> ();

		InputController.onJump += DoJump;
	}
	
	void DoJump (string option) {		
		
		if(IsGrounded()){
			rbCached.AddForce(Vector3.up * jumpForce,ForceMode2D.Impulse);
			//animationCached.Play("jump");
		}
		
	}

    private bool IsGrounded()
    {

		Vector3 position = transformCached.position;
		position.y = colliderCached.bounds.min.y + 0.1f;
        hit = Physics2D.Raycast(position, Vector2.down, 0.2f, 1 << LayerMask.NameToLayer("Ground"));

        if (hit && !hit.collider.isTrigger)
        {
			if (hit.collider.tag == "Ground" || hit.collider.tag == "InnerZone")
            {
                return true;
            }
            else {
                return false;
            }
        }
        return false;
       
    }
		
}
