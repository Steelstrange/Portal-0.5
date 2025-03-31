using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{
    #region Vars
    public bool IsBlue;
    public bool IsVert;
    public string Direction;

    public static List<GameObject> NoTeleport = new List<GameObject>();
    #endregion

    public void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag != "NoTeleport" && other.gameObject.tag != "Ground" && NoTeleport.Contains(other.gameObject) != true)
        {
            Teleport(other.gameObject);
        }
    }

    IEnumerator NoTele(GameObject other)
    {
        NoTeleport.Add(other);
        yield return new WaitForSeconds(.2f);
        NoTeleport.Remove(other);
    }

    public string FindTpType(Portal other)
    {
        string r;
        if (other.Direction == "Left" && Direction == "Right" || other.Direction == "Right" && Direction == "Left" || other.Direction == "Up" && Direction == "Down" || other.Direction == "Down" && Direction == "Up")
            r = "Normal";

        else if (other.Direction == Direction)
            r = "Opposite";

        else
            r = "Perp";
        return r;
    }

    public void Teleport(GameObject other)
    {
        
        //print("Collided with something");
        Portal OtherPortal = null;
        //if blue, find orange
        if (IsBlue)
        {
            OtherPortal = GameObject.FindGameObjectWithTag("OrangePortal").GetComponent<Portal>();
        }
        //else, if orange, find blue
        else if (!IsBlue)
        {
            OtherPortal = GameObject.FindGameObjectWithTag("BluePortal").GetComponent<Portal>();
        }

        //teleport to the transform of the other portal
        StartCoroutine(NoTele(other.gameObject));


        //three types of possible transitions:
        //normal; just change position, dont need to mess with velocity
        //opposite; Reverse our momentum
        //perpendicular; flip the y an d x momentum
        string TpType = FindTpType(OtherPortal);
        //print(TpType);
        //print(OtherPortal.name);

        //find the vector between the center of the portal and whatever is teleporting]
        Vector3 Offset = FindOffset(other, OtherPortal);
        other.transform.position = OtherPortal.transform.position + Offset;

        if (TpType == "Normal")
        {
            
        }

        else if (TpType == "Opposite")
        {
            if (!IsVert)
            {
                other.GetComponent<Rigidbody2D>().velocity = new Vector2(-other.GetComponent<Rigidbody2D>().velocity.x, other.GetComponent<Rigidbody2D>().velocity.y );
            }
            else
            {
                other.GetComponent<Rigidbody2D>().velocity = new Vector2(other.GetComponent<Rigidbody2D>().velocity.x, -other.GetComponent<Rigidbody2D>().velocity.y);
            }
        }

        else if (TpType == "Perp")
        {
            if (OtherPortal.IsVert)
            {
                if (OtherPortal.Direction == "Down")
                {
                    other.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.y /2), -Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.x)); 
                    //other.transform.position -= (Vector3)new Vector2(Offset.x, Mathf.Abs(Offset.y));
                }
                else
                { 
                    other.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.y), Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.x));
                    //other.transform.position -= (Vector3)new Vector2(Offset.y, Offset.x);
                }
            }
            else
            {
                if (OtherPortal.Direction == "Right")
                {
                    //other.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.y), Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.x));
                    other.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.y), 0);
                    //other.transform.position -= (Vector3)new Vector2(Offset.y, Offset.x);
                }
                else
                {
                    other.GetComponent<Rigidbody2D>().velocity = new Vector2(-Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.y), 0);
                    //other.transform.position -= (Vector3)new Vector2(Offset.y, Offset.x);
                }
            }
        }
    }

    private Vector3 FindOffset(GameObject other, Portal OtherPortal)
    {
        Vector3 Offset = Vector3.zero;

        if (OtherPortal.Direction == "Left")
            Offset = new Vector3(-.4f, -.68f, 0);

        else if (OtherPortal.Direction == "Right")
            Offset = new Vector3(.4f, -.68f, 0);

        else if (OtherPortal.Direction == "Up")
            Offset = new Vector3(0, .1f, 0);

        else if (OtherPortal.Direction == "Down")
            Offset = new Vector3(0, -1.5f, 0);
        return Offset;
    }
}