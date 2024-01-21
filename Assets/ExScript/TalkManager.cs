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
    //public Dictionary<string, Sprite> charImages; sprite가 readOnly라 저장한뒤 쓸수가 없음
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
                
                if (textNum >= talkScripts[enemyName + scriptNum.ToString()].Count - 1)//다음스크립트
                {
                    Debug.Log("스크립트 넘어감");
                    scriptNum++;
                    textNum = 0;
                    //전투진입
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
            if (Input.GetKeyDown(KeyCode.Mouse0) && scriptNum >= charNums[enemyName] + 1 )//대화끝내는거
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
            "이 몸은 HoloX의 총수 라플라스 다크니스라고 한다.",
            "먼저 편하게 전투를 해보도록"
        };//1스테이지 라플라스 튜토리얼
        List<string> la2 = new List<string>
        {
            "잘했다",
            "다음 전투부터는 방어카드를 적극적으로 사용해보도록."
        };
        talkScripts.Add("La1", la1);
        talkScripts.Add("La2", la2);
        charNums.Add("La", 2);//스크립트 총갯수
        List<string> koyo1 = new List<string>
        {
            "콘코요! HoloX의 두뇌인 코요리야!",
            "이번 전투에서는 공격에 맞춰서 \n 방어카드를 정확한 타이밍에 써볼래?"
        };
        List<string> koyo2 = new List<string>
        {
            "잘했어! \n 방어카드는 배율이 낮은 빠른공격에 효과적이야!",
            "다음엔 우리 청소원이 힐에 대해서 알려줄거야"
        };
        talkScripts.Add("Koyo1", koyo1);
        talkScripts.Add("Koyo2", koyo2);
        charNums.Add("Koyo", 2);//스크립트 총갯수

        List<string> sakamata1 = new List<string>
        {
            "바쿠바쿠바쿠! 청소원 범고래 사카마타 클로에야",
            "이번 전투에서는 체력이 낮아졌을 때에 힐을 해보겠어?"
        };//3스테이지 사카마타
        List<string> sakamata2 = new List<string>
        {
            "생각보다 잘하네",
            "다음엔 우리 Holox의 경호원 이로하에 가볼래?"
        };
        talkScripts.Add("Sakamata1", sakamata1);
        talkScripts.Add("Sakamata2", sakamata2);
        charNums.Add("Sakamata", 2);//스크립트 총갯수
        List<string> iroha1 = new List<string>
        {
            "카자마 이로하라고 하오이다!",
            "루이누님과 싸우기 전에 좀 더 힘든 전투에 익숙해져 보겠어요?"
        };//4스테이지 이로하
        List<string> iroha2 = new List<string>
        {
            "좋은 전투였어요",
            "다음은 HoloX의 간부인 루이누님과의 전투니까 단단히 준비하길 바래요."
        };
        talkScripts.Add("Iroha1", iroha1);
        talkScripts.Add("Iroha2", iroha2);
        charNums.Add("Iroha", 2);//스크립트 총갯수
        List<string> lui1 = new List<string>
        {
            "맛타카네 타카네 루이라고 해!",
            "여기서 나를 이기면 끝이라고 생각해 줘 \n 그럼 시작해볼까"
        };//마지막 스테이지 루이
        List<string> lui2 = new List<string>
        {
            "좋은 전투였어",
            "라플라스에게는 내가 잘 말해두도록 할게 그럼 수고했어!"
        };
        talkScripts.Add("Lui1", lui1);
        talkScripts.Add("Lui2", lui2);
        charNums.Add("Lui", 2);//스크립트 총갯수
    }
    public void SetTextScriptsJp()
    {
        talkScripts.Clear();
        charNums.Clear();
        List<string> la1 = new List<string>
        {
            "わがはいはHoloXのそうすいLa+Darkesssだ。",
            "先にてがるいせんとをやってみろ。"
        };//1스테이지 라플라스 튜토리얼
        List<string> la2 = new List<string>
        {
            "よくやった。",
            "つぎからはぼうぎょカㅡドをつかってみろ！"
        };
        talkScripts.Add("La1", la1);
        talkScripts.Add("La2", la2);
        charNums.Add("La", 2);//스크립트 총갯수
        List<string> koyo1 = new List<string>
        {
            "こんこよ！HoloXのずのうはくいこよりだよ",
            "こんかいはこうげきにあわせてぼうぎょカㅡドをつかってみみよ"
        };
        List<string> koyo2 = new List<string>
        {
            "こんこよ！",
            "こんこよ！"
        };
        talkScripts.Add("Koyo1", koyo1);
        talkScripts.Add("Koyo2", koyo2);
        charNums.Add("Koyo", 2);//스크립트 총갯수

        List<string> sakamata1 = new List<string>
        {
            "ばくばくばく",
            "ばくばくばく"
        };//3스테이지 사카마타
        List<string> sakamata2 = new List<string>
        {
            "ばくばくばく",
            "ばくばくばく"
        };
        talkScripts.Add("Sakamata1", sakamata1);
        talkScripts.Add("Sakamata2", sakamata2);
        charNums.Add("Sakamata", 2);//스크립트 총갯수
        List<string> iroha1 = new List<string>
        {
            "じゃきんじゃきん",
            "じゃきんじゃきん"
        };//4스테이지 이로하
        List<string> iroha2 = new List<string>
        {
            "じゃきんじゃきん",
            "じゃきんじゃきん"
        };
        talkScripts.Add("Iroha1", iroha1);
        talkScripts.Add("Iroha2", iroha2);
        charNums.Add("Iroha", 2);//스크립트 총갯수
        List<string> lui1 = new List<string>
        {
            "こんるい！",
            "こんるい！"
        };//마지막 스테이지 루이
        List<string> lui2 = new List<string>
        {
            "こんるい！",
            "こんるい！"
        };
        talkScripts.Add("Lui1", iroha1);
        talkScripts.Add("Lui2", iroha2);
        charNums.Add("Lui", 2);//스크립트 총갯수
    }
}
