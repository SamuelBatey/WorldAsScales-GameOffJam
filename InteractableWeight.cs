using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableWeight : MonoBehaviour, IHasMass, ICanBePickedUp, IHasInteraction
{
    [SerializeField]
    private Rigidbody rb;

    private bool isOnGround;

    [SerializeField]
    private PickupSO pickupSO;

    [SerializeField]
    private MeshRenderer visualMeshRenderer;
    
    public float GetMass() {
        if (isOnGround)
        {
            //return rb.mass;
            return pickupSO.mass;
        } else {
            return 0f;
        }
    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    private void OnCollisionEnter(Collision coll) {
        if (coll.collider.gameObject.tag == "Floor")
        {
            isOnGround = true;
        }
    }

    private void OnCollisionExit(Collision coll) {
        if (coll.collider.gameObject.tag == "Floor")
        {
            isOnGround = false;
        }
    }

    public PickupSO GetPickupSO() {
        return pickupSO;
    }

    public void DestroySelf() {
        transform.position = Vector3.up * 1000;
        Destroy(gameObject, 0.1f);
    }

    public void EnableHighlight() {
        visualMeshRenderer.materials[1].SetFloat("_Scale", 1.1f);
    }

    public void DisableHighlight() {
        visualMeshRenderer.materials[1].SetFloat("_Scale", 0.1f);
    }

    public void Interact() {
        return;
    }
}
