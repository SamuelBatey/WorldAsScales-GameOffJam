using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface to be used on objects that have a mass that can affect the stage's rotation
public interface IHasMass
{
    public float GetMass();
    public GameObject GetGameObject();
}
