using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    #region Vars
    [Header("References")]
    [SerializeField] SpriteRenderer sp;
    [SerializeField] Sprite Off, Launched;

    [Header("Stats")]
    [SerializeField] Vector2 ForceToAdd;
    [SerializeField] float Cooldown, Offset;


    private float Timer = 0;
    #endregion

    private void Update()
    {
        Timer += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //when something enters the trigger, launch tf out of it
        if(Timer > Cooldown)
        {
            //if its the player we're bouncing, check if player is holding crtl
            if (other.gameObject.name.Contains("Player") && Input.GetKey(KeyCode.LeftControl))
            {
                //do nothing
            }
            else
            {
                print("This shit fired");
                Timer = 0;
                other.GetComponent<Rigidbody2D>().AddForce(ForceToAdd);

                //change sprite, start coroutine to reset it
                sp.sprite = Launched; //the change in position caused by the  laudifferent sprite is .28
                transform.position = new Vector2(transform.position.x, transform.position.y + Offset);
                StartCoroutine(ResetSprite());
            }
        }
    }


    IEnumerator ResetSprite()
    {
        yield return new WaitForSeconds(Cooldown/1.5f);
        sp.sprite = Off;
        transform.position = new Vector2(transform.position.x, transform.position.y - Offset);
    }
}
