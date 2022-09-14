using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public float speed;
    public float jumpSpeed;
    public BoxCollider2D collider2D;
    public LayerMask ground;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float movementX = Input.GetAxis("Horizontal");
        float faceDirection = Input.GetAxisRaw("Horizontal");
        bool movementY = false;

        if (collider2D.IsTouchingLayers(ground))
        {
            movementY = Input.GetButtonDown("Jump");
        }

        rigidbody.velocity = new Vector2(movementX * speed, rigidbody.velocity.y + isJump(movementY) * jumpSpeed);
        transform.localScale = new Vector3(faceDirection, 1, 1);
    }

    float isJump(bool jump)
    {
        if(jump) return 1;
        else return 0;
    }
}
