using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanBePickedUp
{
    public PickupSO GetPickupSO();
    public void DestroySelf();
}
