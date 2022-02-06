using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool paused = false;

    public GameObject pauseMenu;
    public GameObject gui;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        gui.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {


        if(Input.GetKeyDown("escape"))
        {
            if(paused)
            {
                paused = false;
                pauseMenu.SetActive(false);
                gui.SetActive(true);
            }
            else
            {
                paused = true;
                pauseMenu.SetActive(true);
                gui.SetActive(false);
            }
        }
    }
    public void CloseWindow(GameObject window)
    {
        window.SetActive(false);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("MainMenuManager");
    }
}
