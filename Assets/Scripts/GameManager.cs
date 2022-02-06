using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public bool paused = false;

    public GameObject pauseMenu;
    public GameObject gui;

    public bool won = false;
    public bool lost = false;

    public GameObject wonMenu;
    public GameObject lostMenu;

    public FollowTarget camScript;

    float endGameTimer = 0;
    float endGameTime = 5;

    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        gui.SetActive(true);
        wonMenu.SetActive(false);
        lostMenu.SetActive(false);

        music.Play();
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

        if(won)
        {
            wonMenu.SetActive(true);
            camScript.StartEndGame();
        }

        if(lost)
        {
            lostMenu.SetActive(true);
            camScript.StartEndGame();
        }

        if(won || lost)
        {
            endGameTimer += Time.deltaTime;

            if(endGameTimer >= endGameTime)
            {
                SceneManager.LoadScene("MainMenuManager");
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
