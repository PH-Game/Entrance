using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;
    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    public int jumpRestTimes;
    public float JumpSpeed;
    public int jumpMaxTimes;
    private BoxCollider2D playerBoxCollider;
    private LayerMask ground;
    private bool isGround;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
        ground = LayerMask.GetMask("Ground");
        jumpRestTimes = jumpMaxTimes;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Flip();
        Jump();
    }

    void CheckGround()
    {
        isGround =  playerBoxCollider.IsTouchingLayers(ground);
    }

    void Run()
    {
        float moveDir = Input.GetAxis("Horizontal");
        Vector2 playerVel = new Vector2(moveDir * runSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = playerVel;
        bool playerHasAxisSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
        //playerAnimator.SetBool("Run", playerHasAxisSpeed);


    }
    void Flip()
    {
        bool playerHasAxisSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasAxisSpeed)
        {
            if (playerRigidbody.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (playerRigidbody.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            CheckGround();

            if (isGround)
            {
                jumpRestTimes = jumpMaxTimes;
            }

            if (jumpRestTimes != 0)
            {
                Vector2 jumpVel = new Vector2(0.0f, JumpSpeed);
                playerRigidbody.velocity = Vector2.up * jumpVel;
                jumpRestTimes--;
            }
        }
    }
}
