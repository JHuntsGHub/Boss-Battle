using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MenuOptions contains various methods that are called by buttons in menus.
public class MenuOptions : MonoBehaviour
{
    //The Start method makes the cursor visible and ensures the cursor is not locked. 
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //PlayClicked() loads the main gameplay scene.
    public void PlayClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("2. BossBattle");
    }

    // ControlsClicked() loads the controls info scene.
    public void ControlsClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("4. Controls");
    }

    //BackClicked() Loads the main menu scene.
    public void BackClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("1. MainMenu");
    }

    // QuitClicked() quits the application completely.
    public void QuitClicked()
    {
        Application.Quit();
    }
}
