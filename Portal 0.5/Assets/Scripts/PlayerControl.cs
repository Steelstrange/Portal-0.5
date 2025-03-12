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

    public float AirTimer;
    [SerializeField] private float MaxFallSpeed;
    public bool Grounded = false;   

    [SerializeField] private LayerMask PortalLayer;
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
        #endregion
        #region Better Portaling
        if(rb.velocity.magnitude > 10)
        {
            //if we're moving fast enough, cast a ray down and up to check if we're doing the classic portal thing
            RaycastHit2D Down = Physics2D.Raycast(transform.position, -Vector2.up, 1, PortalLayer);
            if(Down)
            {
                //print("Hell yeah");
                //print(Down.transform.position);
                if(Down.distance < .5) //if we're close to the portal downwards, teleport early
                {
                    print("Name: " + Down.distance);
                    Down.transform.gameObject.GetComponent<Portal>().Teleport(gameObject);
                }
            }
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

        //increase the velocity on the way down
        if (rb.velocity.y < 0f)
        {
            //print(Mathf.Clamp(Mathf.Pow(AirTimer, FallMult), 0, MaxFallSpeed));
            rb.velocity -= new Vector2(0, Mathf.Clamp(Mathf.Pow(AirTimer, FallMult), 0, MaxFallSpeed));
        }
    }
}