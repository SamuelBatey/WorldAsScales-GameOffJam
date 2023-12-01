using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerScript : MonoBehaviour
{
    [SerializeField]
    private int nextSceneID;
    
    public void OnStartClick() {
        SceneManager.LoadScene(nextSceneID);
    }

    public void OnQuitClick() {
        Application.Quit();
    }
}
