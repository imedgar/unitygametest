using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	
	[SerializeField]
	private float speed;
	
	private Animator animationCached;
	
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
		//animationCached.Play("run");
	}
	
}
