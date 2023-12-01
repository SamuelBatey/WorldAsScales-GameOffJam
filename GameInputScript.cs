using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputScript : MonoBehaviour
{
    [SerializeField]
    private PlayerScript playerScript;
    
    private int moveDir;
    
    private void Update() {
        moveDir = 0;
        if (Input.GetKey(KeyCode.A))
        {
            moveDir = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir = 1;
        }

        playerScript.Move(moveDir);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            playerScript.Jump();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerScript.InteractAction();
        }
    }
}
