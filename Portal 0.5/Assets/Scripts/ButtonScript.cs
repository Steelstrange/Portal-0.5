using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonScript : MonoBehaviour
{
    #region Vars
    private List<GameObject> ThingsOnButton = new List<GameObject>(); //keeps track of what is currently on the button
    [Header("References")]
    [SerializeField] private List<GameObject> TheThings = new List<GameObject>(); //what the button turns on and off
    [SerializeField] private SpriteRenderer sp;
    [SerializeField] private Sprite On, Off;
    [SerializeField] private AudioSource aud;


    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        //change the sprite
        sp.sprite = On;
        ThingsOnButton.Add(other.gameObject);
        aud.Play();

        //activate whatever the button is connected too
        //TheThing.GetComponent<BaseButtonable>().OnActivate();
        for(int i = 0; i < TheThings.Count; i++)
        {
            TheThings[i].GetComponent<BaseButtonable>().OnActivate();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //print("Button is turned off");
        //change the sprite
        ThingsOnButton.Remove(other.gameObject);
        if (ThingsOnButton.Count == 0)
        {
            sp.sprite = Off;

            //deactivate
            //TheThing.GetComponent<BaseButtonable>().OnDeactivate();
            for (int i = 0; i < TheThings.Count; i++)
            {
                TheThings[i].GetComponent<BaseButtonable>().OnDeactivate();
            }
        }
    }
}
