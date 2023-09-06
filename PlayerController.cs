using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Player Components
    private Rigidbody2D PlayerRigidBody;
    private Animator PlayerAnimator;
    private Collider2D PlayerCollider2D;

    // States
    private enum State {idle, running, jumping, falling}
    private State state = State.idle;

    // Layer Mask
    [SerializeField] private LayerMask Map;

    // Player Attributes
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float playerJumpForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        PlayerCollider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        UpdateState();
        PlayerAnimator.SetInteger("state", (int)state); // sets animation based on state
    }

    private void Movement()
    {
        // Horizontal Movement
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            PlayerRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * playerSpeed, PlayerRigidBody.velocity.y);
            transform.localScale = new Vector2(Input.GetAxisRaw("Horizontal"), 1);

        }
        // Jump Movement
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && PlayerCollider2D.IsTouchingLayers(Map)) //Input.GetButtonDown("Jump") is the non hard-coded version but i kinda like having both space and w as jump buttons.
        {
            PlayerRigidBody.velocity = new Vector2(PlayerRigidBody.velocity.x, playerJumpForce);
            state = State.jumping;
        }
    }

    private void UpdateState()
    {
        
        if(state == State.jumping)
        {
            //Once player is falling, state is changed to falling
            if(PlayerRigidBody.velocity.y < 0.1f)
            {
                state = State.falling;
            }
            print("Jumping");
        } else if (state == State.falling)
        {
            //If player is falling and lands on map, state is hanged to idle
            if (PlayerCollider2D.IsTouchingLayers(Map))
            {
                state = State.idle;
            }
            print("Falling");
        } else if(Mathf.Abs(PlayerRigidBody.velocity.x) > 2f )
        {
            //Moving
            state = State.running;
            print("Running");
        } else
        {
            state = State.idle;
            print("Idle");
        }
    }
}
