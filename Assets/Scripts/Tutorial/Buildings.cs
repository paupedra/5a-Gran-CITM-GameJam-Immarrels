using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Buildings : MonoBehaviour
{
    public GameObject pop_up;

    public void Start()
    {
        pop_up.SetActive(false);
    }

    public void Update()
    {
        OnMouseOver("PopUp");
    }

    public void OnMouseOver(string tag)
    {
        PointerEventData data = new PointerEventData(EventSystem.current);

        data.position = Input.mousePosition;

        List<RaycastResult> ray = new List<RaycastResult>();

        EventSystem.current.RaycastAll(data,ray);

        foreach(RaycastResult rayResult in ray)
        {
            if(rayResult.gameObject.tag == tag)
            {
                pop_up.SetActive(true);
            }
        }

    }
}
