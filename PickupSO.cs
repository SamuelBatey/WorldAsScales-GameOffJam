using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PickupSO : ScriptableObject
{
    // The prefab of the object related to this SO
    public GameObject prefab;
    // The prefab of the visuals related to this SO
    public GameObject visualPrefab;
    // The mass of the object of this SO
    public float mass;
}
