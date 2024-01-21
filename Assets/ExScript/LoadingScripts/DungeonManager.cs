using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DungeonInit", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DungeonInit()
    {
        Uimanager.Instance.DungeonStart();
        
        LoadingManager.Instance.isDeckLoading = true;//던전 로딩 끝나고 로딩판 치우게
    }
}
