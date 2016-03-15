using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public enum PlayerActions
    {
        MOVE_FORWARD,
        MOVE_BACKWARD,
        JUMP,
        SHIELD
    }

    //Variables
    [SerializeField]
    float speed;
    private float initialSpeed;
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
        acceleration = 0.001f;
        initialSpeed = speed;
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
                        platformControllerActions(PlayerActions.JUMP);
                    }
                    // FixedUpdate Loop Stuff
                    else if (whichUpdate.Equals("FixedUpdate"))
                    {
                        //Transition();
                        if (freeMove)
                        {
                            platformControllerActions(PlayerActions.MOVE_FORWARD);
                            platformControllerActions(PlayerActions.MOVE_BACKWARD);
                        }
                        else {
                            Movement(PlayerActions.MOVE_FORWARD);
                        }

                        platformControllerActions(PlayerActions.SHIELD);

                        // Update score
                        GameManager.Instance.score = transform.position.x;
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
        else {
            GameManager.Instance.ActiveGameLogic();
        }
    }

    private void platformControllerActions(PlayerActions action)
    {

        switch (GameManager.Instance.platform)
        {
            case RuntimePlatform.Android:
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
            case RuntimePlatform.IPhonePlayer:
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
                break;
            default:
                break;
        }

    }

    void Movement(PlayerActions direction)
    {
        if (direction.Equals(PlayerActions.MOVE_BACKWARD))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (direction.Equals(PlayerActions.MOVE_FORWARD))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            speed += acceleration;
        }
    }

    void Jump()
    {
        if (IsGrounded())
        {
            //rb.AddForce(Vector3.up * jumpForce,ForceMode2D.Impulse);
            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
        }
    }

    private void UseShield()
    {
        if (timeStampShield <= Time.time)
        {
            timeStampShield = Time.time + shieldCoolDown;
            shield.SetActive(true);
            GameManager.Instance.currentPlayerState = GameManager.PlayerStates.Shielded;
            Invoke("StopShielding", shieldDuration);
        }
    }

    private void StopShielding()
    {
        shield.SetActive(false);
        GameManager.Instance.currentPlayerState = GameManager.PlayerStates.Running;
    }

    private void Transition()
    {
        if (GameManager.Instance.playerTransition == 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * 4);
            GameManager.Instance.playerTransition++;
            speed = initialSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        CollisionEvents(coll);
    }

    void CollisionEvents(Collision2D coll)
    {
        if (coll.gameObject.tag == "DeathDetector")
        {
            GameManager.Instance.currentState = GameManager.GameStates.Mainmenu;
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    private bool IsGrounded()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down, 0.65f, 1 << LayerMask.NameToLayer("Ground"));
        if (hit)
        {
            if (hit.collider.tag == "Ground" || hit.collider.tag == "Street")
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
