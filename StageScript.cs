using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageScript : MonoBehaviour
{
    private List<IHasMass> massScriptsInTrigger;

    [SerializeField]
    private float rotationStrength;

    [SerializeField]
    private float rotSpeed;

    [SerializeField]
    private float maxRot;

    private void Start() {
        massScriptsInTrigger = new List<IHasMass>();
    }
    
    private void Update() {
        DoRotation();
        //Debug.Log(massScriptsInTrigger.Count);
    }

    private void DoRotation() {
        Quaternion targetRot = Quaternion.AngleAxis(Mathf.Clamp(CalculateTotalWeight() * rotationStrength, maxRot * -1, maxRot), Vector3.back);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed);
    }

    private float CalculateTotalWeight() {
        float totalWeight = 0;

        foreach (IHasMass script in massScriptsInTrigger)
        {
            float weight = script.GetMass() * script.GetGameObject().transform.position.x;
            totalWeight += weight;
        }
        //Debug.Log("Total Weight: " + totalWeight);
        return totalWeight;
    }

    private void OnTriggerEnter(Collider coll) {
        //Debug.Log(coll.attachedRigidbody.gameObject.tag);
        if (coll.attachedRigidbody.gameObject.TryGetComponent<IHasMass>(out IHasMass massScript))
        {
            if (!massScriptsInTrigger.Contains(massScript))
            {
                massScriptsInTrigger.Add(massScript);
            } else {
                Debug.Log("Cannot add script, list already has mass script");
            }
        }
    }

    private void OnTriggerExit(Collider coll) {
        Debug.Log("OnTriggerExit");
        if (coll.attachedRigidbody.gameObject.TryGetComponent<IHasMass>(out IHasMass massScript))
        {
            if (massScriptsInTrigger.Contains(massScript))
            {
                massScriptsInTrigger.Remove(massScript);
            } else {
                Debug.Log("Cannot remove script, list does not have the mass script");
            }
        }
    }
}
