using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FizzlerControl : BaseButtonable
{
    #region Vars
    [Header("References")]
    [SerializeField] Collider2D FizzBox;
    [SerializeField] SpriteRenderer FizzRenderer;
    [SerializeField] Sprite On, Off;

    private bool Changed = false;
    #endregion

    public override void OnActivate()
    {
        if(Activated == false)
            Changed = true;
        Activated = true;
    }

    public override void OnDeactivate()
    {
        if(Activated == true)
            Changed = true;
        Activated = false;
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
            }
            else if(!Activated)
            {
                FizzBox.enabled = false;
                FizzRenderer.sprite = Off;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Fizzler fizzles");
        Destroy(collision.gameObject);
    }
}
