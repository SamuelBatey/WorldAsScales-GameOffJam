using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasInteraction
{
    public void Interact();

    public void EnableHighlight();

    public void DisableHighlight();
}
