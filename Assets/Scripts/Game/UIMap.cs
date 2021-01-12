using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMap : MonoBehaviour
{
    
    void Start()
    {
        for (int i = 0; i < MainSystem.instance.playerAmount; i++)
            transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < MainSystem.instance.playerAmount; i++)
        {   
            int currentBoxID= MainSystem.instance.Characters.GetChild(i).GetComponent<PlayerController>().currentBoxID;
            transform.GetChild(1).GetChild(i).GetComponent<RectTransform>().anchoredPosition
                = transform.GetChild(0).GetChild(currentBoxID).GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
