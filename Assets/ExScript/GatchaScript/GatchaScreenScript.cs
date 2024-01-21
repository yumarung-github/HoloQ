using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GatchaScreenScript : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public int nowGatchaNum;
    void Start()
    {
        nowGatchaNum = 1;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    { 

        if(nowGatchaNum == CardManager.Instance.gatchaNum)
        {
            Uimanager.Instance.deckCanvas.SetActive(false);//�����ȳ����ϱ����� ��� ���ذ� ����
            nowGatchaNum = 0;
            gameObject.SetActive(false);
        }
        else
        {
            CardManager.Instance.GatchaSet();
        }
        nowGatchaNum++;
    }
    
}
