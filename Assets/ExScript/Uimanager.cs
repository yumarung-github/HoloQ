using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Uimanager : SingleTon<Uimanager>
{
    public GameObject battleDeck;//
    public GameObject deckCanvas;//
    public GameObject textCanvas;//
    public GameObject menuCanvas;//
    public bool menuOn;
    public bool isInit;
    public bool autoOn;
    public TextMeshProUGUI mainGoldText;

    public GameObject statWindow;//
    public GameObject deckStatWindow;//

    public GameObject expGainWindow;//

    public Slider playerHpSlider;//
    public TextMeshProUGUI playerHpText;//
    public Slider enemyHpSlider;//
    public TextMeshProUGUI enemyHpText;//
    public Slider enemySkillSlider;//
    public TextMeshProUGUI enemySkillText;//

    public TextMeshProUGUI enemyNameText;//
    public TextMeshProUGUI conversationEnemyName;//

    public GameObject StageButtonCanvas;//
    public Button[] stageButtons;
    public GameObject autoButtonObj;//
    public GameObject miniMapObj;//

    public GameObject popUpMenu;

    public GameObject stagePos1;//
    public Transform[] stageTrasforms;

    public Transform spawnPoint;//
    public GameObject battleDefeatWindow;//
    public GameObject popUpLuck;//
    private bool isPopInit;
    public bool isPop;
    public GameObject cutSceneObj;
    public RectTransform sceneImage;

    private Coroutine cutSceneCo;

    public Transform invenTrans;

    public bool isGameStart;

    public string nowSceneName;

    public GameObject bgmWindow;
    public GameObject bgmContents;
    // Start is called before the first frame update
    new void Start()
    {
        menuOn = false;
        isInit = true;
        isPopInit = false;
        isPop = false;
        isGameStart = false;
        base.Start();
        stageButtons = new Button[6];//스테이지 6개 1-1~1-6
        stageTrasforms = new Transform[7];
        //DungeonStart();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        isGameStart=true;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        nowSceneName = scene.name;
        Debug.Log("OnSceneLoaded" + scene.name);
        Debug.Log(mode);
    }
    // Update is called once per frame
    void Update()
    {
        if (isGameStart)
        {
            GameManager.Instance.player.gameObject.SetActive(false);
            menuCanvas.SetActive(false);
            isGameStart = false;
        }
        if (GameManager.Instance.enemy != null)
        {
            if (GameManager.Instance.enemy.expGainEnd == true)
            {
                expGainWindow.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    GainExpInit();
                }
            }
        }
        if (GameManager.Instance.isDefeat == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //전투패배창
                BattleDefeat();

                GameManager.Instance.enemy.enemyFound.SetActive(true);
            }
        }
        if (isInit && battleDeck.activeSelf)
        {
            if (!isPopInit)
            {
                if (popUpLuck.transform.Find("LuckImage").TryGetComponent<Image>(out Image tempImage))
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (DeckManager.Instance.passiveCards[i] != null)
                        {
                            if (DeckManager.Instance.passiveCards[i].cardStatus is LuckCardStatus luckCardStatus)
                            {
                                tempImage.sprite = DeckManager.Instance.passiveCards[i].transform.GetComponent<Image>().sprite;
                                luckCardStatus.LuckActive();
                                popUpLuck.transform.Find("Atk").GetComponent<TextMeshProUGUI>().text =
                                "행운 공격력 : " + luckCardStatus.attRate.ToString();
                                tempImage.color = new Color(1, 1, 1, 1);
                            }
                        }
                    }
                    if (tempImage.sprite == null)
                    {
                        popUpLuck.transform.Find("Atk").GetComponent<TextMeshProUGUI>().text = "전투 준비";
                        BattleLuckPopUpCheck();
                        tempImage.color = new Color(1, 1, 1, 0);
                    }
                }
                isPopInit = true;
            }
            InvokeRepeating("BattleLuckPopUpCheck", Time.deltaTime, 0f);
        }
        if (!isInit)//로비돌아올때
        {
            Debug.Log("창 모두 닫음");
            ExitAll();
            isInit = true;
            GameManager.Instance.player.gameObject.SetActive(false);
            LobbyBtnOnOff(false);
            LoadingManager.Instance.isDeckLoading = true;//덱 로딩 끝나고 로딩판 치우게

        }
    }
    public void OnClickPop(bool onOff)
    {
        popUpMenu.gameObject.SetActive(onOff);
    }
    public void OnLoadLobby()
    {
        LoadingManager.Instance.LoadingCanvasOn("CopyLobby");
    }
    public void DungeonStart()
    {
        GameManager.Instance.player.gameObject.SetActive(true);
        LobbyBtnOnOff(true);//던전에선 켜줘야함
        StageButtonInit();//후에 옮길수도있음
        StageTransformInit();//후에 옮길수도있음
    }
    public void LobbyBtnOnOff(bool onOff)//로비에서 꺼야될거 끄고 던전오면 다시켜주게
    {
        autoButtonObj.SetActive(onOff);
        miniMapObj.SetActive(onOff);
        StageButtonCanvas.SetActive(onOff);
        popUpMenu = GameObject.Find("PopUpWindow");
        bool tempbool = false;
        Debug.Log(popUpMenu.transform.GetChild(0).transform.Find("DeckBtn").name);
        if (nowSceneName == "MainGame")
        {
            popUpMenu.transform.GetChild(0).transform.Find("DeckBtn").
                GetComponent<Button>().onClick.AddListener(DeckOpen);
            popUpMenu.transform.GetChild(0).transform.Find("LobbyBtn").
                GetComponent<Button>().onClick.AddListener(OnLoadLobby);

            popUpMenu.transform.GetChild(1).transform.Find("WindowExit").
                GetComponent<Button>().onClick.AddListener(() => OnClickPop(tempbool));
        }
        else
        {
            popUpMenu.transform.GetChild(0).transform.Find("DeckBtn").
                GetComponent<Button>().onClick.AddListener(DeckOpen);
            popUpMenu.transform.GetChild(0).transform.Find("LangBtn").
                GetComponent<Button>().onClick.AddListener(TalkManager.Instance.SetTextScriptsJp);
            //로비일단뺌
            popUpMenu.transform.GetChild(1).transform.Find("WindowExit").
                GetComponent<Button>().onClick.AddListener(() => OnClickPop(tempbool));
            bgmWindow.transform.Find("Bgmbar").GetChild(0).GetComponent<Button>().onClick.
                AddListener(AudioManager.Instance.SortMusic);
        }
        popUpMenu.SetActive(false);
    }
    public void CutScene()
    {
        cutSceneObj.SetActive(true);
        if (cutSceneCo != null)
        {
            StopCoroutine("CutSceneCo");
        }
        sceneImage.GetComponent<Image>().color = new Color(1, 1, 1, 1);//color32랑 color랑 다름
        cutSceneCo = StartCoroutine("CutSceneCo");
    }

    IEnumerator CutSceneCo()
    {
        sceneImage.localScale = new Vector3(0.1f, 0.1f, 1f);

        //Debug.Log(sceneImage.GetComponent<Image>().color);
        while (sceneImage.localScale.x < 1 && sceneImage.localScale.y < 1)
        {
            sceneImage.localScale += new Vector3(Time.deltaTime * 3f, Time.deltaTime * 3f, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (sceneImage.TryGetComponent(out Image image))
        {
            while (image.color.a > 0f)
            {
                //Debug.Log(image.color);
                Color tempColor = image.color;
                tempColor.a -= 0.1f;
                image.color = tempColor;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        cutSceneObj.SetActive(false);
        cutSceneCo = null;
    }

    private void BattleLuckPopUpCheck()
    {
        if (!isPop && isPopInit && !textCanvas.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Time.timeScale = 1;
                //Debug.Log(Time.timeScale);
                popUpLuck.SetActive(false);
                isPop = true;
                GameManager.Instance.isBattle = true;
                CancelInvoke("BattleLuckPopUpCheck");
                if (popUpLuck.transform.Find("LuckImage").TryGetComponent<Image>(out Image tempImage))
                {
                    tempImage.sprite = null;
                }
            }
        }
    }
    private void BattleDefeat()
    {
        BattleOff();
        GameManager.Instance.player.transform.position = spawnPoint.position;
        GameManager.Instance.player.transform.rotation = spawnPoint.rotation;
        TalkManager.Instance.ScriptInit();
        textCanvas.SetActive(false);
        DeckManager.Instance.DeckInit();
        GameManager.Instance.isDefeat = false;
    }
    private void GainExpInit()
    {
        BattleOff();
        GameManager.Instance.enemy.expGainEnd = false;
    }
    public void ClickStageButton()
    {
        Debug.Log("click");
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;

        GameManager.Instance.player.autoMove(stageTrasforms[clickObject.transform.GetSiblingIndex() + 1]);
    }
    public void ClickAuto()
    {
        if (autoOn)
        {
            autoOn = false;
        }
        else
        {
            autoOn = true;
        }
    }
    public void StageButtonInit()
    {
        stageButtons = StageButtonCanvas.transform.GetComponentsInChildren<Button>();
    }
    public void StageTransformInit()
    {
        stagePos1 = GameObject.Find("Stage1Pos");
        spawnPoint = stagePos1.transform;
        stageTrasforms = stagePos1.transform.GetComponentsInChildren<Transform>();
    }
    public void BattleOffInit()
    {
        enemyNameText.text = null;
        enemyHpSlider.value = 1f;
        enemySkillText.text = null;
        enemySkillSlider.value = 0f;
    }
    public void BattleOff()
    {
        BattleOffInit();
        playerHpSlider.value = GameManager.Instance.player.Hp / GameManager.Instance.player.MaxHp;
        GameManager.Instance.BuffReset();
        Debug.Log("전투종료");
        GameManager.Instance.isBattle = false;
        isPopInit = false;
        isPop = false;
        cutSceneObj.SetActive(false);

        battleDeck.SetActive(false);
        deckCanvas.SetActive(false);
        textCanvas.SetActive(true);
        menuCanvas.SetActive(true);

    }
    public void BattleOn()
    {
        battleDeck.SetActive(true);
        isPopInit = false;
        popUpLuck.SetActive(true);
        expGainWindow.SetActive(false);//경험치 바
        battleDefeatWindow.SetActive(false);
        menuCanvas.SetActive(false);
        cutSceneObj.SetActive(false);
        DeckManager.Instance.BattleDeckInit();
        deckCanvas.SetActive(false);
        textCanvas.SetActive(false);
        enemyHpSlider.value = 1f;
        GameManager.Instance.player.Hp = GameManager.Instance.player.Hp;//uiinit하기
        GameManager.Instance.enemy.isSkillEnd = true;//스킬시작
        GameManager.Instance.BuffPoolingInit();

        Time.timeScale = 0f;
        Debug.Log(Time.timeScale);
    }
    public void ExitAll()
    {
        GameManager.Instance.player.MoveSpeed = 3f;
        battleDeck.SetActive(false);
        deckCanvas.SetActive(false);
        textCanvas.SetActive(false);
        menuOn = false;
    }
    public void ExitDeck()
    {
        GameManager.Instance.player.MoveSpeed = 3f;
        deckCanvas.SetActive(false);
        menuOn = false;
    }
    public void DeckOpen()
    {
        GameManager.Instance.player.MoveSpeed = 0f;
        deckCanvas.SetActive(true);
        menuOn = true;
        PlayerStatusInit();
    }
    public void PlayerStatusInit()
    {
        deckStatWindow.transform.Find("Atk").GetComponent<Text>().text
            = "Atk : " + GameManager.Instance.player.Atk.ToString();
        deckStatWindow.transform.Find("Hp").GetComponent<Text>().text
            = "Hp / MaxHp : \n" + GameManager.Instance.player.Hp.ToString() + " / " + GameManager.Instance.player.MaxHp.ToString();

    }
    public void BgmWindowOnoff(bool onOff)
    {
        bgmWindow.SetActive(onOff);
    }
}
