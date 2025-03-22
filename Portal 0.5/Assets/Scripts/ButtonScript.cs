using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    #region Vars
    [Header("References")]
    [SerializeField] private SpriteRenderer sp;
    [SerializeField] private Sprite On, Off;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //change the sprite
        sp.sprite = On;

        //activate whatever the button is connected too
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        print("Button is turned off");
        //change the sprite
        sp.sprite = Off;

        //deactivate
    }
}
