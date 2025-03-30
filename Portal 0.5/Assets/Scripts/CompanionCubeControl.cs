using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionCubeControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DropperControl Dropper; //what dropper we need to trigger
    public string NameOfDropper; 
    private void Awake()
    {
        Dropper = GameObject.Find(NameOfDropper).GetComponent<DropperControl>();
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
