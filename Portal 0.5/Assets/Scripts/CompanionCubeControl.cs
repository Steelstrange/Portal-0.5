using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionCubeControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DropperControl Dropper; //what dropper we need to trigger
    private void Awake()
    {
        Dropper = GameObject.Find("CubeDropper").GetComponent<DropperControl>();
    }
    public void Respawn()
    {
        Dropper.Respawn();
    }

    private void OnDestroy()
    {
        if(Dropper != null)
            Dropper.Respawn();
    }
}
