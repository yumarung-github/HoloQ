using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenRay : MonoBehaviour
{
    public bool rayOnOff;
    public float maxDistance;
    public Canvas canvas;
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    private PointerEventData eventData;
    // Start is called before the first frame update
    void Start()
    {
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        rayOnOff = false;
        maxDistance = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (rayOnOff)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                eventData = new PointerEventData(eventSystem);
                eventData.position = Input.mousePosition;

                Debug.DrawLine(transform.position, transform.position + transform.forward * maxDistance, Color.red);
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(eventData, results);
                GameObject hitObj = results[1].gameObject;
                Debug.Log(hitObj.gameObject.name);
                if(hitObj.CompareTag("Inven"))
                {
                    transform.SendMessage("Silblingindex", hitObj);
                }
            }
        }
    }
    private void RayOn()
    {
        rayOnOff = true;
        Debug.Log("on");
    }
    private void RayOff()
    {
        rayOnOff = false;
        Debug.Log("off");

    }
}
