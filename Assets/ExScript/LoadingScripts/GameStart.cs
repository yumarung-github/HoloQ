using UnityEngine;
using UnityEngine.EventSystems;

public class GameStart : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Ŭ��");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("��������");
        LoadingManager.Instance.LoadingCanvasOn("CopyLobby");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
