using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterfaceSpace;
using TMPro;
using UnityEngine.UI;
public enum char_Type
{
    Lap = 0,
    Koyo = 1,
    Saka = 2,
    Iro = 3,
    Lui = 4,
    towa = 5
}
public class Enemy : MonoBehaviour, IActiveable, ITakeDamageable
{
    [SerializeField]
    private char_Type enemy_Type;
    public Skill skill;
    [SerializeField]
    private string enemyName;
    public string EnemyName
    {
        get { return enemyName; }
        set {
            enemyName = value;
            Uimanager.Instance.conversationEnemyName.text = enemyName;
            Uimanager.Instance.enemyNameText.text = enemyName;
        }
    }
    [SerializeField]
    private float maxHp;
    public float MaxHp
    {
        get { return maxHp; }
        set { 
            maxHp = value;
            Uimanager.Instance.enemyHpSlider.value = hp / maxHp;
            string tempText = hp.ToString() + " / " + maxHp.ToString();
            Uimanager.Instance.enemyHpText.text = tempText;
        }
    }
    [SerializeField]
    private float hp;
    public float Hp
    {
        get { return hp; }
        set {
            if (value <= 0)
            {
                StopCoroutine(skillCool);
                skillCool = null;
                hp = 0;
                string tempText = hp.ToString() + " / " + maxHp.ToString();
                Uimanager.Instance.enemyHpText.text = tempText;
                Uimanager.Instance.enemyHpSlider.value = hp / maxHp;

                DeckManager.Instance.CardExpGain(gainExp);
                GameManager.Instance.player.Gold += gainGold;
                DeckManager.Instance.DeckInit();
                GameManager.Instance.isClear[enemyName] = true;
                expGainEnd = true;
                Uimanager.Instance.autoOn = false;
                
            }
            else
            {
                hp = value;
                if (hp / maxHp >= 1f)
                {
                    Uimanager.Instance.enemyHpSlider.value = 1f;
                }
                else
                {
                    if (GameManager.Instance.isBattle)
                    {
                        Uimanager.Instance.enemyHpSlider.value = hp / maxHp;
                        AudioManager.Instance.enemyDmgSound.
                            Enqueue(AudioManager.Instance.PopBgm(enemyDmgSound, false, transform));
                        transform.GetComponent<Test>().DmgEmotion(enemy_Type);
                    }
                }
                
                string tempText = hp.ToString() + " / " + maxHp.ToString();
                Uimanager.Instance.enemyHpText.text = tempText;
            }
        }//HP바 조절 추가
    }
    [SerializeField]
    private float atk;
    public float Atk
    {
        get { return atk; }
        set { atk = value; }
    }
    [SerializeField]
    private float defenseRate;
    public float DefenseRate
    {
        get
        {
            return defenseRate;
        }
        set
        {
            defenseRate = value;
        }
    }
    [SerializeField]
    private float gainExp;
    [SerializeField]
    private int gainGold;
    public bool expGainEnd;

    public bool isSkillEnd;
    Coroutine skillCool = null;
    public int enemyRoomNum;

    [SerializeField]
    private AudioClip enemyDmgSound;
    // Start is called before the first frame update
    public GameObject enemyFound;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSkillEnd && GameManager.Instance.isBattle)
        {
            Uimanager.Instance.enemySkillText.text = skill.SkillName;
            Debug.Log("시작" + skill.SkillName);
            //스타트 코루틴
            //StartCoroutine("EnemySkillCool");
            skillCool = StartCoroutine("EnemySkillCool");
            isSkillEnd = false;
            
        }
        if(GameManager.Instance.enemy == this)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                //임시
                Active();
            }
        }
        if (AudioManager.Instance.enemyDmgSound.Count != 0)
        {
            SoundScript tempSc = AudioManager.Instance.enemyDmgSound.Peek();
            if (tempSc.IsPlayingMusic() == false)
            {
                Debug.Log("없애기");
                AudioManager.Instance.ReturnBgm(AudioManager.Instance.enemyDmgSound.Dequeue());

            }

        }
    }
    public void InitEnemyStatus()
    {
        Hp = MaxHp;
        expGainEnd = false;
        EnemyName = enemyName;
        isSkillEnd = false;
    }
    public void Active() // 공격
    {
        GameManager.Instance.player.Hit(atk * skill.SkillDmgRate);
        //스킬사용
        int temp = 0;
        temp = Random.Range(0, 2);
        switch (temp)
        {
            case 0:
                skill = skill.skill1;
                break;
            case 1: 
                skill = skill.skill2;
                break;
            default:
                Debug.Log("error");
                break;
        }
    }
    public void Hit(float damage)//데미지 받는거
    {
        Hp -= damage;
        //적이 공격받는 효과음 추가

        Debug.Log("현재 적의 체력" + Hp);
        //카드가 공격을하면
        //hp = hp - damage
    }
    
    IEnumerator EnemySkillCool()
    {
        Slider tempSlider = Uimanager.Instance.enemySkillSlider;

        float updateTime = 0f;
        tempSlider.value = 0.0f;
        updateTime += Time.deltaTime;
        while (tempSlider.value != 1f)
        {
            updateTime += Time.deltaTime;
            //Debug.Log(updateTime / skill.SkillUseTime);
            //Debug.Log(Mathf.Lerp(0, 10, updateTime / skill.SkillUseTime));
            tempSlider.value = Mathf.Lerp(0, 10, updateTime / skill.SkillUseTime);
            //Debug.Log(tempSlider.value);

            yield return new WaitForSeconds(Time.deltaTime * 10f);
        }
        tempSlider.value = 0f;
        isSkillEnd = true;
        StopCoroutine(skillCool);
        Active();
    }
}
