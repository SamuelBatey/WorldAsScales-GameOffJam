using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour, IHasInteraction
{
    [SerializeField]
    private MeshRenderer baseMeshRenderer;
    [SerializeField]
    private MeshRenderer leverMeshRenderer;

    [SerializeField]
    private GameObject outputObj;
    private IToggleable output;

    private int state;


    private void Start() {
        state = 0;
        if (outputObj.TryGetComponent<IToggleable>(out IToggleable outputComponent))
        {
            output = outputComponent;
        } else {
            Debug.LogError("Switch output is not toggleable!");
        }
    }

    public void Interact() {
        if (state == 0)
        {
            ToggleOn();
        } else if (state == 1)
        {
            ToggleOff();
        }
    }

    public void EnableHighlight() {
        baseMeshRenderer.materials[1].SetFloat("_Scale", 1.1f);
        leverMeshRenderer.materials[1].SetFloat("_Scale", 1.1f);
    }

    public void DisableHighlight() {
        baseMeshRenderer.materials[1].SetFloat("_Scale", 0.3f);
        leverMeshRenderer.materials[1].SetFloat("_Scale", 0.3f);
    }

    private void ToggleOn() {
        state = 1;
        Debug.Log("Toggled On");
        leverMeshRenderer.transform.rotation = Quaternion.AngleAxis(25f, Vector3.forward);
        output.ToggleOn();
    }

    private void ToggleOff() {
        state = 0;
        Debug.Log("Toggled Off");
        leverMeshRenderer.transform.rotation = Quaternion.AngleAxis(-25f, Vector3.forward);
        output.ToggleOff();
    }
}
