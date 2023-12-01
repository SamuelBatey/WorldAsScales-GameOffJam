using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoosePlatformScript : MonoBehaviour, IHasMass
{
    [SerializeField]
    private float mass;

    
    public float GetMass() {
        return mass;
    }

    public GameObject GetGameObject() {
        return gameObject;
    }
    
}
