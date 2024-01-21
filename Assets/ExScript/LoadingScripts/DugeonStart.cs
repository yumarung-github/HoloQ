using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DugeonStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickDugeon()
    {
        LoadingManager.Instance.LoadingCanvasOn("MainGame");
        if (Uimanager.Instance.popUpMenu != null)
        {
            Uimanager.Instance.popUpMenu.SetActive(true);
        }
    }
}
