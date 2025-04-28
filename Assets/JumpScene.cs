using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpScene : MonoBehaviour
{

    public void LoadSceneManager(int value) {

        SceneManager.LoadScene(value);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
   
}
