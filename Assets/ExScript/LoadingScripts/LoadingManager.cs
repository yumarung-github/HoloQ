using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : SingleTon<LoadingManager>
{
    public GameObject loadingCanvas;
    public Slider slider;
    private string sceneName;
    public Image loadingImage;
    public TextMeshProUGUI textInfo;
    private Dictionary<int, string> loadingTexts;
    public List< Sprite> loadingPosters = new List<Sprite>();

    private float time;
    public bool isDeckLoading;
    Coroutine coroutine;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        loadingTexts = new Dictionary<int, string>();
        isDeckLoading = false;
        LoadingTextInit();
    }

    void Update()
    {
        if (isDeckLoading)
        {
            loadingCanvas.SetActive(false);
            isDeckLoading = false;
        }
        
    }
    public void LoadingCanvasOn(string name)
    {
        sceneName = name;
        int temp = 0;
        loadingPosters = Resources.LoadAll<Sprite>("Loading").ToList();
        temp = Random.Range(1, loadingPosters.Count + 1);
        Debug.Log(temp);
        Debug.Log(loadingPosters.Count);
        loadingImage.sprite = Resources.Load<Sprite>("Loading/Loading" + temp.ToString());
        temp = Random.Range(1, 6);
        textInfo.text = loadingTexts[temp];

        coroutine = StartCoroutine("LoadSceneCoroutine");
        //SceneManager.LoadScene(sceneName);
    }
    void LoadingTextInit()
    {
        loadingTexts[1] = "HOLOLIVE 상식 : STARTED팀은 APEX CR컵에서 초반 싸움에 가장 만나기 무서운 팀이였다.";
        loadingTexts[2] = "HOLOLIVE 상식 : JEWELRY BOX팀은 V최협결정전S5이후 HOLOLIVE멤버와 VSPO의 콜라보가 " +
            "잦아지게 되는 계기가 되었다.";
        loadingTexts[3] = "HOLOLIVE 상식 : 이 게임의 주인공이기도한 토코야미 토와는 APEX CR컵에서 2회 우승했다.";
        loadingTexts[4] = "HOLOLIVE 상식 : HoloX의 총수 La+ Darkness는 V최협결정전S5에서 한 라운드 우승을 달성했다.";
        loadingTexts[5] = "HOLOLIVE 상식 : HoloX의 총수 La+ Darkness는 세계정복을 꿈꾸지만 부하들은 크게 관심이 없다.";

    }
    IEnumerator LoadSceneCoroutine()
    {
        loadingCanvas.SetActive(true);
        //SceneManager.LoadScene(sceneName);
        AsyncOperation opration = SceneManager.LoadSceneAsync(sceneName);
        opration.allowSceneActivation = false;
        slider.value = 0f;
        float updateTime = 0f;
        slider.value = 0.0f;


        while (slider.value != 1f || !opration.isDone)
        {
            updateTime += Time.deltaTime;
            slider.value = Mathf.Lerp(0, opration.progress, updateTime / 0.7f);
            //Debug.Log(slider.value);
            //Debug.Log(opration.progress);
            if (slider.value >= 0.9f)
            {
                Debug.Log("로딩끝");
                slider.value = 1f;
                opration.allowSceneActivation = true;
                //StopCoroutine(coroutine);
            }
            //Debug.Log(opration.progress);
            yield return new WaitForSeconds(Time.deltaTime * 0.7f);
        }
        if (sceneName == "CopyLobby")
        {
            Invoke("InitLobby", 1f);
            Debug.Log("실행완료");
        }
        if (sceneName == "MainGame")
        {
            GameManager.Instance.player.transform.position = new Vector3(8.63f, 0.2f, 2.5f);
            GameManager.Instance.player.transform.rotation = new Quaternion(0f,180f,0f,0f);
        }
    }
    void InitLobby()
    {
        Uimanager.Instance.isInit = false;
    }
}
