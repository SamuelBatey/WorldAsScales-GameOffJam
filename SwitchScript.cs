using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour, IHasInteraction
{
    // Mesh Renderers of the switch's visuals
    [SerializeField]
    private MeshRenderer baseMeshRenderer;
    [SerializeField]
    private MeshRenderer leverMeshRenderer;

    [SerializeField]
    private GameObject outputObj;   // The object that is affected by this switch
    private IToggleable output;     // The actual script component of the object

    private int state;  // The current state the switch is in


    private void Start() {
        // Initialise the switch's state
        state = 0;

        // Get the toggleable script from the object
        if (outputObj.TryGetComponent<IToggleable>(out IToggleable outputComponent))
        {
            output = outputComponent;
        } else {
            Debug.LogError("Switch output is not toggleable!");
        }
    }

    public void Interact() {
        // Toggle the switch when interacted with
        if (state == 0)
        {
            ToggleOn();
        } else if (state == 1)
        {
            ToggleOff();
        }
    }

    // Turn on the highlight for the visuals
    public void EnableHighlight() {
        baseMeshRenderer.materials[1].SetFloat("_Scale", 1.1f);
        leverMeshRenderer.materials[1].SetFloat("_Scale", 1.1f);
    }

    // Turn off the highlight for the visuals
    public void DisableHighlight() {
        baseMeshRenderer.materials[1].SetFloat("_Scale", 0.3f);
        leverMeshRenderer.materials[1].SetFloat("_Scale", 0.3f);
    }

    private void ToggleOn() {
        // Change the state and update the visuals to reflect that
        state = 1;
        leverMeshRenderer.transform.rotation = Quaternion.AngleAxis(25f, Vector3.forward);  // Now that I'm thinking about this, this along with the
                                                                                            // enable and disable highlight stuff should probably be in it's own SwitchVisual script
        // Tell the output to do its thing
        output.ToggleOn();
    }

    private void ToggleOff() {
        // Change the state and update the visuals to reflect that
        state = 0;
        leverMeshRenderer.transform.rotation = Quaternion.AngleAxis(-25f, Vector3.forward);
        // Tell the output to do its thing
        output.ToggleOff();
    }
}
