using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using TMPro;


public class UISystem : MonoBehaviour
{
    [HideInInspector]  public static UISystem instance;

    /*
        change to enum later
        0 = null; 
        1,2,3,4 = uiplayerInfo;
        5 = items;
        6 = cards;
        7 = overview;
        8 = map;
        9 = help;
    */
    public int windowOwner = 0; 

    private GameObject postProcessing;
    private bool isBlurring = false;
    private float blurFactor;

    private void Start()
    {
        instance = this;
        postProcessing = GameObject.Find("PostProcessing");
        
        //Initialize PlayerInfo panel
        for(int i = 0; i < MainSystem.instance.playerAmount; i++)
        {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
            switch (MainSystem.instance.playerAmount)
            {
                case 2: 
                    transform.GetChild(0).GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector2(-160 + i * 320, -342);
                    transform.GetChild(0).GetChild(i).GetComponent<UIPlayerInfo>().PanelOrigianlPos = new Vector2(-160 + i * 320, -342);
                    break;
                case 3: 
                    transform.GetChild(0).GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector2(-320+ i * 320, -342); 
                    transform.GetChild(0).GetChild(i).GetComponent<UIPlayerInfo>().PanelOrigianlPos = new Vector2(-320 + i * 320, -342); 
                    break;
                default: 
                    transform.GetChild(0).GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector2(-480 + i * 320, -342);
                    transform.GetChild(0).GetChild(i).GetComponent<UIPlayerInfo>().PanelOrigianlPos = new Vector2(-480 + i * 320, -342);
                    break;
            }

            SetPlayerInfo(i + 1, GameObject.Find("LocalData").GetComponent<LocalData>().singleinfo[i]);
        }


    }

    void Update()
    { 
        if (isBlurring)
        {
            postProcessing.GetComponent<Volume>().profile.components[0].parameters[5].SetValue(new FloatParameter(blurFactor));
        }
    }

    void ShowDialog(int EvnetType = 0)
    {

    }

    public void BlurIn()
    {
        isBlurring = true;
        blurFactor = 6f;
        DOTween.To(() => blurFactor, x => blurFactor = x, 1f, .2f).OnComplete(() =>  isBlurring = false) ;
    }

    public void BlurOut()
    {
        isBlurring = true;
        blurFactor = 1f;
        DOTween.To(() => blurFactor, x => blurFactor = x, 6f, .2f).OnComplete(() => isBlurring = false);
    }

    public void SetPlayerInfo(int playerNum, LocalData.SingleSimpleInfoRsp singleInfo)
    {
        transform.GetChild(0).GetChild(playerNum - 1).GetChild(0).GetComponent<TMP_Text>().text = singleInfo.name;
        //brief Info
        transform.GetChild(0).GetChild(playerNum - 1).GetChild(1).GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = singleInfo.cash.ToString();
        transform.GetChild(0).GetChild(playerNum - 1).GetChild(1).GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = singleInfo.deposit.ToString();
        transform.GetChild(0).GetChild(playerNum - 1).GetChild(1).GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = singleInfo.specialCoin.ToString();
        transform.GetChild(0).GetChild(playerNum - 1).GetChild(1).GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = singleInfo.Lands.ToString();
        //Full Info
        transform.GetChild(0).GetChild(playerNum - 1).GetChild(2).GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = singleInfo.cash.ToString();
        transform.GetChild(0).GetChild(playerNum - 1).GetChild(2).GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = singleInfo.deposit.ToString();
        transform.GetChild(0).GetChild(playerNum - 1).GetChild(2).GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = singleInfo.specialCoin.ToString();
        transform.GetChild(0).GetChild(playerNum - 1).GetChild(2).GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = singleInfo.Lands.ToString();
        transform.GetChild(0).GetChild(playerNum - 1).GetChild(2).GetChild(4).GetChild(1).GetComponent<TMP_Text>().text = singleInfo.Facilities.ToString();
        transform.GetChild(0).GetChild(playerNum - 1).GetChild(2).GetChild(5).GetChild(1).GetComponent<TMP_Text>().text = singleInfo.DayInHospital.ToString();
        transform.GetChild(0).GetChild(playerNum - 1).GetChild(2).GetChild(6).GetChild(1).GetComponent<TMP_Text>().text = singleInfo.DayInPrison.ToString();
        transform.GetChild(0).GetChild(playerNum - 1).GetChild(2).GetChild(7).GetChild(1).GetComponent<TMP_Text>().text = singleInfo.DayInHotel.ToString();
    }
}
