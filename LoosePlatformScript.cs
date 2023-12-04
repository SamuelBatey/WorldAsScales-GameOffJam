using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoosePlatformScript : MonoBehaviour, IHasMass
{
    [SerializeField]
    private float mass;

    // Return the mass of this object
    public float GetMass() {
        return mass;
    }

    // Return the GameObject of this object
    public GameObject GetGameObject() {
        return gameObject;
    }
    
}
