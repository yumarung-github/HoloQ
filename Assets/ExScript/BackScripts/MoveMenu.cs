using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ImageP
{
    right,
    left,
    top,
    bottom
}
public class MoveMenu : MonoBehaviour
{
    
    public ImageP tempP;
    private Vector3 mainPos;
    public Transform targetTrans;
    public Button onBtn;
    public Button offBtn;

    private bool isOn;
    private bool isOff;

    private GameObject tempGold;
    private GameObject tempGoldImage;
    private GameObject tempMenu;


    // Start is called before the first frame update
    void Start()
    {
        mainPos = transform.position;
        isOn = false;
        isOff = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(tempP == ImageP.right)
        {
            if (isOn)
            {
                if (targetTrans.position.x - Time.deltaTime * 8f < transform.position.x)
                {
                    isOn = false;
                    offBtn.enabled = true;
                }
                transform.position = Vector3.Lerp(transform.position, targetTrans.position, Time.deltaTime * 8f);
            }
            if (isOff)
            {
                if (mainPos.x + Time.deltaTime * 8f > transform.position.x)
                {
                    isOff = false;
                    onBtn.enabled = true;
                }
                //Debug.Log(targetTrans.position);
                transform.position = Vector3.Lerp(transform.position, mainPos, Time.deltaTime * 8f);
            }
        }
        else if(tempP == ImageP.left)
        {
            if (isOn)
            {
                if (targetTrans.position.x + Time.deltaTime * 8f > transform.position.x)
                {
                    isOn = false;
                    offBtn.enabled = true;
                }
                transform.position = Vector3.Lerp(transform.position, targetTrans.position, Time.deltaTime * 8f);
            }
            if (isOff)
            {
                if (mainPos.x - Time.deltaTime * 8f < transform.position.x)
                {
                    isOff = false;
                    onBtn.enabled = true;
                }
                //Debug.Log(targetTrans.position);
                transform.position = Vector3.Lerp(transform.position, mainPos, Time.deltaTime * 8f);
            }
        }
        else if(tempP == ImageP.top) 
        {
            if (isOn)
            {
                if (targetTrans.position.y - Time.deltaTime * 8f < transform.position.y)
                {
                    isOn = false;
                    offBtn.enabled = true;
                }
                transform.position = Vector3.Lerp(transform.position, targetTrans.position, Time.deltaTime * 8f);
            }
            if (isOff)
            {
                if (mainPos.y + Time.deltaTime * 8f > transform.position.y)
                {
                    isOff = false;
                    onBtn.enabled = true;
                }
                //Debug.Log(targetTrans.position);
                transform.position = Vector3.Lerp(transform.position, mainPos, Time.deltaTime * 8f);
            }
        }
        else
        {
            if (isOn)
            {
                if (targetTrans.position.y + Time.deltaTime * 8f > transform.position.y)
                {
                    isOn = false;
                    offBtn.enabled = true;
                }
                transform.position = Vector3.Lerp(transform.position, targetTrans.position, Time.deltaTime * 8f);
            }
            if (isOff)
            {
                if (mainPos.y - Time.deltaTime * 8f < transform.position.y)
                {
                    isOff = false;
                    onBtn.enabled = true;
                }
                //Debug.Log(targetTrans.position);
                transform.position = Vector3.Lerp(transform.position, mainPos, Time.deltaTime * 8f);
            }
        }
        
    }
    public void MoveTarget(bool temp)
    {
        //Debug.Log("dd");
        if (temp)
        {
            if (tempGold == null)
                tempGold = Uimanager.Instance.menuCanvas.transform.GetChild(0).gameObject;
            if (tempGoldImage == null)
                tempGoldImage = Uimanager.Instance.menuCanvas.transform.GetChild(1).gameObject;
            if (tempMenu == null)
                tempMenu = Uimanager.Instance.menuCanvas.transform.Find("MenuButton").gameObject;
            tempGold.SetActive(false);
            tempGoldImage.SetActive(false);
            tempMenu.SetActive(false);
            isOn = true;
            offBtn.enabled = false;
        }
        else
        {
            tempGold.SetActive(true);
            tempGoldImage.SetActive(true);
            tempMenu.SetActive(true);
            isOff =true;
            onBtn.enabled = false;
        }
    }
}
