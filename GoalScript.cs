using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    [SerializeField]
    private int nextSceneID;

    private void OnTriggerEnter(Collider coll) {
        if (coll.attachedRigidbody.gameObject.tag == "Player" && coll.isTrigger == false)
        {
            SceneManager.LoadScene(nextSceneID);
        }
    }

}
