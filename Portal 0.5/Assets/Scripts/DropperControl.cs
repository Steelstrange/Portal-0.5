using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperControl : MonoBehaviour
{
    #region Vars
    [Header("References")]
    [SerializeField] private GameObject ThingToSpawn;
    [SerializeField] private SpriteRenderer Top, Bars, Bottom;
    [SerializeField] private Sprite Open, Closed;
    [SerializeField] private Collider2D BottomCollider;
    #endregion

    public void Respawn()
    {
        //start coroutine to open the bottom (and mess with layers)
        StartCoroutine(RealRespawn());
    }

    IEnumerator RealRespawn()
    {
        yield return new WaitForEndOfFrame();
        if (this.gameObject != null)
        {
            //spawn in the thing
            GameObject Respawned = Instantiate(ThingToSpawn, Top.transform.position, Quaternion.identity);
            if (Respawned.name.Contains("Cube")) 
            {
                Respawned.GetComponent<CompanionCubeControl>().NameOfDropper = this.gameObject.name;
            }

            Respawned.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(ReEnableCollider(Respawned));

            yield return new WaitForSeconds(1);
            //disable the collider, change the sprite
            BottomCollider.enabled = false;
            Bottom.sprite = Open;

            yield return new WaitForSeconds(1);
            BottomCollider.enabled = true;
            Bottom.sprite = Closed;
        }

    }

    IEnumerator ReEnableCollider(GameObject thing)
    {
        yield return new WaitForSeconds(.4f);
        thing.GetComponent<Collider2D>().enabled = true;
    }
}
