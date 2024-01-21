using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : SingleTon<AudioManager>
{
    public List<GameObject> musics = new List<GameObject>();//가챠를 위한거
    public GameObject prefabBgm;
    public Queue<SoundScript> musicsQueue = new Queue<SoundScript>();
    public SoundScript nowBgm;
    public SoundScript walkingSound;
    public SoundScript doorSound;
    public AudioClip doorClip;

    public Queue<SoundScript> playerDmgSound = new Queue<SoundScript>();
    public Queue<SoundScript> enemyDmgSound = new Queue<SoundScript>();

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        musics = Resources.LoadAll<GameObject>("Music").ToList();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            nowBgm = PopBgm(musics[0].GetComponent<Bgm>().bgmClip, true, transform);
        }
        if (doorSound != null)
        {
            if (doorSound.IsPlayingMusic() == false)
            {
                ReturnBgm(doorSound);
                Debug.Log("문소리");
                doorSound = null;
            }
        }
    }
    void Init()
    {

        for (int i = 0; i < 10; i++)
        {
            GameObject tempObj;
            tempObj = Instantiate(prefabBgm);
            tempObj.transform.parent = transform;
            musicsQueue.Enqueue(tempObj.GetComponent<SoundScript>());
            tempObj.SetActive(false);
        }
    }
    public SoundScript PopBgm(AudioClip audioClip, bool isLoop, Transform parents = null)
    {

        SoundScript soundScript = musicsQueue.Dequeue();
        soundScript.gameObject.SetActive(true);
        soundScript.transform.parent = parents;
        soundScript.PlayMusic(audioClip, isLoop);
        return soundScript;
    }
    public void ReturnBgm(SoundScript soundScript)
    {
        musicsQueue.Enqueue(soundScript);
        soundScript.transform.parent = transform;
        soundScript.gameObject.SetActive(false);
    }
    public void SortMusic()
    {
        List<Bgm> tempList = new List<Bgm>();
        for (int i=0; i < Uimanager.Instance.bgmContents.transform.childCount; i++)
        {
            if(Uimanager.Instance.bgmContents.transform.GetChild(i).TryGetComponent(out Bgm bgm))
            {
                tempList.Add(bgm);
            }
        }      

        tempList.Sort();

        int tempNum = 0;
        foreach (Bgm bgm in tempList)//여기서 오브젝트자체 정렬
        {
            bgm.transform.SetSiblingIndex(tempNum);
            tempNum++;
        }

    }
}
