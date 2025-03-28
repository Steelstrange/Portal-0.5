using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCheck : MonoBehaviour
{
    PlayerControl PC;

    private void Awake()
    {
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Ground")
            PC.Grounded = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //if we leave ground, we're no longer grounded (crazy I know)
        if (other.tag == "Ground")
        {
            PC.Grounded = false;
        }
    }
}
