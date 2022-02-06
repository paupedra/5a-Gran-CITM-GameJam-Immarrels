using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Assistant : MonoBehaviour
{
    [SerializeField] private TextWriter textWriter;
    private Text messageText;

    private void Awake()
    {
        messageText = transform.Find("WelcomePanel").Find("UI Assistant").Find("message").Find("WelcomeText").GetComponent<Text>();
    }

    private void Start()
    {
        textWriter.AddWriter(messageText, "Welcome to Tamara.\nMove with WASD and recollect resources with SpaceBar.\nOnce you have collected all resources spend them to unlock the next tile.\nSTART YOUR EXPANSION AND ENJOY!", 0.1f);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
