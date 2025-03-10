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
    [SerializeField] private float FallMult;

    private float AirTimer;
    [SerializeField] private float MaxFallSpeed;
    public Vector2 OldVelocity;
    
    public bool Grounded = false;   


    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        StartCoroutine(OldVelocityUpdater());
        #region Jump
        //coyote time
        if (Grounded)
        {
            coyoteTimeCounter = coyoteTime;
            AirTimer = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            AirTimer += Time.deltaTime;
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

        //increase the velocity on the way down
        if (rb.velocity.y < 0f)
        {
            //print(Mathf.Clamp(Mathf.Pow(AirTimer, FallMult), 0, MaxFallSpeed));
            rb.velocity -= new Vector2(0, Mathf.Clamp(Mathf.Pow(AirTimer, FallMult), 0, MaxFallSpeed));
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

    IEnumerator OldVelocityUpdater()
    {
        yield return null; //wait one frame
        OldVelocity = rb.velocity;
        print(OldVelocity);
    }
}