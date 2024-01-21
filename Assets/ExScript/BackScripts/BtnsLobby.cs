using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnsLobby : MonoBehaviour
{
    public GameObject offBtn;
    public void OnClickThisBtn()
    {
        gameObject.SetActive(false);
        offBtn.SetActive(true);
    }
}
