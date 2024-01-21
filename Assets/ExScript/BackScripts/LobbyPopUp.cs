using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPopUp : MonoBehaviour
{
    private bool isOn;
    public GameObject alertWindow;
    private Image temp;
    // Start is called before the first frame update
    private void OnEnable()
    {
        isOn = true;
        temp = alertWindow.GetComponent<Image>();
        temp.color = new Color(temp.color.r, temp.color.b, temp.color.g, 1);
        Debug.Log("º¯°æ Ã¢ On");
    }
    private void Update()
    {
        if (isOn)
        {
            temp.color = new Color(temp.color.r, temp.color.b, temp.color.g, temp.color.a - Time.deltaTime);
            if(temp.color.a <= 0.05f)
            {
                isOn = false;
                gameObject.SetActive(false);
            }
        }
    }
}
