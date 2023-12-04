using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerScript : MonoBehaviour
{
    [SerializeField]
    private int nextSceneID;
    
    // Function that is activated by the Start button on the main menu
    public void OnStartClick() {
        SceneManager.LoadScene(nextSceneID);
    }

    // Function that is activated by the Quit button on the main menu
    // In the final web build, the Quit button is disabled because it doesnt make sense to put a quit button
    // in the web app, if the user wants to quit, they will close the browser window or tab
    public void OnQuitClick() {
        Application.Quit();
    }
}
