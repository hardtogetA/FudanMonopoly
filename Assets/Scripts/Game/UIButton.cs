using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int windowNum;

    public Image lightImg;
    public Color lightColor;
    private bool isMapOpen = false;

    void Start()
    {

    }

    public void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        lightImg.DOColor(lightColor, .1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {   
        lightImg.DOColor(Color.clear, .1f);
        // if (EventSystem.current.currentSelectedGameObject != gameObject)
        // {
        //     rect.DOColor(rectColorMouseOver, .2f);
        // }
    }

    //set in Inspector to attach this function to the Map Button
    public void MapControl()
    {      
        if (!isMapOpen)
        {
            if (UISystem.instance.windowOwner > 4) return;

            isMapOpen = true;
            UISystem.instance.GetComponent<UISystem>().windowOwner = 8;
            UISystem.instance.transform.GetChild(1).gameObject.SetActive(true);
            UISystem.instance.BlurIn();
        }
        else
        {
            UISystem.instance.transform.GetChild(1).gameObject.SetActive(false);
            UISystem.instance.BlurOut();
            UISystem.instance.GetComponent<UISystem>().windowOwner = 0;
            isMapOpen = false;
        }
        
    }
}
