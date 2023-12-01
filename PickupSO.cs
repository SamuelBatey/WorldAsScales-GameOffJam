using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PickupSO : ScriptableObject
{
    public GameObject prefab;
    public GameObject visualPrefab;
    public float mass;
}
