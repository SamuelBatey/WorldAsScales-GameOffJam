using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TogglePlatformScript : MonoBehaviour, IToggleable
{
    [SerializeField]
    private float speed;        // Speed at which the platform moves
    [SerializeField]
    private Transform point1;   // The starting point of the platform
    [SerializeField]
    private Transform point2;   // The end point of the platform

    [SerializeField]
    private Transform platformTransform;    // The transform of the platform

    private Vector3 target;     // The target the platform should move towards
    

    public void ToggleOn() {
        // Change the target position
        target = point2.localPosition;
    }

    public void ToggleOff() {
        // Change the target position
        target = point1.localPosition;
    }

    private void Start() {
        // Initialise the start position and the target
        // All position stuff is done in local space, otherwise the platform wont rotate correctly with the stage
        platformTransform.localPosition = point1.localPosition;
        target = platformTransform.localPosition;
    }

    private void Update() {
        // If the platform isn't at the target, then move it towards the target
        if (platformTransform.localPosition != target)
        {
            float step = speed * Time.deltaTime;
            platformTransform.localPosition = Vector3.MoveTowards(platformTransform.localPosition, target, step);
        }

        // If the platform is close enough, then snap it to the target
        if (Vector3.Distance(point1.localPosition, platformTransform.localPosition) < 0.001f)
        {
            platformTransform.localPosition = point1.localPosition;
        }
    }
}
