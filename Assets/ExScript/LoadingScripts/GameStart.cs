using UnityEngine;
using UnityEngine.EventSystems;

public class GameStart : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("클릭");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("실행됬음");
        LoadingManager.Instance.LoadingCanvasOn("CopyLobby");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
