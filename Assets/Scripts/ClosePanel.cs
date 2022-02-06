using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    public void OnOffPanel(GameObject name)
    {
        if (name.activeSelf == true)
            name.SetActive(false);
        else
            name.SetActive(true);
    }
}
