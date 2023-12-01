using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IHasMass
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private CapsuleCollider capsuleColl;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float jumpHeight;

    [SerializeField]
    private Transform hands;

    private PickupSO pickedupSO;

    private bool isOnGround;

    [SerializeField]
    private Transform stageParentTransform;

    [SerializeField]
    private float maxSlopeHeight;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private LayerMask interactableLayerMask;

    [SerializeField]
    private float mass;

    [SerializeField]
    private float fallSpeed;

    [SerializeField]
    private float climbGravity;

    [SerializeField]
    private float maxDownwardVelocity;

    private IHasInteraction highlightedObj;

    public void Move(int moveDir) {
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = capsuleColl.radius * 0.4f;
        float playerHeight = (capsuleColl.height * 0.4f) - maxSlopeHeight;
        float sphereCenterOffset = (playerHeight / 2f) - playerRadius;

        if (moveDir != 0)
        {
            transform.forward = Vector3.right * moveDir * moveDistance;
        }

        bool canMove = !Physics.CapsuleCast(transform.position + Vector3.up * sphereCenterOffset, transform.position - Vector3.up * sphereCenterOffset, playerRadius, Vector3.right * moveDir, moveDistance, layerMask);
        if (canMove)
        {
            transform.position += Vector3.right * moveDir * moveDistance;
            //transform.position += Vector3.ProjectOnPlane(Vector3.right*moveDir*moveDistance, groundNormal);
        }
    }

    public void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        if (isOnGround)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    public float GetMass() {
        if (isOnGround)
        {
            if (pickedupSO)
            {
                return mass + pickedupSO.mass;
            }
            return mass;
        } else {
            return 0f;
        }
    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    private void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Floor")
        {
            isOnGround = true;
        }
    }

    private void OnTriggerExit(Collider coll) {
        if (coll.gameObject.tag == "Floor")
        {
            isOnGround = false;
        }
    }

    private void OnTriggerStay(Collider coll) {
        if (coll.gameObject.tag == "Floor")
        {
            isOnGround = true;
        }
    }

    public void InteractAction() {
        float reachDist = capsuleColl.radius * 0.4f * 3;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, reachDist, interactableLayerMask))
        {
            if (hitInfo.rigidbody.gameObject.TryGetComponent<IHasInteraction>(out IHasInteraction interactable))
            {
                interactable.Interact();
            }
            if (hitInfo.rigidbody.gameObject.TryGetComponent<ICanBePickedUp>(out ICanBePickedUp pickupable))
            {
                PickupObj(pickupable);
            }
        } else {
            if (pickedupSO != null)
            {
                DropObj(reachDist);
            }
        }
    }

    public void PickupObj(ICanBePickedUp obj) {
        pickedupSO = obj.GetPickupSO();

        GameObject instantiatedObj = Instantiate(pickedupSO.visualPrefab, hands.position, Quaternion.identity, hands.transform);
        instantiatedObj.GetComponent<BoxCollider>().enabled = false;
        instantiatedObj.transform.localScale = Vector3.one * 0.4f;

        obj.DestroySelf();
    }

    public void DropObj(float reachDist) {
        Instantiate(pickedupSO.prefab, transform.position + transform.forward * reachDist, stageParentTransform.rotation, stageParentTransform);
        pickedupSO = null;
        Destroy(hands.GetChild(0).gameObject);
    }

    private void FixedUpdate() {
        if (rb.velocity.y < 0 && rb.velocity.y > maxDownwardVelocity)
        {
            rb.AddForce(Vector3.down * fallSpeed);
        }

        if (rb.velocity.y > 0)
        {
            rb.AddForce(Vector3.down * climbGravity);
        }

        //Debug.Log(rb.velocity.y);
    }

    private void Update() {
        float reachDist = capsuleColl.radius * 0.4f * 3;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, reachDist, interactableLayerMask))
        {
            if (hitInfo.rigidbody.gameObject.TryGetComponent<IHasInteraction>(out IHasInteraction interactable) && highlightedObj == null)
            {
                interactable.EnableHighlight();
                highlightedObj = interactable;
            }
        } else {
            if (highlightedObj != null)
            {
                highlightedObj.DisableHighlight();
                highlightedObj = null;
            }
        }
    }
}
