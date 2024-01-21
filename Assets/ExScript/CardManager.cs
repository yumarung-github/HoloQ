using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CardManager : SingleTon<CardManager>
{
    public List<GameObject> cards;
    
    public Dictionary<CARD_TYPE, CardStatus> thisCard;//�E��
    public GameObject gatchaItem;
    
    public int gatchaNum;
    public int gatchaWaste;
    public Dictionary<string, VideoClip> cardVideo = new Dictionary<string, VideoClip>();
    public string nowLobbyName;

    new void Start()
    {
        base.Start();
        gatchaNum = 2;
        gatchaWaste = 20;
        nowLobbyName = null;
        

        //���� ī��Ŵ������� �������ִ°� ����.
        cards = Resources.LoadAll<GameObject>("Card").ToList();
        foreach (GameObject cardObj in cards)
        {
            string cardName = cardObj.GetComponent<Image>().sprite.name;
            string[] words = new string[3];
            words = cardName.Split('_');
            cardName = words[0];

            
            if(!cardVideo.ContainsKey(cardName))
            {
                Debug.Log(cardName);
                cardVideo[cardName] = Resources.Load<VideoClip>("Sprites/LoadingVideo/" + cardName);
            }
            //Debug.Log(cardVideo.Count);
        }
        //cards = inven.invenItemIndex[]
        thisCard = new Dictionary<CARD_TYPE, CardStatus>();
        thisCard.Add(CARD_TYPE.Smite, new SmiteCardStatus());
        thisCard.Add(CARD_TYPE.Quick, new QuickCardStatus());
        thisCard.Add(CARD_TYPE.Anger, new AngerCardStatus());
        thisCard.Add(CARD_TYPE.Rage, new RageCardStatus());
        thisCard.Add(CARD_TYPE.Luck, new LuckCardStatus());
        thisCard.Add(CARD_TYPE.Defense, new DefenseCardStatus());
        thisCard.Add(CARD_TYPE.Ironwall, new IronwallCardStatus());
        thisCard.Add(CARD_TYPE.Smash, new SmashCardStatus());
        thisCard.Add(CARD_TYPE.Grit, new GritCardStatus());
        thisCard.Add(CARD_TYPE.Patience, new PatienceCardStatus());
        thisCard.Add(CARD_TYPE.Heal, new HealCardStatus());
        thisCard.Add(CARD_TYPE.Friendship, new FriendShipCardStatus());
        thisCard.Add(CARD_TYPE.Encourage, new EncourageCardStatus());
        thisCard.Add(CARD_TYPE.Pray, new PrayCardStatus());
    }
    void Update()
    {
        
    }
    public GameObject GatchaRand()//���⼭ �׳� Ȯ�� ����ϸ�ɵ� ī��� ��ݾƿ�������
    {

        int tempGatcha = 0;
        if (AudioManager.Instance.musics.Count > 0)
        {
            tempGatcha = Random.Range(0, 2);
        }
        else
        {
            tempGatcha = 0;
        }
        GameObject gatchaObj;
        if (tempGatcha == 0)
        {
            
            int temp = Random.Range(0, cards.Count());
            gatchaObj = Instantiate(cards[temp]);
            return gatchaObj;
        }
        else
        {
            int temp = Random.Range(0, AudioManager.Instance.musics.Count());
            gatchaObj = Instantiate(AudioManager.Instance.musics[temp]);
            AudioManager.Instance.musics.RemoveAt(temp);
            return gatchaObj;
        }
    }
    public void GatchaSet()
    {
        Uimanager.Instance.deckCanvas.SetActive(true);
        if (gatchaItem.TryGetComponent(out Image gatchaImage))
        {
            GameObject tempGatchaObj = GatchaRand();
            if (tempGatchaObj.TryGetComponent(out Image cardImage))
            {
                string tempText = tempGatchaObj.name;
                string[] words = new string[3];
                    words = tempText.Split('(');
                tempText = words[0];
                /*
                Debug.Log(tempText.IndexOf('('));
                tempText.Substring(tempText.IndexOf('('), tempText.IndexOf('(') - 2);*/
                Debug.Log(tempText);
                tempGatchaObj.name = tempText;
                //gatchaItem.SetActive(true);//��� �ɰŰ����ѵ�
                gatchaImage.sprite = cardImage.sprite;
                TextMeshProUGUI gatchaTextTemp 
                    = gatchaImage.transform.parent.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                //�̸�ŭ �ؽ�Ʈ���� �̹�������

                if(tempGatchaObj.GetComponent<Card>() != null)//ī���϶�
                {
                    tempGatchaObj.transform.SetParent(Uimanager.Instance.invenTrans);

                    tempGatchaObj.transform.localScale = new Vector3(1f, 1f, 1f);
                    DeckManager.Instance.transform.GetComponent<InvenCheck>().SiblingIndex();
                    gatchaTextTemp.text = "ī�� ȹ�� !";
                }
                else
                {
                    gatchaTextTemp.text = "BGM ȹ�� !";
                    tempGatchaObj.transform.SetParent(Uimanager.Instance.bgmContents.transform); 
                }
            }
        }
    }
}
