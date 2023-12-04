using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputScript : MonoBehaviour
{
    [SerializeField]
    private PlayerScript playerScript;
    
    // The move direction from the inputs: -1 for left, 1 for right, and 0 for no movement
    private int moveDir;
    
    private void Update() {
        //Set the move direction
        moveDir = 0;
        if (Input.GetKey(KeyCode.A))
        {
            moveDir = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir = 1;
        }
        //Tell the player to move in the given direction
        playerScript.Move(moveDir);

        // Check for jump input
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            playerScript.Jump();
        }

        // Check for interact input
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerScript.InteractAction();
        }
    }
}
