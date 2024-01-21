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
        loadingTexts[1] = "HOLOLIVE ��� : STARTED���� APEX CR�ſ��� �ʹ� �ο� ���� ������ ������ ���̿���.";
        loadingTexts[2] = "HOLOLIVE ��� : JEWELRY BOX���� V����������S5���� HOLOLIVE����� VSPO�� �ݶ󺸰� " +
            "������� �Ǵ� ��Ⱑ �Ǿ���.";
        loadingTexts[3] = "HOLOLIVE ��� : �� ������ ���ΰ��̱⵵�� ���ھ߹� ��ʹ� APEX CR�ſ��� 2ȸ ����ߴ�.";
        loadingTexts[4] = "HOLOLIVE ��� : HoloX�� �Ѽ� La+ Darkness�� V����������S5���� �� ���� ����� �޼��ߴ�.";
        loadingTexts[5] = "HOLOLIVE ��� : HoloX�� �Ѽ� La+ Darkness�� ���������� �޲����� ���ϵ��� ũ�� ������ ����.";

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
                Debug.Log("�ε���");
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
            Debug.Log("����Ϸ�");
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
