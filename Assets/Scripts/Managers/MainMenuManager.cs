using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public AudioSource buttonClickSound;

    public void LoadGame()
    {
        this.buttonClickSound.Play();
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        this.buttonClickSound.Play();
        Application.Quit();
    }
}
