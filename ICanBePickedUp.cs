using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface to be used on items that can be picked up such as the interactable weights
public interface ICanBePickedUp
{
    public PickupSO GetPickupSO();
    public void DestroySelf();
}
