using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class TalkManager : SingleTon<TalkManager>
{
    private Dictionary<string, List<string>> talkScripts;
    //public Dictionary<string, Sprite> charImages; sprite�� readOnly�� �����ѵ� ������ ����
    public Image charImage;
    public TextMeshProUGUI scriptsText;
    public GraphicRaycaster ray;
    public string enemyName;
    public int scriptNum;
    public int lastScriptNum;
    public int textNum;
    //public Action change;
    public GameObject textCanvas;

    public bool cameraOn;

    private Dictionary<string, int> charNums;

    new void Start()
    {
        
        base.Start();
        textCanvas.SetActive(false);
        cameraOn = false;
        //change?.Invoke();
        
        talkScripts = new Dictionary<string, List<string>>();
        charNums = new Dictionary<string, int>();
        SetTextScripts();
        scriptNum = 1;
        lastScriptNum = 0;
        textNum = -1;
        enemyName = string.Empty;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(textCanvas.activeSelf == true && !Uimanager.Instance.menuOn && !GameManager.Instance.isBattle)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && scriptNum <= talkScripts[enemyName + scriptNum.ToString()].Count)
            {
                Debug.Log(scriptNum.ToString() +"/" + textNum.ToString());
                
                if (textNum >= talkScripts[enemyName + scriptNum.ToString()].Count - 1)//������ũ��Ʈ
                {
                    Debug.Log("��ũ��Ʈ �Ѿ");
                    scriptNum++;
                    textNum = 0;
                    //��������
                    if(scriptNum == 2)
                    Uimanager.Instance.BattleOn();
                }
                else
                {
                    textNum++;
                    lastScriptNum = scriptNum;
                }
                Debug.Log(scriptNum + " < " + charNums[enemyName]);
                if (scriptNum < charNums[enemyName] + 1)
                {
                    scriptsText.text = talkScripts[enemyName + scriptNum.ToString()][textNum];
                }
                if (lastScriptNum != scriptNum)
                {
                    charImage.sprite = Resources.Load<Sprite>(enemyName + scriptNum);

                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse0) && scriptNum >= charNums[enemyName] + 1 )//��ȭ�����°�
            {
                ScriptInit();
            }
        }
    }
    public void ScriptInit()
    {
        scriptNum = 1;
        textNum = -1;
        lastScriptNum = 0;
        if (!GameManager.Instance.isDefeat)
        {
            GameManager.Instance.enemy.gameObject.SetActive(false);
        }
        Uimanager.Instance.ExitAll();
        cameraOn = false;
        GameManager.Instance.isBattle = false;
    }
    public void InitText()
    {
        textNum = 0;
        charImage.sprite = Resources.Load<Sprite>(enemyName + scriptNum);
        lastScriptNum = 1;
        scriptsText.text = talkScripts[enemyName + scriptNum.ToString()][textNum];
    }
    public void SetTextScripts()
    {
        talkScripts.Clear();
        charNums.Clear();
        List<string> la1 = new List<string>
        {
            "�� ���� HoloX�� �Ѽ� ���ö� ��ũ�Ͻ���� �Ѵ�.",
            "���� ���ϰ� ������ �غ�����"
        };//1�������� ���ö� Ʃ�丮��
        List<string> la2 = new List<string>
        {
            "���ߴ�",
            "���� �������ʹ� ���ī�带 ���������� ����غ�����."
        };
        talkScripts.Add("La1", la1);
        talkScripts.Add("La2", la2);
        charNums.Add("La", 2);//��ũ��Ʈ �Ѱ���
        List<string> koyo1 = new List<string>
        {
            "���ڿ�! HoloX�� �γ��� �ڿ丮��!",
            "�̹� ���������� ���ݿ� ���缭 \n ���ī�带 ��Ȯ�� Ÿ�ֿ̹� �Ẽ��?"
        };
        List<string> koyo2 = new List<string>
        {
            "���߾�! \n ���ī��� ������ ���� �������ݿ� ȿ�����̾�!",
            "������ �츮 û�ҿ��� ���� ���ؼ� �˷��ٰž�"
        };
        talkScripts.Add("Koyo1", koyo1);
        talkScripts.Add("Koyo2", koyo2);
        charNums.Add("Koyo", 2);//��ũ��Ʈ �Ѱ���

        List<string> sakamata1 = new List<string>
        {
            "����������! û�ҿ� ���� ��ī��Ÿ Ŭ�ο���",
            "�̹� ���������� ü���� �������� ���� ���� �غ��ھ�?"
        };//3�������� ��ī��Ÿ
        List<string> sakamata2 = new List<string>
        {
            "�������� ���ϳ�",
            "������ �츮 Holox�� ��ȣ�� �̷��Ͽ� ������?"
        };
        talkScripts.Add("Sakamata1", sakamata1);
        talkScripts.Add("Sakamata2", sakamata2);
        charNums.Add("Sakamata", 2);//��ũ��Ʈ �Ѱ���
        List<string> iroha1 = new List<string>
        {
            "ī�ڸ� �̷��϶�� �Ͽ��̴�!",
            "���̴��԰� �ο�� ���� �� �� ���� ������ �ͼ����� ���ھ��?"
        };//4�������� �̷���
        List<string> iroha2 = new List<string>
        {
            "���� ���������",
            "������ HoloX�� ������ ���̴��԰��� �����ϱ� �ܴ��� �غ��ϱ� �ٷ���."
        };
        talkScripts.Add("Iroha1", iroha1);
        talkScripts.Add("Iroha2", iroha2);
        charNums.Add("Iroha", 2);//��ũ��Ʈ �Ѱ���
        List<string> lui1 = new List<string>
        {
            "��Ÿī�� Ÿī�� ���̶�� ��!",
            "���⼭ ���� �̱�� ���̶�� ������ �� \n �׷� �����غ���"
        };//������ �������� ����
        List<string> lui2 = new List<string>
        {
            "���� ��������",
            "���ö󽺿��Դ� ���� �� ���صε��� �Ұ� �׷� �����߾�!"
        };
        talkScripts.Add("Lui1", lui1);
        talkScripts.Add("Lui2", lui2);
        charNums.Add("Lui", 2);//��ũ��Ʈ �Ѱ���
    }
    public void SetTextScriptsJp()
    {
        talkScripts.Clear();
        charNums.Clear();
        List<string> la1 = new List<string>
        {
            "�窱�Ϫ���HoloX�Ϊ�������La+Darkesss����",
            "໪˪ƪ��몤����Ȫ��êƪߪ�"
        };//1�������� ���ö� Ʃ�丮��
        List<string> la2 = new List<string>
        {
            "�誯��ê���",
            "�Ī�����Ϫܪ����竫�ѫɪ�Ī��êƪߪ�"
        };
        talkScripts.Add("La1", la1);
        talkScripts.Add("La2", la2);
        charNums.Add("La", 2);//��ũ��Ʈ �Ѱ���
        List<string> koyo1 = new List<string>
        {
            "���󪳪裡HoloX�Ϊ��Ϊ��Ϫ�����������",
            "���󪫪��Ϫ��������˪��請�ƪܪ����竫�ѫɪ�Ī��êƪߪߪ�"
        };
        List<string> koyo2 = new List<string>
        {
            "���󪳪裡",
            "���󪳪裡"
        };
        talkScripts.Add("Koyo1", koyo1);
        talkScripts.Add("Koyo2", koyo2);
        charNums.Add("Koyo", 2);//��ũ��Ʈ �Ѱ���

        List<string> sakamata1 = new List<string>
        {
            "�Ъ��Ъ��Ъ�",
            "�Ъ��Ъ��Ъ�"
        };//3�������� ��ī��Ÿ
        List<string> sakamata2 = new List<string>
        {
            "�Ъ��Ъ��Ъ�",
            "�Ъ��Ъ��Ъ�"
        };
        talkScripts.Add("Sakamata1", sakamata1);
        talkScripts.Add("Sakamata2", sakamata2);
        charNums.Add("Sakamata", 2);//��ũ��Ʈ �Ѱ���
        List<string> iroha1 = new List<string>
        {
            "���㪭�󪸪㪭��",
            "���㪭�󪸪㪭��"
        };//4�������� �̷���
        List<string> iroha2 = new List<string>
        {
            "���㪭�󪸪㪭��",
            "���㪭�󪸪㪭��"
        };
        talkScripts.Add("Iroha1", iroha1);
        talkScripts.Add("Iroha2", iroha2);
        charNums.Add("Iroha", 2);//��ũ��Ʈ �Ѱ���
        List<string> lui1 = new List<string>
        {
            "����몤��",
            "����몤��"
        };//������ �������� ����
        List<string> lui2 = new List<string>
        {
            "����몤��",
            "����몤��"
        };
        talkScripts.Add("Lui1", iroha1);
        talkScripts.Add("Lui2", iroha2);
        charNums.Add("Lui", 2);//��ũ��Ʈ �Ѱ���
    }
}
