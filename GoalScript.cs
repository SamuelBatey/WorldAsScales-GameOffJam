using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    // The id of the scene this goal should load when touched by the player
    [SerializeField]
    private int nextSceneID;

    private void OnTriggerEnter(Collider coll) {
        // Check if the player was the one that entered the collider
        // Additional check for isTrigger to prevent the player's feet trigger from running this code a second time
        if (coll.attachedRigidbody.gameObject.tag == "Player" && coll.isTrigger == false)
        {
            SceneManager.LoadScene(nextSceneID);
        }
    }

}
