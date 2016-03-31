using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public enum PlayerActions
    {
        MOVE_FORWARD,
        MOVE_BACKWARD,
        JUMP,
        SHIELD,
		BEND
    }

    //Variables
    [SerializeField]
    float speed;
    private float initialSpeed;
	private float speedRecorder;
	private float speedCap;
    float acceleration;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    public List<GameObject> prefabs;
    [SerializeField]
    GameObject shield;

    float weaponCoolDown;
    [SerializeField]
    float shieldCoolDown;
    [SerializeField]
    float shieldDuration;
    float timeStampShield;

    RaycastHit2D hit;

    // Player RigidBody Reference
    Rigidbody2D rb;

    // Test Stuff 
    [SerializeField]
    bool freeMove;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shield.SetActive(false);
        acceleration = 0.002f;
		speedCap = 11.5f;
        initialSpeed = GameManager.Instance.naturalWorldSpeed;
		speedRecorder = GameManager.Instance.naturalWorldSpeed;
    }

    void Update()
    {
        Behaviour("Update", GameManager.Instance.currentState);
    }

    void FixedUpdate()
    {
        Behaviour("FixedUpdate", GameManager.Instance.currentState);
    }

    void Behaviour(string whichUpdate, GameManager.GameStates currentGameState)
    {
		// Handles all game states
        if (GameManager.Instance.CanStartGameLogic())
        {
            switch (currentGameState)
            {
                case GameManager.GameStates.Mainmenu:
                    break;
				case GameManager.GameStates.InnerZone:
                case GameManager.GameStates.Roofs:
                    // Update Loop Stuff
                    if (whichUpdate.Equals("Update"))
                    {
                        platformControllerActions(PlayerActions.JUMP);
                    }
                    // FixedUpdate Loop Stuff
                    else if (whichUpdate.Equals("FixedUpdate"))
                    {
                        //Transition();
						// Movement Stuff
                        if (freeMove)
                        {
                            platformControllerActions(PlayerActions.MOVE_FORWARD);
                            platformControllerActions(PlayerActions.MOVE_BACKWARD);
                        }
                        else {
                            //Movement(PlayerActions.MOVE_FORWARD);
                        }
						// Other actions
                        platformControllerActions(PlayerActions.SHIELD);
                        //platformControllerActions(PlayerActions.BEND);

                        // Update score

						GameManager.Instance.playerSpeed = speed;
                    }
                    break;
                case GameManager.GameStates.Street:
                        Transition();
                    break;			
                default:
                    Debug.Log("Unaccepted command!");
                    break;
            }
        }
    }

    private void platformControllerActions(PlayerActions action)
    {
		// Device platform controllers
        switch (GameManager.Instance.platform)
        {
            case RuntimePlatform.Android:
			case RuntimePlatform.IPhonePlayer:
                foreach (Touch touch in Input.touches)
                {
                    if (touch.position.x > Screen.width / 2 && action.Equals(PlayerActions.SHIELD))
                    {
                        UseShield();
                    }
                    if (touch.position.x < Screen.width / 2 && action.Equals(PlayerActions.JUMP))
                    {
                        Jump();
                    }
                }
                break;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsWebPlayer:
                if (Input.GetKey(KeyCode.R) && action.Equals(PlayerActions.SHIELD))
                {
                    UseShield();
                }
                if (Input.GetKey(KeyCode.W) && action.Equals(PlayerActions.JUMP))
                {
                    Jump();
                }
                if (Input.GetKey(KeyCode.A) && action.Equals(PlayerActions.MOVE_BACKWARD))
                {
                    Movement(PlayerActions.MOVE_BACKWARD);
                }
                if (Input.GetKey(KeyCode.D) && action.Equals(PlayerActions.MOVE_FORWARD))
                {
                    Movement(PlayerActions.MOVE_FORWARD);
                }
			    if (Input.GetKey(KeyCode.S) && action.Equals(PlayerActions.BEND))
                {
                    Bend ();
                }
                break;
            default:
                break;
        }

    }
	
	// Movement function *Backward and Forward*
    void Movement(PlayerActions direction)
    {
        if (direction.Equals(PlayerActions.MOVE_BACKWARD))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (direction.Equals(PlayerActions.MOVE_FORWARD))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
			if (speed < speedCap){
            	speed += acceleration;
			}
        }
    }
	
	// Jump function
    void Jump()
    {
        if (IsGrounded())
        {
            //rb.AddForce(Vector3.up * jumpForce,ForceMode2D.Impulse);
            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
        }
    }
	
	// Shield function
    private void UseShield()
    {
		// Shield if cooldown allows
        if (timeStampShield <= Time.time)
        {
            timeStampShield = Time.time + shieldCoolDown;
            shield.SetActive(true);
			// Switch player state
            GameManager.Instance.currentPlayerState = GameManager.PlayerStates.Shielded;
            Invoke("StopShielding", shieldDuration);
        }
    }

    private void StopShielding()
    {
		// Stop Shielding
        shield.SetActive(false);
        GameManager.Instance.currentPlayerState = GameManager.PlayerStates.Running;
    }
	
	private void Bend (){
		transform.Rotate (Vector3.forward * -90);
	}
	
	// Transition action to streets
    private void Transition()
    {
        if (GameManager.Instance.playerTransition == 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * 4);
            GameManager.Instance.playerTransition++;
            speed = initialSpeed;
        }
    }
	
	// Collision Events
    void OnCollisionEnter2D(Collision2D coll)
    {
        CollisionEvents(coll);
    }
	
	// Inner Zone detection
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "InnerZone")
        {
			if(GameManager.Instance.playerEnteredInnerZone){
				GameManager.Instance.playerEnteredInnerZone = false;
				if(GameManager.Instance.naturalWorldSpeed > 8.2f){
                    GameManager.Instance.naturalWorldSpeed = speedRecorder;
				}
				}
			else {
				GameManager.Instance.playerEnteredInnerZone = true;
				speedRecorder = GameManager.Instance.naturalWorldSpeed;
				if(GameManager.Instance.naturalWorldSpeed > initialSpeed * 1.2f){
                    GameManager.Instance.naturalWorldSpeed = initialSpeed * 1.2f;
				}
			}
        }
    }

    void CollisionEvents(Collision2D coll)
    {
        if (coll.gameObject.tag == "DeathDetector")
        {
			GameManager.Instance.GameRestart ();
        }
		if (coll.gameObject.tag == "Obstacle")
        {
			GameManager.Instance.GameRestart ();
        }
    }
	
	// Grounded Check *Debug option drawray*
    private bool IsGrounded()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down, 0.65f, 1 << LayerMask.NameToLayer("Ground"));
        if (hit && !hit.collider.isTrigger)
        {
            if (hit.collider.tag == "Ground" || hit.collider.tag == "Street" || hit.collider.tag == "InnerZone")
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
		
}
