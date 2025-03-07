using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region Vars
    private Rigidbody2D rb;

    [Header("Stats")]
    public float MoveSpeed, JumpPower;

    public float coyoteTime = .2f;
    private float coyoteTimeCounter;
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    public bool Grounded = false;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        #region Jump
        //coyote time
        if (Grounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        //player has a buffer for jumping
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            print("Player tried to jump");
            jumpBufferCounter = jumpBufferTime;
        }
        else
            jumpBufferCounter -= Time.deltaTime;

        //if player presses space and we are still in coyote range, jump
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && Grounded)
        {
            print("Player Jumped");
            jumpBufferCounter = 0f;
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector2(MoveSpeed, 0));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector2(-MoveSpeed, 0));
        }
    }
}