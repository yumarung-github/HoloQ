using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class ChangeLobby : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public GameObject lobbyCanvas;
    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject thisObj = DeckManager.Instance.deckCards[transform.GetSiblingIndex()].gameObject;
        string cardName = thisObj.GetComponent<Image>().sprite.name;
        string[] words = new string[3];
        words = cardName.Split('_');
        cardName = words[0];
        CardManager.Instance.nowLobbyName = cardName;
        lobbyCanvas.GetComponent<VideoPlayer>().clip = CardManager.Instance.cardVideo[cardName];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
