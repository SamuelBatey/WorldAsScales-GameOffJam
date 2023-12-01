using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TogglePlatformScript : MonoBehaviour, IToggleable
{
    [SerializeField]
    private Transform point1;

    [SerializeField]
    private Transform point2;

    [SerializeField]
    private Transform platformTransform;

    [SerializeField]
    private float speed;

    private Vector3 target;
    
    public void ToggleOn() {
        target = point2.localPosition;
    }

    public void ToggleOff() {
        target = point1.localPosition;
    }

    private void Start() {
        platformTransform.localPosition = point1.localPosition;
        target = platformTransform.localPosition;
    }

    private void Update() {
        if (platformTransform.localPosition != target)
        {
            float step = speed * Time.deltaTime;
            platformTransform.localPosition = Vector3.MoveTowards(platformTransform.localPosition, target, step);
            Debug.Log("Moving");
        }
        if (Vector3.Distance(point1.localPosition, platformTransform.localPosition) < 0.001f)
        {
            platformTransform.localPosition = point1.localPosition;
        }
    }
}
