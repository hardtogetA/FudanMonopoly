using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIPlayerInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("PlayerControls")]
    public int windowNum;
    public Image playerInfo;
    private Vector3 moveDistance = new Vector3(0, 8, 0);
    public Vector2 PanelOrigianlPos;
    public bool isOpen = false;
    public bool isUp = false;

    void Start()
    {

    }

    void Update()
    { 
        if (isOpen && UISystem.instance.GetComponent<UISystem>().windowOwner != windowNum)
            ForcedClose();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UISystem.instance.GetComponent<UISystem>().windowOwner > 4)
            return;

        if (!isOpen)
        {
            isUp = true;
            playerInfo.transform.DOComplete();
            playerInfo.rectTransform.DOComplete();
            playerInfo.transform.DOMove(playerInfo.transform.position + moveDistance, .1f).SetEase(Ease.OutCubic);
            playerInfo.transform.DOPunchScale(Vector3.one / 6, .2f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (UISystem.instance.GetComponent<UISystem>().windowOwner > 4)
            return;

        if (!isOpen && isUp)
        {
            isUp = false;
            playerInfo.transform.DOComplete();
            playerInfo.rectTransform.DOComplete();
            playerInfo.transform.DOMove(playerInfo.transform.position - moveDistance, .1f).SetEase(Ease.OutCubic);
        }        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (UISystem.instance.GetComponent<UISystem>().windowOwner > 4)
            return;

        if (isOpen) 
        {
            playerInfo.transform.DOComplete();
            playerInfo.rectTransform.DOComplete();
            transform.GetChild(2).gameObject.SetActive(false);
            playerInfo.rectTransform.DOAnchorPos(PanelOrigianlPos, .3f).SetEase(Ease.OutCubic).OnComplete(() => 
                { 
                    isOpen = false;  
                    UISystem.instance.GetComponent<UISystem>().windowOwner = 0;
                    transform.GetChild(2).gameObject.SetActive(false); //to fix a bug which emerges when info is closed before fully open
                    transform.GetChild(1).gameObject.SetActive(true);
                });
            playerInfo.rectTransform.DOSizeDelta(new Vector2(200, 300), .2f);
            playerInfo.transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 131.4f), .2f);
        } 
        else 
        {
            isOpen = true;
            isUp = false;
            transform.GetChild(1).gameObject.SetActive(false);
            UISystem.instance.GetComponent<UISystem>().windowOwner = windowNum;
            playerInfo.transform.DOComplete();
            playerInfo.rectTransform.DOComplete();
            playerInfo.rectTransform.DOAnchorPos(new Vector2(0, 100), .2f).SetEase(Ease.OutCubic);
            playerInfo.rectTransform.DOSizeDelta(new Vector2(700, 500), .2f);
            playerInfo.transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 214), .2f).OnComplete(() => 
                {
                transform.GetChild(2).gameObject.SetActive(true);
                });
        }
    }

    public void ForcedClose(){
        isOpen = false;
        isUp = false;
        playerInfo.transform.DOComplete();
        playerInfo.rectTransform.DOComplete();
        transform.GetChild(2).gameObject.SetActive(false);
        playerInfo.rectTransform.DOAnchorPos(PanelOrigianlPos, .3f).SetEase(Ease.OutCubic);
        playerInfo.rectTransform.DOSizeDelta(new Vector2(200, 300), .2f);
        playerInfo.transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 131.4f), .2f).OnComplete(() =>
        {
            transform.GetChild(2).gameObject.SetActive(false); //to fix a bug which emerges when info is closed before fully open
            transform.GetChild(1).gameObject.SetActive(true);
        }); ;
    }
}
