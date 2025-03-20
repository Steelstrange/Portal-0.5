using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region Vars
    private Rigidbody2D rb;
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject PortalGunPos;
    [SerializeField] private Transform LeftPickup, RightPickup;
    private GameObject PickedUpThing;

    [Header("Stats")]
    public float MoveSpeed;
    public float AirMoveSpeed, JumpPower;
    private float OGMoveSpeed;
    public float coyoteTime = .2f;
    private float coyoteTimeCounter;
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    [SerializeField] private float FallMult;
    [SerializeField] private float PickupRange;

    [HideInInspector] public float AirTimer;
    [SerializeField] private float MaxFallSpeed;
    public bool Grounded = false;
    public bool HasBlue, HasOrange;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask PortalLayer;
    [SerializeField] private LayerMask CanPortalOn, PortalCheck, Pickup;

    [Header("Prefabs")]
    [SerializeField] private GameObject BlueDown;
    [SerializeField] private GameObject BlueLeft, BlueRight, BlueUp, OrgDown, OrgLeft, OrgRight, OrgUp;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        OGMoveSpeed = MoveSpeed;
    }

    private void Update()
    {
        #region Jump
        //coyote time
        if (Grounded)
        {
            coyoteTimeCounter = coyoteTime;
            AirTimer = 0;
            MoveSpeed = OGMoveSpeed;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            AirTimer += Time.deltaTime;
            MoveSpeed = AirMoveSpeed;
        }

        //player has a buffer for jumping
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            //print("Player tried to jump");
            jumpBufferCounter = jumpBufferTime;
        }
        else
            jumpBufferCounter -= Time.deltaTime;

        //if player presses space and we are still in coyote range, jump
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && Grounded)
        {
            //print("Player Jumped");
            jumpBufferCounter = 0f;
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
        }
        #endregion

        #region Better Portaling
        if (rb.velocity.y < -1)
        {
            //if we're moving fast enough, cast a ray down and up to check if we're doing the classic portal thing
            RaycastHit2D Down = Physics2D.Raycast(transform.position, -Vector2.up, 1, PortalLayer);
            if (Down)
            {
                //print("Hell yeah");
                //print(Down.transform.position);
                if (Down.distance < .5) //if we're close to the portal downwards, teleport early
                {
                    //print("Name: " + Down.transform.name);
                    Down.transform.gameObject.GetComponent<Portal>().Teleport(gameObject);
                }
            }
        }
        #endregion

        //dear lord help me
        #region Portal Gun
        if (Input.GetKeyDown(KeyCode.Mouse0) && HasBlue)
        {
            //find mouse position and shoot a ray towards it
            Vector2 MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            //print(MousePos);

            RaycastHit2D hit = Physics2D.Raycast(PortalGunPos.transform.position, MousePos - (Vector2)PortalGunPos.transform.position, 18, CanPortalOn);
            //Debug.DrawRay(PortalGunPos.transform.position, MousePos -  (Vector2) PortalGunPos.transform.position, Color.black);
            //print(hit.transform.gameObject.name);

            //if we hit something we can make a portal on, find its name
            //based on name, create a portal
            if (hit)
            {
                //if theres already anothe blue portal MURDER IT IN COLD BLOOD
                if (GameObject.FindWithTag("BluePortal") != null)
                    Destroy(GameObject.FindWithTag("BluePortal"));

                //do some math
                int x = Mathf.RoundToInt(hit.point.x);
                int y = Mathf.RoundToInt(hit.point.y);

                //create portal
                if (hit.transform.name.Contains("Floor"))
                {
                    //check to the left and right for anything to block the portal
                    RaycastHit2D left = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(0, .5f, 0), Vector2.left, .999f, PortalCheck);
                    RaycastHit2D right = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(0, .5f, 0), Vector2.right, .999f, PortalCheck);

                    if (!left && !right)
                        Instantiate(BlueUp, new Vector3(x, y, 0), Quaternion.identity);
                    else
                        print("Portal was blocked");
                }
                else if (hit.transform.name.Contains("Left"))
                {
                    RaycastHit2D up = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(.5f, 0, 0), Vector2.up, .999f, PortalCheck);
                    RaycastHit2D down = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(.5f, 0, 0), Vector2.down, .999f, PortalCheck);

                    if (!up && !down)
                        Instantiate(BlueRight, new Vector3(x, y, 0), Quaternion.identity);
                    else
                        print("Portal was blocked");
                }
                else if (hit.transform.name.Contains("Right"))
                {
                    RaycastHit2D up = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(-.5f, 0, 0), Vector2.up, .999f, PortalCheck);
                    RaycastHit2D down = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(-.5f, 0, 0), Vector2.down, .999f, PortalCheck);

                    if (!up && !down)
                        Instantiate(BlueLeft, new Vector3(x, y, 0), Quaternion.identity);
                    else
                        print("Portal was blocked");
                }
                else if (hit.transform.name.Contains("Down"))
                {
                    RaycastHit2D left = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(0, -.5f, 0), Vector2.left, .999f, PortalCheck);
                    RaycastHit2D right = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(0, -.5f, 0), Vector2.right, .999f, PortalCheck);

                    if (!left && !right)
                        Instantiate(BlueDown, new Vector3(x, y, 0), Quaternion.identity);
                    else
                        print("Portal was blocked");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasOrange)
        {
            //find mouse position and shoot a ray towards it
            Vector2 MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            //print(MousePos);

            RaycastHit2D hit = Physics2D.Raycast(PortalGunPos.transform.position, MousePos - (Vector2)PortalGunPos.transform.position, 18, CanPortalOn);
            //Debug.DrawRay(PortalGunPos.transform.position, MousePos -  (Vector2) PortalGunPos.transform.position, Color.black);
            //print(hit.transform.gameObject.name);

            //if we hit something we can make a portal on, find its name
            //based on name, create a portal
            if (hit)
            {
                //if theres already anothe blue portal MURDER IT IN COLD BLOOD
                if (GameObject.FindWithTag("OrangePortal") != null)
                    Destroy(GameObject.FindWithTag("OrangePortal"));

                //do some math
                int x = Mathf.RoundToInt(hit.point.x);
                int y = Mathf.RoundToInt(hit.point.y);

                //create portal
                if (hit.transform.name.Contains("Floor"))
                {//check to the left and right for anything to block the portal
                    RaycastHit2D left = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(0, .5f, 0), Vector2.left, .999f, PortalCheck);
                    RaycastHit2D right = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(0, .5f, 0), Vector2.right, .999f, PortalCheck);

                    if (!left && !right)
                        Instantiate(OrgUp, new Vector3(x, y, 0), Quaternion.identity);
                    else
                        print("Portal was blocked");
                }
                else if (hit.transform.name.Contains("Left"))
                {
                    RaycastHit2D up = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(.5f, 0, 0), Vector2.up, .999f, PortalCheck);
                    RaycastHit2D down = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(.5f, 0, 0), Vector2.down, .999f, PortalCheck);

                    if (!up && !down)
                        Instantiate(OrgRight, new Vector3(x, y, 0), Quaternion.identity);
                    else
                        print("Portal was blocked");
                }
                else if (hit.transform.name.Contains("Right"))
                {
                    RaycastHit2D up = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(-.5f, 0, 0), Vector2.up, .999f, PortalCheck);
                    RaycastHit2D down = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(-.5f, 0, 0), Vector2.down, .999f, PortalCheck);

                    if (!up && !down)
                        Instantiate(OrgLeft, new Vector3(x, y, 0), Quaternion.identity);
                    else
                        print("Portal was blocked");
                }
                else if (hit.transform.name.Contains("Down"))
                {
                    RaycastHit2D left = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(0, -.5f, 0), Vector2.left, .999f, PortalCheck);
                    RaycastHit2D right = Physics2D.Raycast(new Vector3(x, y, 0) + new Vector3(0, -.5f, 0), Vector2.right, .999f, PortalCheck);

                    if (!left && !right)
                        Instantiate(OrgDown, new Vector3(x, y, 0), Quaternion.identity);
                    else
                        print("Portal was blocked");
                }
            }
        }
        #endregion
        //wasnt too bad

        //this on the other hand
        #region Picking things up
        //if player presses E, shoot a ray out a bit and see if there is an object nearby
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (PickedUpThing == null)
            {
                //find mouse position and shoot a ray towards it
                Vector2 MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(PortalGunPos.transform.position, MousePos - (Vector2)PortalGunPos.transform.position, PickupRange, Pickup);

                //if we hit something, pick it up
                if (hit)
                {
                    PickedUpThing = hit.transform.gameObject;
                    PickedUpThing.GetComponent<Collider2D>().enabled = false;
                    PickedUpThing.GetComponent<Rigidbody2D>().simulated = false;
                    PickedUpThing.transform.rotation = Quaternion.identity;
                    PickedUpThing.transform.parent = this.gameObject.transform;
                }
                //if we didnt hit anything, maybe search around a bit
                else
                {
                    //naw nevermind, only if i have time
                }
            }
            else
            {
                //drop the thing we have picked up
                PickedUpThing.GetComponent<Collider2D>().enabled = true;
                PickedUpThing.GetComponent<Rigidbody2D>().simulated = true;
                PickedUpThing.transform.parent = null;
                PickedUpThing = null;
            }
        }

        if (PickedUpThing != null)
        {
            if(rb.velocity.x > 0)
            {
                PickedUpThing.transform.position = RightPickup.position;
            }
            else if(rb.velocity.x < 0)
            {
                PickedUpThing.transform.position = LeftPickup.position;
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