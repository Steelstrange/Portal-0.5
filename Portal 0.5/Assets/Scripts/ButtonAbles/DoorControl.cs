using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorControl : BaseButtonable
{
    #region Vars
    [Header("References")]
    [SerializeField] SpriteRenderer sp;
    [SerializeField] Sprite Open, Closed;
    [SerializeField] Scene NextLevel;

    #region In-House vars 
    private bool Changed = false;
    private bool PlayerInRange = false;
    #endregion

    #endregion

    public override void OnActivate()
    {
        if(base.Activated == false)
            Changed = true;
        base.Activated = true;
    }

    public override void OnDeactivate()
    {
        if(base.Activated == true)
            Changed = true;
        base.Activated = false;
    }

    private void Update()
    {
        //if we changed status, update sprite
        if(Changed)
        {
            Changed = false;
            
            if (Activated)
            {
                sp.sprite = Open;
            }
            else if(!Activated)
            {
                sp.sprite = Closed;
            }
        }

        //if the player presses E, enter the door (load next level)
        if(PlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            print("Only if you have dlc ;)");
            //SceneManager.LoadScene(NextLevel.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerInRange = false;
    }
}
