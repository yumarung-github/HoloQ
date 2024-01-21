using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GatchaOnOff : MonoBehaviour
{
    public GameObject gatchaCanvas;
    public GameObject gatchaOnScreen;
    public GameObject gatchaItem;
    public GameObject backWindow;

    private bool isLobby;

    public GameObject langWindow;
    public GameObject goldAlert;
    // Start is called before the first frame update
    void Start()
    {
        isLobby = false;
        transform.Find("DeckButton").transform.GetComponent<Button>().
            onClick.AddListener(Uimanager.Instance.DeckOpen);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLobby)
        {
            if(CardManager.Instance.nowLobbyName != null)
            {
                transform.GetComponent<VideoPlayer>().clip = CardManager.Instance.cardVideo[CardManager.Instance.nowLobbyName];
            }
            else
            {
                transform.GetComponent<VideoPlayer>().clip = CardManager.Instance.cardVideo["Miko"];
            }
            gatchaCanvas.SetActive(false);
            gatchaOnScreen.SetActive(false);
            isLobby = true;
        }
    }
    public void LangWindowOn()
    {
        Debug.Log("언어변경");
        langWindow.SetActive(true);
    }
    public void GatchaCanvasOnOff(bool onOff)
    {
        CardManager.Instance.gatchaItem = this.gatchaItem;
        gatchaCanvas.SetActive(onOff);
        if (onOff)
        {
            if(AudioManager.Instance.nowBgm !=null)
                AudioManager.Instance.nowBgm.StopMusic();
        }
        else
        {
            if (AudioManager.Instance.nowBgm != null)
                AudioManager.Instance.nowBgm.PlayMusic();
        }
    }
    public void BackWindow(bool onOff)
    {
        backWindow.SetActive(onOff);
        if (onOff)
        {
            int temp = 0;
            
            foreach(Card card in DeckManager.Instance.deckCards)
            {
                Debug.Log(temp);
                if (!backWindow.transform.GetChild(0).GetChild(temp).gameObject.activeSelf)
                {
                    backWindow.transform.GetChild(0).GetChild(temp).gameObject.SetActive(true);
                }
                
                backWindow.transform.GetChild(0).GetChild(temp).GetComponent<Image>().sprite
                = card.gameObject.GetComponent<Image>().sprite;
                temp++;
            }
            for(int i = temp; i < 10; i++)
            {
                backWindow.transform.GetChild(0).GetChild(temp).gameObject.SetActive(false);
            }
        }
    }
    public void GatchaScreenOnOff(int num)
    {
        CardManager.Instance.gatchaNum = num;
        int wasteGold = CardManager.Instance.gatchaWaste * num;
        if (GameManager.Instance.player.Gold > wasteGold)
        {
            GameManager.Instance.player.Gold -= wasteGold;
            gatchaOnScreen.SetActive(true);
            Uimanager.Instance.deckCanvas.SetActive(true);
            CardManager.Instance.GatchaSet();//시작하자마자 바로 나오게
        }
        else
        {
            Debug.Log("골드가 부족하다");
            goldAlert.SetActive(true);
        }
    }
    public void BgmWindowOn()
    {
        Uimanager.Instance.BgmWindowOnoff(true);
    }
}
