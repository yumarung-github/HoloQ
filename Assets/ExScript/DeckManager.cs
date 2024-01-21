using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckManager : SingleTon<DeckManager>
{
    public GameObject deckCanvas;
    public GameObject battleDeck;//배틀덱캔버스
    
    public Transform deckTransform;
    public List<Card> deckCards;
    public ActiveCard[] activeCards;
    public Transform[] battleCardTransforms;
    public PassiveCard[] passiveCards;

    public Button[] buttons;
    public int activeCardNum;
    public int passiveCardNum;


    new void Start()
    {
        base.Start();

        deckCards = new List<Card>();
        activeCards = new ActiveCard[6];
        battleCardTransforms = new Transform[6];
        passiveCards = new PassiveCard[6];

        
        //deckCanvas.SetActive(false);
        Invoke("DeckInit", 1f);//로딩끝나는시간
        
        activeCardNum = 0;
        passiveCardNum = 0;
    }
    // Update is called once per frame
    void Update()
    {
        
        
    }
    public void DeckSortByNameOrder()
    {
        List<GameObject> tempList = transform.GetComponent<InvenCheck>().inventory;
        List<Card> cardsList = new List<Card>();
        for (int i = 0; i < tempList.Count; i++)
        {
            cardsList.Add(tempList[i].GetComponent<Card>());
            cardsList[i].sortString = cardsList[i].cardName;
        }

        cardsList.Sort();

        int tempNum = 0;
        foreach (Card card in cardsList)//여기서 오브젝트자체 정렬
        {
            card.transform.SetSiblingIndex(tempNum);
            tempNum++;
        }


        //Dictionary<string,Card> cards = new Dictionary<string, Card>();

        //foreach(Card card in cardsList)
        //{
        //    cards[card.cardName] = card;
        //}
        //Dictionary<string, Card> sortDict = SortDictionary(cards);

    }
    public void DeckSortByOrder()
    {
        List<GameObject> tempList = transform.GetComponent<InvenCheck>().inventory;
        List<Card> cardsList = new List<Card>();
        for(int i = 0; i < tempList.Count; i++)
        {
            cardsList.Add(tempList[i].GetComponent<Card>());
            cardsList[i].sortString = cardsList[i].type;
        }
        
        cardsList.Sort();

        int tempNum = 0;
        foreach (Card card in cardsList)//여기서 오브젝트자체 정렬
        {
            card.transform.SetSiblingIndex(tempNum);
            tempNum++;
        }


        //Dictionary<string,Card> cards = new Dictionary<string, Card>();

        //foreach(Card card in cardsList)
        //{
        //    cards[card.cardName] = card;
        //}
        //Dictionary<string, Card> sortDict = SortDictionary(cards);

    }
    /*
    private Dictionary<string, Card> SortDictionary(Dictionary<string, Card> dict)
    {
        Dictionary<string, Card> sortedDic = new Dictionary<string, Card>();
        List<string> cardNames = new List<string>();
        cardNames = dict.Keys.ToList();
        cardNames.Sort();
        //여기에 반대로 정렬하면 됨

        foreach(string cardName in cardNames)
        {
            sortedDic.Add(cardName, dict[cardName]);
        }
        return sortedDic;
    }*/
    public void CardExpGain(float expAmount)
    {
        int tempExp = (int)(expAmount / deckCards.Count);
        for(int i= 0; i < deckCards.Count; i++)
        {
            deckCards[i].Exp += tempExp;
        }
        Uimanager.Instance.expGainWindow.transform.GetComponentInChildren<TextMeshProUGUI>().text 
            = "Exp " + tempExp.ToString() + "씩 획득";
    }
    public void DeckInit()//건들면 큰일남
    {
        //deckCanvas.SetActive(true);
        deckCards.Clear();
        passiveCardNum = 0;
        activeCardNum = 0;
        StatusInit();
        for (int i = 0; i < deckTransform.childCount - 12; i++)
        {
            //Debug.Log(i);
            //Debug.Log(deckTransform.childCount);
            //Debug.Log(i - passiveCardNum);
            //
            deckCards.Add(deckTransform.GetChild(i + 12).GetComponent<Card>());
            if (deckCards[i].GetComponent<ActiveCard>() != null)
            {
                //Debug.Log(deckCards[i].GetComponent<Item>().DeckIndex + "ddd");
                deckCards[i].GetComponent<ActiveCard>().StatusInit();
                activeCards[deckCards[i].GetComponent<Item>().DeckIndex] = deckCards[i].GetComponent<ActiveCard>();
                activeCardNum++;
            }
            else if (deckCards[i].GetComponent<PassiveCard>() != null)
            {
                deckCards[i].GetComponent<PassiveCard>().StatusInit();
                passiveCards[deckCards[i].GetComponent<Item>().DeckIndex - 6] = deckCards[i].GetComponent<PassiveCard>();
                passiveCardNum++;
            }
            //Debug.Log(deckCards[i].GetComponent<PassiveCard>().cardStatus.name);
        }
        string tempSkillInfo = null;
        Debug.Log(GameManager.Instance.player.MaxHp);
        Debug.Log(GameManager.Instance.player.Atk);
        for (int j = 0; j < passiveCards.Length; j++)
        {
            if (passiveCards[j] != null)
            {
                Debug.Log(passiveCards[j].cardStatus.name);
                passiveCards[j].cardStatus.SetPassive();
                tempSkillInfo = tempSkillInfo + "\n" + passiveCards[j].cardStatus.name ;
            }
        }
        Uimanager.Instance.deckStatWindow.transform.Find("Skills").GetComponent<Text>().text
            = "Skills " + tempSkillInfo;
        Uimanager.Instance.PlayerStatusInit();
        //BattleDeckInit();
        /*
        if (!Uimanager.Instance.isInit)
        {
            Debug.Log("창 모두 닫음");
            Uimanager.Instance.ExitAll();
            Uimanager.Instance.isInit = true;
            GameManager.Instance.player.gameObject.SetActive(false);
            Uimanager.Instance.LobbyBtnOnOff(false);
            LoadingManager.Instance.isDeckLoading = true;//덱 로딩 끝나고 로딩판 치우게
        }*/
        //deckCanvas.SetActive(false);
    }
    public void ButtonInit()
    {
        for (int i = 0; i < 6; i++)
        {
            buttons[i] = battleDeck.transform.Find("BattledeckPool").GetChild(i + 6).transform.GetComponent<Button>();

            if (activeCards[i] != null)
            {
                buttons[i].GetComponent<Image>().sprite = cardButtonSprite(i);

                buttons[i].GetComponent<CardCoolTime>().cardCoolDownTime = activeCards[i].coolTime;
                buttons[i].GetComponent<CardCoolTime>().cardInfo = activeCards[i];
                buttons[i].GetComponent<CardCoolTime>().coolOn = true;
                buttons[i].enabled = true;
            }
            else
            {
                buttons[i].enabled = false;
            }
        }
    }
    Sprite cardButtonSprite(int num)
    {
        //Debug.Log(activeCards[num].type);
        return Resources.Load<Sprite>("Icon/" + activeCards[num].type);        
    }
    public void BattleDeckInit()
    {
        //battleDeck = GameObject.Find("BattleDeck").gameObject;//ui매니저로 뺼수있으면 빼기
        
        //덱에서 액티브 가져오기
        for(int i=0;i < 6; i++)
        {
            if (activeCards[i] != null)
            {
                Debug.Log(activeCards[i].name);
                battleCardTransforms[i] = battleDeck.transform.Find("BattledeckPool").GetChild(i).transform;
                battleCardTransforms[i].GetComponent<Image>().sprite
                    = Resources.Load<GameObject>("Card/" + activeCards[i].name).GetComponent<Image>().sprite;
            }
        }
        ButtonInit();
    }
    void StatusInit()
    {
        GameManager.Instance.player.MaxHp = 0;
        //GameManager.Instance.player.Hp = 0;
        GameManager.Instance.player.Atk = 0;
        GameManager.Instance.player.DefenseRate = 1f;
        GameManager.Instance.player.HealRate = 1f;
    }
}
