using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface to be used on objects that can be interacted with such as the switch
public interface IHasInteraction
{
    public void Interact();

    public void EnableHighlight();

    public void DisableHighlight();
}
