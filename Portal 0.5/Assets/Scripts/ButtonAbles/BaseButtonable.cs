using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseButtonable : MonoBehaviour
{
    //what happens when the button this object is attached to is activated
    abstract public void OnActivate();

    //same thing but for de-activation
    abstract public void OnDeactivate();
}
