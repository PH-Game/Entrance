using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;
    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    private int jumpRestTimes;
    public float JumpSpeed;
    public int jumpMaxTimes;
    private BoxCollider2D playerBoxCollider;
    private LayerMask ground;
    public float dashCD, dashSpeed, dashLen;
    private float lastDash;
    public float hoverCD, hoverSpeed, hoverLen, JumpstartSpeed;
    private float lastHover, hoverStart;
    private bool isHovering;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
        ground = LayerMask.GetMask("Ground");
        jumpRestTimes = jumpMaxTimes;
        lastDash = (float) -1e6;
        lastHover = (float) -1e6;
        isHovering = false;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Flip();
        Jump();
        Dash();
        Hover();
    }

    bool CheckGround()
    {
        return playerBoxCollider.IsTouchingLayers(ground);
    }

    void Run()
    {
        if(Time.time - lastDash > dashLen && !isHovering)
        {
            float moveDir = Input.GetAxis("Horizontal");
            Vector2 playerVel = new Vector2(moveDir * runSpeed, playerRigidbody.velocity.y);
            playerRigidbody.velocity = playerVel;
            bool playerHasAxisSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
            //playerAnimator.SetBool("Run", playerHasAxisSpeed);
        }
    }

    void Flip()
    {
        bool playerHasAxisSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasAxisSpeed)
        {
            if (playerRigidbody.velocity.x > 0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (playerRigidbody.velocity.x < -0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    void Jump()
    {
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown("w")) && !isHovering)
        {
            if (CheckGround())
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

    float FacingDir()
    {
        if(transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            return 1;
        }
        if(transform.rotation == Quaternion.Euler(0, 180, 0))
        {
            return -1;
        }
        return -1;
    }

    void Dash()
    {
       if(Input.GetKeyDown(KeyCode.LeftShift) && Time.time - lastDash > dashCD)
       {
            lastDash = Time.time;
       }
       if(Time.time - lastDash < dashLen)
       {
            Vector2 playerVel = new Vector2(dashSpeed * FacingDir(), 0);
            playerRigidbody.velocity = playerVel;
       }
    }

    void Hover()
    {
        if(Input.GetKeyDown("c") && Time.time - lastHover > hoverCD)
        {
            isHovering = true;
            hoverStart = Time.time;
            if(CheckGround())
            {
                Vector2 playerVel = new Vector2(0, JumpstartSpeed);
                playerRigidbody.velocity = Vector2.up * playerVel;
            }
        }
        if(isHovering && Time.time - hoverStart < hoverLen && Time.time - hoverStart > 0.1)
        {
            lastHover = Time.time;
            if(CheckGround())
            {
                isHovering = false;
            }
            float xMoveDir = Input.GetAxis("Horizontal"), yMoveDir = Input.GetAxis("Vertical");
            Vector2 playerVel = new Vector2(xMoveDir * hoverSpeed, yMoveDir * hoverSpeed * 0.5f);
            playerRigidbody.velocity = playerVel;
        }
        if(Time.time - hoverStart > hoverLen)
        {
            isHovering = false;
        }
    }
}
