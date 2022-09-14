using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public float speed;
    public float jumpAcc;
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
        bool movementY = Input.GetButtonDown("Jump");
        rigidbody.velocity = new Vector2(movementX * speed, rigidbody.velocity.y + isJump(movementY) * jumpAcc);
    }

    float isJump(bool jump)
    {
        if(jump) return 1;
        else return 0;
    }
}
