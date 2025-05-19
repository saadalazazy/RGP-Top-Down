using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        MusicManager.Instance.PlayMusic("MainMenu");
    }
    public void Play()
    {
        SceneManager.LoadScene("Level1");
        MusicManager.Instance.PlayMusic("Level1");
    }
    public void Mainmenu()
    {
        SceneManager.LoadScene("MainMenu");
        MusicManager.Instance.PlayMusic("MainMenu");
    }
    public void Quit()
    {
        Application.Quit();
    }
}