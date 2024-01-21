using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler, IEndDragHandler, IPointerEnterHandler
{
    public int invenIndex;
    [SerializeField]
    private int deckIndex;//덱에 있을 때
    public int DeckIndex
    {
        get
        {
            return deckIndex;
        }
    }

    public GameObject canvas;
    public Transform deckTransform;
    public Transform invenTransform;

    private GraphicRaycaster raycaster;
    public Transform cardTransform;

    public bool rayOnOff;
    public float maxDistance;

    public Vector3 nowPos;
    public bool isInven;


    //넘버찍기
    void Start()
    {
        canvas = GameObject.Find("DeckCanvas") as GameObject;
        deckTransform = canvas.transform.Find("DeckPool") as Transform;
        invenTransform = canvas.transform.Find("Inventory").transform.Find("Viewport").transform.Find("Content") as Transform;
        
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        rayOnOff = false;
        maxDistance = 1;
        cardTransform = transform.parent;
        nowPos = transform.position;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Uimanager.Instance.deckCanvas.activeSelf)
        {
            List<RaycastResult> cardresults = new List<RaycastResult>();
            raycaster.Raycast(eventData, cardresults);
            GameObject hitObj = cardresults[0].gameObject;
            
            if (hitObj.TryGetComponent<Card>(out Card card))
            {
                Uimanager.Instance.statWindow.transform.Find("Stat1").transform.Find("Name").GetComponent<Text>().text = card.cardName;
                Uimanager.Instance.statWindow.transform.Find("Stat1").transform.Find("Atk").GetComponent<Text>().text = "Atk : " + card.atk.ToString();
                Uimanager.Instance.statWindow.transform.Find("Stat1").transform.Find("Hp").GetComponent<Text>().text = "Hp : " + card.hp.ToString();
                Uimanager.Instance.statWindow.transform.Find("Stat1").transform.Find("Skill").GetComponent<Text>().text = card.skillInfo;
                Uimanager.Instance.statWindow.transform.Find("Stat2").transform.Find("Level").GetComponent<Text>().text = "Level : "  + card.Level.ToString();
                Uimanager.Instance.statWindow.transform.Find("Stat2").transform.Find("Exp").GetComponent<Text>().text 
                    = "Exp : " + card.Exp.ToString() + "/" + GameManager.Instance.essentialExp[card.Level];
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<Image>().raycastTarget = false;//내가 들고있는애가 맞으면 안되니까
        rayOnOff = true;
        cardTransform = transform.parent;//현재 부모 저장해두기
        transform.SetParent(canvas.transform);//맨위로 꺼내주기 밑으로 빠지면 안되서
        if(!isInven)
        {
            if (transform.GetComponent<ActiveCard>() != null)
            {
                DeckManager.Instance.activeCards[deckIndex] = null;
            }
            else if (transform.GetComponent<PassiveCard>() != null)
            {
                DeckManager.Instance.passiveCards[deckIndex - 6] = null;
            }
        }        
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        //밖으로 안나가게 하면 좋겠음
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);

        Debug.Log("드래그 중");
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 끝남");
        InvenRay(eventData);
        transform.position = nowPos;
        //Debug.Log(nowPos);
        rayOnOff = false;
        GetComponent<Image>().raycastTarget = true;//다시 켜주기
        DeckManager.Instance.DeckInit();
    }
    void InvenRay(PointerEventData eventData)
    {
        if (rayOnOff)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(eventData, results);
            GameObject hitObj = results[0].gameObject;
            if (hitObj.CompareTag("Inven"))
            {
                cardTransform = invenTransform;
                nowPos = hitObj.transform.position;//위치수정
                transform.SetParent(cardTransform);
            }
            else if (hitObj.CompareTag("Active") && transform.GetComponent<ActiveCard>() != null)
            {
                deckIndex = hitObj.transform.GetSiblingIndex();
                cardTransform = deckTransform;
                nowPos = hitObj.transform.position;
                //invenIndex = DeckNum;
                transform.SetParent(cardTransform);

                int tempNum = 0;
                for (int i = 0; i < deckIndex; i++)
                {
                    if (DeckManager.Instance.activeCards[i] == null)
                    {
                        tempNum++;
                    }
                }
                transform.SetSiblingIndex(deckIndex - tempNum + 12);
            }
            else if(hitObj.CompareTag("Passive") && transform.GetComponent<PassiveCard>() != null)
            {
                int tempNum = 0;
                deckIndex = hitObj.transform.GetSiblingIndex();
                cardTransform = deckTransform;
                nowPos = hitObj.transform.position;
                transform.SetParent(cardTransform);
                for (int i = 0; i < deckIndex; i++)
                {
                    if (i < 6)
                    {
                        if (DeckManager.Instance.activeCards[i] == null)
                        {
                            tempNum++;
                        }
                    }
                    else
                    {
                        if (DeckManager.Instance.passiveCards[i - 6] == null)
                        {
                            tempNum++;
                        }
                    }
                }
                transform.SetSiblingIndex(deckIndex - tempNum + 12);
            }
            else if (hitObj.CompareTag("Item"))
            {
                if (hitObj.GetComponent<Item>().cardTransform == invenTransform)//아이템 끼리일댸 인벤일때
                {
                    cardTransform = invenTransform;
                    if (hitObj.transform.position.x - eventData.position.x < 0)
                    {
                        nowPos = hitObj.transform.position;//위치수정
                        transform.SetParent(cardTransform);
                        transform.SetSiblingIndex(hitObj.GetComponent<Item>().invenIndex + 1);

                    }
                    else
                    {
                        transform.SetParent(cardTransform);
                        if (hitObj.GetComponent<Item>().invenIndex != 0)
                        {
                            transform.SetSiblingIndex(hitObj.GetComponent<Item>().invenIndex);
                        }
                        else
                        {
                            //Debug.Log(hitObj.GetComponent<Item>().invenIndex);
                            transform.SetSiblingIndex(0);
                        }

                    }
                }
                else//덱일떄
                {
                    if((transform.GetComponent<ActiveCard>() != null && hitObj.GetComponent<ActiveCard>() != null)
                        || (transform.GetComponent<PassiveCard>() != null && hitObj.GetComponent<PassiveCard>() != null))
                    {
                        int tempIndex = deckIndex;
                        deckIndex = hitObj.GetComponent<Item>().deckIndex;
                        hitObj.GetComponent<Item>().deckIndex = tempIndex;

                        Vector3 tempPos = Vector3.zero;
                        Transform tempCardTransform;
                        tempPos = nowPos;
                        tempCardTransform = cardTransform;
                        nowPos = hitObj.transform.position;
                        hitObj.transform.position = tempPos;
                        cardTransform = hitObj.GetComponent<Item>().cardTransform;
                        hitObj.GetComponent<Item>().cardTransform = tempCardTransform;

                        transform.SetParent(cardTransform);//
                        if (transform.GetComponent<PassiveCard>() != null)
                        {
                            int tempNum = 0;
                            for (int i = 0; i < deckIndex; i++)
                            {
                                if (i < 6)
                                {
                                    if (DeckManager.Instance.activeCards[i] == null)
                                    {
                                        tempNum++;
                                    }
                                }
                                else
                                {
                                    if (DeckManager.Instance.passiveCards[i - 6] == null)
                                    {
                                        tempNum++;
                                    }
                                }
                            }
                            transform.SetSiblingIndex(deckIndex - tempNum + 12);
                        }
                        else if (transform.GetComponent<ActiveCard>() != null)
                        {
                            int tempNum = 0;
                            for (int i = 0; i < deckIndex; i++)
                            {
                                if (DeckManager.Instance.activeCards[i] == null)
                                {
                                    tempNum++;
                                }
                            }
                            transform.SetSiblingIndex(deckIndex - tempNum + 12);
                        }

                        hitObj.transform.SetParent(tempCardTransform);
                        if (hitObj.GetComponent<PassiveCard>() != null)
                        {
                            int tempNum = 0;
                            for (int i = 0; i < hitObj.GetComponent<Item>().deckIndex; i++)
                            {
                                //Debug.Log(i);
                                if (i < 6)
                                {
                                    if (DeckManager.Instance.activeCards[i] == null)
                                    {
                                        tempNum++;
                                    }
                                }
                                else
                                {
                                    if (DeckManager.Instance.passiveCards[i - 6] == null)
                                    {
                                        tempNum++;
                                    }
                                }
                            }
                            //Debug.Log(tempNum);
                            hitObj.transform.SetSiblingIndex(hitObj.GetComponent<Item>().deckIndex - tempNum + 12);
                            //Debug.Log(hitObj.GetComponent<Item>().deckIndex - tempNum + 12);
                        }
                        else if (hitObj.GetComponent<ActiveCard>() != null)
                        {
                            int tempNum = 0;
                            for (int i = 0; i < hitObj.GetComponent<Item>().deckIndex; i++)
                            {
                                // Debug.Log(i);
                                //Debug.Log(hitObj.GetComponent<Item>().deckIndex);
                                //Debug.Log(hitObj.name);
                                if (DeckManager.Instance.activeCards[i] == null)
                                {
                                    tempNum++;
                                }
                            }
                            //Debug.Log(tempNum);
                            hitObj.transform.SetSiblingIndex(hitObj.GetComponent<Item>().deckIndex - tempNum + 12);
                            //Debug.Log(hitObj.GetComponent<Item>().deckIndex - tempNum + 12);
                        }
                    }
                    else
                    {
                        transform.SetParent(cardTransform);
                        transform.SetSiblingIndex(invenIndex);
                        Debug.Log("error다아아");
                    }
                    
                }
                hitObj.GetComponent<Item>().nowPos = hitObj.transform.position;
            }
            else
            {
                Debug.Log("error");
                //transform.position = nowPos;
                transform.SetParent(cardTransform);
                if(cardTransform == invenTransform)
                {
                    transform.SetSiblingIndex(invenIndex);
                }
                else
                {

                }
                
            }
            DeckManager.Instance.transform.
                GetComponent<InvenCheck>().SiblingIndex();//인덱스 자동정렬
            //invenmanager로 만들고 호출해버리자
            DeckManager.Instance.DeckInit();
            //이런식으로
            
        }
        /*
        void ActiveCardSetSibling()
        {
            int tempNum = 0;
            for (int i = 0; i < deckIndex; i++)
            {
                if (i < 6)
                {
                    if (DeckManager.Instance.activeCards[i] == null)
                    {
                        tempNum++;
                    }
                }
                else
                {
                    if (DeckManager.Instance.passiveCards[i - 6] == null)
                    {
                        tempNum++;
                    }
                }
            }
            transform.SetSiblingIndex(deckIndex - tempNum + 12);
        }
        void PassiveCardSetSibling(GameObject hitObj)
        {
            int tempNum = 0;
            for (int i = 0; i < hitObj.GetComponent<Item>().deckIndex; i++)
            {
                //Debug.Log(i);
                if (i < 6)
                {
                    if (DeckManager.Instance.activeCards[i] == null)
                    {
                        tempNum++;
                    }
                }
                else
                {
                    if (DeckManager.Instance.passiveCards[i - 6] == null)
                    {
                        tempNum++;
                    }
                }
            }
            Debug.Log(tempNum);
            hitObj.transform.SetSiblingIndex(hitObj.GetComponent<Item>().deckIndex - tempNum + 12);
            Debug.Log(hitObj.GetComponent<Item>().deckIndex - tempNum + 12);
        }*/
    }

    
}
