using UnityEngine;
using System.Collections;

public class Grounded : MonoBehaviour {

	private Rigidbody2D rbCached; 
	private BoxCollider2D colliderCached;
	private Transform transformCached;
	private RaycastHit2D hit;
	
	// Awake
	void Awake () {
		
		this.rbCached = GetComponent<Rigidbody2D> ();
		this.colliderCached = GetComponent<BoxCollider2D> ();
		this.transformCached = GetComponent<Transform> ();
		Jump.isGrounded += IsGrounded;
		Move.isGrounded += IsGrounded;
	}
	
	bool IsGrounded()
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
