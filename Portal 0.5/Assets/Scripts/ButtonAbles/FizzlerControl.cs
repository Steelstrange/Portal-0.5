using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FizzlerControl : BaseButtonable
{
    #region Vars
    [Header("References")]
    [SerializeField] Collider2D FizzBox;
    [SerializeField] SpriteRenderer FizzRenderer, FizzRenderer2;
    [SerializeField] Sprite On, Off;

    private bool Changed = false;
    #endregion

    public override void OnActivate()
    {
        if(Activated == true)
            Changed = true;
        Activated = false;
    }

    public override void OnDeactivate()
    {
        if(Activated == false)
            Changed = true;
        Activated = true;
    }

    private void Update()
    {
        if(Changed)
        {
            //update the sprite and the fizbox
            if(Activated)
            {
                FizzBox.enabled = true;
                FizzRenderer.sprite = On;
                FizzRenderer2.sprite = On;
            }
            else if(!Activated)
            {
                FizzBox.enabled = false;
                FizzRenderer.sprite = Off;
                FizzRenderer2.sprite = Off;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 8) //the players layer
        {
            //reset the players portals
            print("Fizzles the player's portals");
            if (GameObject.FindGameObjectWithTag("BluePortal") != null)
                Destroy(GameObject.FindGameObjectWithTag("BluePortal"));

            if (GameObject.FindGameObjectWithTag("OrangePortal") != null)
                Destroy(GameObject.FindGameObjectWithTag("OrangePortal"));

            //check if player is holding something
            if (PlayerControl.Main.PickedUpThing != null)
            {
                print("Fizzling held object");
                Destroy(PlayerControl.Main.PickedUpThing);
            }
        }

        else
        {
            print("Fizzler fizzles");
            Destroy(other.gameObject);
        }
    }
}
