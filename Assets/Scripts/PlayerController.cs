using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;
    private Rigidbody2D playerRigidbody;
    //private Animator playerAnimator;
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

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        //playerAnimator = GetComponent<Animator>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
        ground = LayerMask.GetMask("Ground");
        jumpRestTimes = jumpMaxTimes;
        lastDash = (float) -1e6;
        lastHover = (float) -1e6;
        isHovering = false;
    }

    void Update()
    {
        Run();
        Flip();
        Jump();
        Dash();
        Hover();

        if (playerRigidbody.position.y < -3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    bool CheckGround()
    {
        return playerBoxCollider.IsTouchingLayers(ground);
    }

    void Run()
    {
        if(Time.time - lastDash > dashLen && !isHovering)
        {
            // Get Horizontal Direction
            float moveDir = Input.GetAxis("Horizontal");
            // Construct Movement Vector
            Vector2 playerVel = new Vector2(moveDir * runSpeed, playerRigidbody.velocity.y);
            // Set Velocity
            playerRigidbody.velocity = playerVel;

            //bool playerHasAxisSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
            //playerAnimator.SetBool("Run", playerHasAxisSpeed);
        }
    }

    void Flip()
    {
        bool playerHasAxisSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasAxisSpeed)
        {
            // If Face Right
            if (playerRigidbody.velocity.x > 0.1f)
            {
                // Set Sprite Direction to Right
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            // If Face Left
            if (playerRigidbody.velocity.x < -0.1f)
            {
                // Set Sprite Direction to Left
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    void Jump()
    {
        // If Pressed `Space` or `w`
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown("w")) && !isHovering)
        {
            // If Player On Ground
            if (CheckGround())
            {
                // Reset Jumped Rest Time
                jumpRestTimes = jumpMaxTimes;
            }

            // If Still Remain Jump Rest Time
            if (jumpRestTimes != 0)
            {
                // Set Vertical Speed (Jump Speed)
                Vector2 jumpVel = new Vector2(0.0f, JumpSpeed);
                // Set Vertical Velocity
                playerRigidbody.velocity = Vector2.up * jumpVel;
                // Minus Jump Rest Time
                jumpRestTimes--;
            }
        }
    }

    // Get Facing Direction
    float FacingDir()
    {
        if(transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            return 1;
        }

        return -1;
    }

    void Dash()
    {
       // If Pressed Left Shift and not in CD
       if(Input.GetKeyDown(KeyCode.LeftShift) && Time.time - lastDash > dashCD)
       {
            // Set Dash Time to Now
            lastDash = Time.time;
       }

       // If Not In CD
       if(Time.time - lastDash < dashLen)
       {
            // Plus a Horizontal Dash Velocity
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
