using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public Button statButton;
    public Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenURL(string user)
    {
        Application.OpenURL(user);
    }

    public void OnOffPanel(GameObject name)
    {
        if (name.activeSelf == true)
            name.SetActive(false);
        else
            name.SetActive(true);
    }

    public void OnStart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
