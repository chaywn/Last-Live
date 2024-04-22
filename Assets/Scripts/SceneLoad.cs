using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public void LoadGameScene()
    {
        // Destroy the background music before transitioning into the game scene
        // so it doesn't get retained into the next scene
        if (DontDestroy.instance != null) {
            Destroy(DontDestroy.instance.gameObject);
            DontDestroy.instance = null;
        }
        
        SceneManager.LoadScene("Game");
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene("Title");
    }

    public void LoadWeaponScene()
    {
        SceneManager.LoadScene("Weapon");
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
