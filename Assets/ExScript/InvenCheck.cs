using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenCheck : MonoBehaviour
{
    //인벤위치 넣기
    public List<GameObject> inventory;
    private Transform invenTrans;
    // Start is called before the first frame update
    void Start()
    {
        inventory = new List<GameObject>();//자식갯수
        Invoke("SiblingIndex", Time.deltaTime);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SiblingIndex()//덱에 넣어줄때마다 호출
    {
        invenTrans = Uimanager.Instance.invenTrans;
        inventory.Clear();
        for (int i = 0; i < invenTrans.childCount; i++)
        {
            inventory.Add(invenTrans.transform.GetChild(i).gameObject);
            inventory[i].GetComponent<Item>().invenIndex = i;
        }

    }
}
