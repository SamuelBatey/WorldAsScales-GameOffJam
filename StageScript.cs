using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageScript : MonoBehaviour
{
    
    [SerializeField]
    private float rotationStrength;     // Strength to apply to the rotation
    [SerializeField]
    private float rotSpeed;             // Speed at which the stage rotates
    [SerializeField]
    private float maxRot;               // Max amount the stage can rotate

    private List<IHasMass> massScriptsInTrigger;    // All the things with mass that are within the bounds of the stage

    private void Start() {
        // Initialise list of things with mass
        massScriptsInTrigger = new List<IHasMass>();
    }
    
    private void Update() {
        // Rotate the stage
        DoRotation();
    }

    private void DoRotation() {
        // Calculate where the stage should be rotated to
        Quaternion targetRot = Quaternion.AngleAxis(Mathf.Clamp(CalculateTotalWeight() * rotationStrength, maxRot * -1, maxRot), Vector3.back);

        // Actually rotate the damn thing
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed);
    }

    // Calculate the total weight of the objects in the stage
    private float CalculateTotalWeight() {
        float totalWeight = 0;

        foreach (IHasMass script in massScriptsInTrigger)
        {
            // Calculate it's distance from the fulcrum (which is at x = 0)
            float weight = script.GetMass() * script.GetGameObject().transform.position.x;
            // Add it to the weight
            totalWeight += weight;
        }

        // Since the total weight can be positive or negative, the sign indicates which side to rotate towards, i.e. a positive rotation from 0 or a negative rotation
        return totalWeight;
    }

    private void OnTriggerEnter(Collider coll) {
        // If something new enters the stage, add it to the list of stuff in the stage (unless its already there)
        if (coll.attachedRigidbody.gameObject.TryGetComponent<IHasMass>(out IHasMass massScript))
        {
            if (!massScriptsInTrigger.Contains(massScript))
            {
                massScriptsInTrigger.Add(massScript);
            }
        }
    }

    private void OnTriggerExit(Collider coll) {
        // If something exits the stage, remove it from the list of stuff in the stage (unless for some reason its not there to begin with)
        if (coll.attachedRigidbody.gameObject.TryGetComponent<IHasMass>(out IHasMass massScript))
        {
            if (massScriptsInTrigger.Contains(massScript))
            {
                massScriptsInTrigger.Remove(massScript);
            }
        }
    }
}
