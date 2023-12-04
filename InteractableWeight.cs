using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableWeight : MonoBehaviour, IHasMass, ICanBePickedUp, IHasInteraction
{
    private bool isOnGround;

    // The scriptable object that corresponds to this object
    [SerializeField]
    private PickupSO pickupSO;

    // The mesh renderer of the weight visual
    [SerializeField]
    private MeshRenderer visualMeshRenderer;
    
    public float GetMass() {
        // Only return the mass if the weight is touching the ground, that way the stage's rotation won't be affected by the weight if it isn't on the ground
        if (isOnGround)
        {
            return pickupSO.mass;
        } else {
            return 0f;
        }
    }

    // Need this since I cant just do IHasMass.gameObject
    public GameObject GetGameObject() {
        return gameObject;
    }

    // Collision detection for floor
    private void OnCollisionEnter(Collision coll) {
        if (coll.collider.gameObject.tag == "Floor")
        {
            isOnGround = true;
        }
    }

    // Collision detection for floor
    private void OnCollisionExit(Collision coll) {
        if (coll.collider.gameObject.tag == "Floor")
        {
            isOnGround = false;
        }
    }

    // Return the SO that corresponds to this interactable weight
    public PickupSO GetPickupSO() {
        return pickupSO;
    }

    // Code that destroys the weight when it is, for example, picked up
    public void DestroySelf() {
        // Teleports outside of the stage before deleting to avoid an error where it would
        // delete itself while the stage is in the middle of running its code to determine weight distribution of objects
        transform.position = Vector3.up * 1000;
        Destroy(gameObject, 0.1f);
    }

    // Enables the highlight by changing the scale of the effect in the shadergraph, giving the appearance of an outline
    public void EnableHighlight() {
        visualMeshRenderer.materials[1].SetFloat("_Scale", 1.1f);
    }

    // Disables the highlight by changing the scale of the effect in the shadergraph so the outline shrinks to be fully obcured by the object itself, thus no longer being visible
    public void DisableHighlight() {
        visualMeshRenderer.materials[1].SetFloat("_Scale", 0.1f);
    }

    // Since picking up objects is handled by the player script, the interactable weight has not code to execute
    // on interact, however it still needs to be here because of the interface
    public void Interact() {
        // If a pickup sound or some other effect were to be added to the game, it would be placed here
        return;
    }
}
