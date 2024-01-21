using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenCheck : MonoBehaviour
{
    //�κ���ġ �ֱ�
    public List<GameObject> inventory;
    private Transform invenTrans;
    // Start is called before the first frame update
    void Start()
    {
        inventory = new List<GameObject>();//�ڽİ���
        Invoke("SiblingIndex", Time.deltaTime);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SiblingIndex()//���� �־��ٶ����� ȣ��
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
