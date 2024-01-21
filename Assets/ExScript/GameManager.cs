using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingleTon<GameManager>
{
    public Player player;
    public Enemy enemy;
    public bool isBattle;

    public Dictionary<int, int> essentialExp;

    public GameObject buffWindow;
    public List<GameObject> buffPooling;
    public List<GameObject> pooledBuff;
    public bool isDefeat;
    public Dictionary<string, bool> isClear;
    public Transform cameraTrans;
    new void Start()
    {
        isBattle = false;
        isDefeat = false;
        isClear = new Dictionary<string, bool>
        {
            { "����", false },
            { "���ö󽺴�ũ�Ͻ�", false },
            { "��ī��ŸŬ�ο�", false },
            { "ī�ڸ��̷���", false },
            { "Ÿī�׷���", false },
            { "�������ڿ丮", false }
        };
        base.Start();
        essentialExp = new Dictionary<int, int>();
        LevelingMap();
        buffPooling = new List<GameObject>();
        pooledBuff = new List<GameObject>();
        for(int i = 0; i < buffWindow.transform.childCount; i++)
        {
            buffPooling.Add(buffWindow.transform.GetChild(i).gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BuffPoolingInit()
    {
        foreach(GameObject gameObject in buffPooling)
        {
            gameObject.SetActive(false);
        }
        int tempIndex = 0;
        for(int i = 0; i<DeckManager.Instance.passiveCards.Length; i++)
        {
            if (DeckManager.Instance.passiveCards[i] != null)
            {
                if (DeckManager.Instance.passiveCards[i].type == "Attacker")
                {
                    if(buffPooling.FindIndex(gameObject => gameObject.name.Equals("AttackerPassive")) == -1)
                    {
                        tempIndex = buffPooling.FindIndex(gameObject => gameObject.name.Equals("AttackerPassive(Clone)"));
                    }
                    else
                    {
                        tempIndex = buffPooling.FindIndex(gameObject => gameObject.name.Equals("AttackerPassive"));
                    }
                }
                else if (DeckManager.Instance.passiveCards[i].type == "Defensive")
                {
                    if (buffPooling.FindIndex(gameObject => gameObject.name.Equals("DefensivePassive")) == -1)
                    {
                        tempIndex = buffPooling.FindIndex(gameObject => gameObject.name.Equals("DefensivePassive(Clone)"));
                    }
                    else
                    {
                        tempIndex = buffPooling.FindIndex(gameObject => gameObject.name.Equals("DefensivePassive"));
                    }
                }
                else if (DeckManager.Instance.passiveCards[i].type == "Healing")
                {
                    if (buffPooling.FindIndex(gameObject => gameObject.name.Equals("HealingPassive")) == -1)
                    {
                        tempIndex = buffPooling.FindIndex(gameObject => gameObject.name.Equals("HealingPassive(Clone)"));
                    }
                    else
                    {
                        tempIndex = buffPooling.FindIndex(gameObject => gameObject.name.Equals("HealingPassive"));
                    }
                }
                else
                {
                    Debug.Log("error");
                }

                if (tempIndex == -1)//������
                {
                    Debug.Log(DeckManager.Instance.passiveCards[i].type + "����");
                    GameObject tempGameObject;
                    tempGameObject = Instantiate(Resources.Load<GameObject>(
                        "Icon/" + DeckManager.Instance.passiveCards[i].type + "Passive"));
                    tempGameObject.transform.SetParent(buffWindow.transform);
                    pooledBuff.Add(tempGameObject);
                }
                else
                {
                    buffPooling[tempIndex].SetActive(true);
                    pooledBuff.Add(buffPooling[tempIndex]);
                    buffPooling.RemoveAt(tempIndex);
                }
            }
            else
            {
                Debug.Log("nullPassive");
            }
        }
    }
    public void BuffReset()//������������
    {
        foreach(GameObject tempGameObject in pooledBuff)
        {
            buffPooling.Add(tempGameObject);
        }
        pooledBuff.Clear();
    }
    public void BuffPoolingActive(string activeType)
    {
        int tempIndex = 0;
        if (activeType != null)
        {
            if (activeType == "Attacker")
            {
                if (buffPooling.FindIndex(gameObject => gameObject.name.Equals("AttackerActive")) == -1)
                {
                    tempIndex = buffPooling.FindIndex(gameObject => gameObject.name.Equals("AttackerActive(Clone)"));
                }
                else
                {
                    tempIndex = buffPooling.FindIndex(gameObject => gameObject.name.Equals("AttackerActive"));
                }
            }
            else if (activeType == "Defensive")
            {
                if (buffPooling.FindIndex(gameObject => gameObject.name.Equals("DefensiveActive")) == -1)
                {
                    tempIndex = buffPooling.FindIndex(gameObject => gameObject.name.Equals("DefensiveActive(Clone)"));
                }
                else
                {
                    tempIndex = buffPooling.FindIndex(gameObject => gameObject.name.Equals("DefensiveActive"));
                }
            }
            else if (activeType == "Healing")
            {
                if (buffPooling.FindIndex(gameObject => gameObject.name.Equals("HealingActive")) == -1)
                {
                    tempIndex = buffPooling.FindIndex(gameObject => gameObject.name.Equals("HealingActive(Clone)"));
                }
                else
                {
                    tempIndex = buffPooling.FindIndex(gameObject => gameObject.name.Equals("HealingActive"));
                }
            }
            else
            {
                Debug.Log("error");
            }

            if (tempIndex == -1)//������
            {
                Debug.Log(activeType + "����");
                GameObject tempGameObject;
                tempGameObject = Instantiate(Resources.Load<GameObject>(
                    "Icon/" + activeType + "Active"));
                tempGameObject.transform.SetParent(buffWindow.transform);
                pooledBuff.Add(tempGameObject);
            }
            else
            {
                buffPooling[tempIndex].SetActive(true);
                pooledBuff.Add(buffPooling[tempIndex]);
                buffPooling.RemoveAt(tempIndex);
            }
        }
    }
    public void BuffPoolingActiveReset(string activeType)
    {
        int tempIndex;
        if (pooledBuff.FindIndex(gameObject => gameObject.name.Equals(activeType + "Active")) == -1)
        {
            tempIndex = pooledBuff.FindIndex(gameObject => gameObject.name.Equals(activeType + "Active(Clone)"));
        }
        else
        {
            tempIndex = pooledBuff.FindIndex(gameObject => gameObject.name.Equals(activeType + "Active"));
        }
        Debug.Log(tempIndex);
        GameObject tempObject = pooledBuff[tempIndex];
        buffPooling.Add(tempObject);
        pooledBuff.RemoveAt(tempIndex);
        tempObject.SetActive(false);
    }
    void LevelingMap()
    {
        for(int i = 1; i < 11; i++)
        {
            essentialExp.Add(i , i * i * 10);
            //Debug.Log(i + "������ �ʿ� ����ġ" + i * i * 10);
        }
    }
}
