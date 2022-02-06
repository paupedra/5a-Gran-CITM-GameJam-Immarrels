using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Buildings : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject pop_up;

    public void Start()
    {
        pop_up.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        pop_up.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pop_up.SetActive(false);
    }


}
