using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IHasMass
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float mass;
    [SerializeField]
    private float jumpHeight;           // Upward force that is applied to the player when jumping
    [SerializeField]
    private float fallSpeed;            // Downward force on the player when they are at the end of the jump arc, aka falling
                                        // This should be a positive number, it is later then multiplied by -1 when the force is applied
    [SerializeField]
    private float climbGravity;         // Downward force on the player when they are at the start of the jump arc, aka climbing in altitude
                                        // This should be a positive number, it is later then multiplied by -1 when the force is applied
    [SerializeField]
    private float maxDownwardVelocity;  // The loose cap on downward velocity so the fallSpeed doesnt make the player just keep going faster and faster
                                        // It isn't a hard cap on velocity, it just stops the fallSpeed force from being applied after the player's downward
                                        // velocity has hit this limit
                                        // This should be a negative number, it is tested against the player's y velocity
    [SerializeField]
    private float maxSlopeHeight;       // This is the additional distance the movement capsule cast should be off the ground so that the player can go up slopes
    [SerializeField]
    private LayerMask layerMask;        // Layermask used for the movement capsule cast
    [SerializeField]
    private LayerMask interactableLayerMask;    // Layermask used when casting for interactable objects in front of the player


    [SerializeField]
    private CapsuleCollider capsuleColl;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Transform hands;    // The transform of the player's hands, a.k.a where to instantiate the visual of the object the player is holding
    [SerializeField]
    private Transform stageParentTransform;


    private PickupSO pickedupSO;
    private bool isOnGround;
    private IHasInteraction highlightedObj; // Stores the object that was last highlighted so that it can be un-highlighted when the interactable cast no longer detects anything


    public void Move(int moveDir) {
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = capsuleColl.radius * 0.4f;     // Multiplied by 0.4 becuase thats the scale of the player object in the game world
        float playerHeight = (capsuleColl.height * 0.4f) - maxSlopeHeight;
        float sphereCenterOffset = (playerHeight / 2f) - playerRadius;  // Distance from the center of the player to center of the spheres on the top and bottom of the player collider

        // Rotate the player object in the direction of movement
        if (moveDir != 0)
        {
            transform.forward = Vector3.right * moveDir * moveDistance;
        }

        // Do a capsule cast in front of the player to see if they can move to where they want to, if they can't then don't move
        bool canMove = !Physics.CapsuleCast(transform.position + Vector3.up * sphereCenterOffset, transform.position - Vector3.up * sphereCenterOffset, playerRadius, Vector3.right * moveDir, moveDistance, layerMask);
        if (canMove)
        {
            transform.position += Vector3.right * moveDir * moveDistance;
        }
    }

    public void Jump() {
        // Reset the player's vertical velocity
        // Prevents a bug where the player could glitch into a platform and use the momentum from the physics correction to jump heigher
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        // Check player is on the ground
        if (isOnGround)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    public float GetMass() {
        // Check player is on the ground
        if (isOnGround)
        {
            // If the player is holding something, add it's mass to the player's mass
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

    // The player's feet is a box collider around the bottom of the player's capsule collider, detecting ground this way allows for coyote time
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

    // Prevents a bug where the player would pick up the interactable wieght, the feet would detect it leaving it's trigger and would sent isOnGround to
    // false, even though the player was still on the ground
    private void OnTriggerStay(Collider coll) {
        if (coll.gameObject.tag == "Floor")
        {
            isOnGround = true;
        }
    }

    // Called when the Interact input is pressed
    public void InteractAction() {
        // Send out a cast for anything in front of the player that can be interacted with
        float reachDist = capsuleColl.radius * 0.4f * 3;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, reachDist, interactableLayerMask))
        {
            // If it can be interacted with, interact with it
            if (hitInfo.rigidbody.gameObject.TryGetComponent<IHasInteraction>(out IHasInteraction interactable))
            {
                interactable.Interact();
            }

            // If it can be picked up, pick it up
            if (hitInfo.rigidbody.gameObject.TryGetComponent<ICanBePickedUp>(out ICanBePickedUp pickupable))
            {
                PickupObj(pickupable);
            }
        } else {
            // If there aren't any interactables in front, and the player is holding something, drop it
            if (pickedupSO != null)
            {
                DropObj(reachDist);
            }
        }
    }

    public void PickupObj(ICanBePickedUp obj) {
        // Get the SO of the object to be picked up
        pickedupSO = obj.GetPickupSO();

        // Instantiate its visuals onto the players hands
        GameObject instantiatedObj = Instantiate(pickedupSO.visualPrefab, hands.position, Quaternion.identity, hands.transform);
        // Disable its collider and scale it down
        instantiatedObj.GetComponent<BoxCollider>().enabled = false;
        instantiatedObj.transform.localScale = Vector3.one * 0.4f;

        // Destroy the original object
        obj.DestroySelf();
    }

    public void DropObj(float reachDist) {
        // Instantiate the object in front of the player, clear the pickedupSO variable and destroy the visuals attached to the players hands
        Instantiate(pickedupSO.prefab, transform.position + transform.forward * reachDist, stageParentTransform.rotation, stageParentTransform);
        pickedupSO = null;
        Destroy(hands.GetChild(0).gameObject);
    }

    private void FixedUpdate() {
        // Apply the forces various forces related to jumping
        if (rb.velocity.y < 0 && rb.velocity.y > maxDownwardVelocity)
        {
            rb.AddForce(Vector3.down * fallSpeed);
        }

        if (rb.velocity.y > 0)
        {
            rb.AddForce(Vector3.down * climbGravity);
        }
    }

    private void Update() {
        // Cast in front of the player for any interactables
        float reachDist = capsuleColl.radius * 0.4f * 3;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, reachDist, interactableLayerMask))
        {
            // If you find an interactable and nothing is currently highlighted, then highlight it
            if (hitInfo.rigidbody.gameObject.TryGetComponent<IHasInteraction>(out IHasInteraction interactable) && highlightedObj == null)
            {
                interactable.EnableHighlight();
                highlightedObj = interactable;
            }
        } else {
            // If the ray doesn't hit anything and something is currently highlighted, then un-highlight it
            if (highlightedObj != null)
            {
                highlightedObj.DisableHighlight();
                highlightedObj = null;
            }
        }
    }
}
