using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    #region Vars
    [SerializeField] protected bool IsBlue;
    #endregion

    public void OnTriggerEnter2D(Collider2D other)
    {
        print("Collided with something");
        if(other.gameObject.tag != "NoTeleport")
        {
            GameObject OtherPortal = null;
            //if blue, find orange
            if(IsBlue)
            {
                OtherPortal = GameObject.FindGameObjectWithTag("OrangePortal");
            }
            //else, if orange, find blue
            else if(!IsBlue)
            {
                OtherPortal = GameObject.FindGameObjectWithTag("BluePortal");
            }

            //teleport to the transform of the other portal
            StartCoroutine(NoTele(other.tag, other.gameObject));
            other.transform.position = OtherPortal.transform.position;

        }
    }

    IEnumerator NoTele(string OGtag, GameObject other)
    {
        other.tag = "NoTeleport";
        yield return new WaitForSeconds(.3f);
        other.tag = OGtag;
    }
}