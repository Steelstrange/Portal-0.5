using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseButtonable : MonoBehaviour
{
    public bool Activated; //if the object is on or not
    
    //what happens when the button this object is attached to is activated
    abstract public void OnActivate();

    //same thing but for de-activation
    abstract public void OnDeactivate();
}
