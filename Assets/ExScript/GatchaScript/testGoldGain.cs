using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGoldGain : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoldTest()
    {
        GameManager.Instance.player.Gold += 220;
    }
}
