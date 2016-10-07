using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	
	[SerializeField]
	private float speed;
	
	private Animator animationCached;
	
	public delegate bool OnAction();
	public static event OnAction isGrounded;
	
	// Awake
	void Awake ()
	{
		this.animationCached = GetComponent<Animator> ();
		InputController.onMove += Movement;
	}
	
	// The event that gets called
	void Movement(string direction)
	{
		transform.Translate(Vector3.right * speed * Time.deltaTime);
		if(isGrounded ()){
			animationCached.Play("run");
		} else {
			animationCached.Play("jump");
		}
	}
	
}
