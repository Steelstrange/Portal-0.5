using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    #region Vars
    private List<GameObject> ThingsOnButton = new List<GameObject>(); //keeps track of what is currently on the button
    [Header("References")]
    private BaseButtonable TheThing;
    [SerializeField] private SpriteRenderer sp;
    [SerializeField] private Sprite On, Off;


    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        //change the sprite
        sp.sprite = On;
        ThingsOnButton.Add(other.gameObject);

        //activate whatever the button is connected too
        TheThing.GetComponent<BaseButtonable>().OnActivate();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //print("Button is turned off");
        //change the sprite
        ThingsOnButton.Remove(other.gameObject);
        if(ThingsOnButton.Count == 0)
            sp.sprite = Off;

        //deactivate
        TheThing.GetComponent<BaseButtonable>().OnDeactivate();
    }
}
