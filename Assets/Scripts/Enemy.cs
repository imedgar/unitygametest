﻿using UnityEngine;
using System.Collections;

abstract public class Enemy : MonoBehaviour {

	//Variables
	[SerializeField]
	protected float speed;
	[SerializeField]
	protected float jumpForce;
	[SerializeField]
	protected float visionRange;

	protected float initialSpeed;
	protected bool canJump;
	protected RaycastHit2D hit;

    // Enemy RigidBody Reference
    protected Rigidbody2D rb;
	// Player GameObject Reference
	protected GameObject playerRef;
	// Camera reference
	protected GameObject camRef;

	// Use this for initialization
	protected void Start () {
		initialSpeed = speed;
		rb = GetComponent<Rigidbody2D>();
		playerRef = GameObject.FindGameObjectWithTag ("Player");
		camRef = GameObject.Find("Main Camera");
	}
	
	// Physics stuff
	protected void Update () {
		Behaviour ("Update", GameManager.Instance.currentState);
	}

	// Movement
	protected void FixedUpdate(){
        transform.Translate(Vector3.left * GameManager.Instance.naturalWorldSpeed * Time.deltaTime);
        Behaviour ("FixedUpdate", GameManager.Instance.currentState);
	}
	
	protected void OnCollisionEnter2D(Collision2D coll) {
		Collided (coll);
	}
	
	protected void KillEnemy(){
		speed = initialSpeed;
		ObjectPool.instance.PoolObject(gameObject);
	}
	protected void BackToPool(){
		if ((camRef.transform.position.x) - (transform.position.x + 0.1) >= 25)
		{
			KillEnemy ();
		}
	}

	protected bool PlayerInRange () {
		if ((transform.position.x - playerRef.transform.position.x) < visionRange) {
			return true;
		} else {
			return false;
		}	
	}

	protected void MoveForward (){
		transform.Translate ( Vector3.left * speed * Time.deltaTime );
	}

	protected void MoveBackward (){
		transform.Translate ( Vector3.right * speed * Time.deltaTime );
	}
	
	protected bool IsGrounded(){
		hit = Physics2D.Raycast(transform.position, Vector2.down, 0.65f, 1 << LayerMask.NameToLayer("Ground"));
        if (hit) {     
			if(hit.collider.tag == "Ground" || hit.collider.tag == "InnerZone")
            {
				return true;
			}
			else {
				return false;
			}  
        } 
		return false;
        //Debug.DrawRay(transform.position, Vector2.down, Color.red, 0.65f);
	}

	// Unimplemented Methods
	protected virtual void Movement(){}
	protected virtual void Collided(Collision2D coll){}
	protected virtual void Behaviour(string whichUpdate, GameManager.GameStates currentGameState){}
	protected virtual void PhysicsStuff(){}
}
