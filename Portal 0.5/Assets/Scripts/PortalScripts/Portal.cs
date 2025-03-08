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
    #endregion


    public void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag != "NoTeleport" && other.gameObject.tag != "Ground")
        {
            print("Collided with something");
            Portal OtherPortal = null;
            //if blue, find orange
            if(IsBlue)
            {
                OtherPortal = GameObject.FindGameObjectWithTag("OrangePortal").GetComponent<Portal>();
            }
            //else, if orange, find blue
            else if(!IsBlue)
            {
                OtherPortal = GameObject.FindGameObjectWithTag("BluePortal").GetComponent<Portal>();
            }

            //teleport to the transform of the other portal
            StartCoroutine(NoTele(other.tag, other.gameObject)); 


            //three types of possible transitions:
            //normal; just change position, dont need to mess with velocity
            //opposite; Reverse our momentum
            //perpendicular; flip the y an d x momentum
            string TpType = FindTpType(OtherPortal);
            //print(TpType);
            //print(OtherPortal.name);

            //find the vector between the center of the portal and whatever is teleporting
            Vector2 Offset = transform.position - other.transform.position;
            //print(Offset);

            if(TpType == "Normal")
            {
                other.transform.position = OtherPortal.transform.position;
                other.transform.position -=  (Vector3) new Vector2(-Offset.x, Offset.y);
            }

            else if(TpType == "Opposite")
            {
                other.transform.position = OtherPortal.transform.position;
                other.transform.position -= (Vector3)new Vector2(Offset.x, Offset.y);

                if (!IsVert)
                    other.GetComponent<Rigidbody2D>().velocity = new Vector2(-other.GetComponent<Rigidbody2D>().velocity.x, other.GetComponent<Rigidbody2D>().velocity.y);
                else
                    other.GetComponent<Rigidbody2D>().velocity = new Vector2(other.GetComponent<Rigidbody2D>().velocity.x, -other.GetComponent<Rigidbody2D>().velocity.y);
            }

            else if(TpType == "Perp")
            {

            }
        }
    }

    IEnumerator NoTele(string OGtag, GameObject other)
    {
        other.tag = "NoTeleport";
        yield return new WaitForSeconds(.2f);
        other.tag = OGtag;
    }

    public string FindTpType(Portal other)
    {
        string r = null;
        if (other.Direction == "Left" && Direction == "Right" || other.Direction == "Right" && Direction == "Left" || other.Direction == "Up" && Direction == "Down" || other.Direction == "Down" && Direction == "Up")
            r = "Normal";

        else if (other.Direction == Direction)
            r = "Opposite";

        else
            r = "Perp";
        return r;
    }
}