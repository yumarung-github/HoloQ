using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using InterfaceSpace;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : SingleTon<Player>, ITakeDamageable
{
    public void Hit(float damage)
    {
        Hp -= damage;
        //플레이어가 공격받는 효과음 추가
    }
    
    //public bool isGrounded;//현재 점프할 수 있는지
    //private float jumpSpeed;//점프 속도
    private float gravityForce;//중력
    protected Animator animator;
    Rigidbody rb;
    private float x;
    private float z;
    public Transform[] goal;
    public NavMeshAgent agent;

    public Detective playerDetect;
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { 
            moveSpeed = value;
            agent.speed = moveSpeed;
        }
    }
    [SerializeField]
    private float maxHp;
    public float MaxHp
    {
        get { return maxHp; }
        set
        {
            maxHp = value;
            hp = maxHp;
            //Uimanager.Instance.playerHp.value = hp / maxHp;
            string tempText= hp.ToString() + " / " + maxHp.ToString();
            Uimanager.Instance.playerHpText.text = tempText;
        }
    }

    [SerializeField]
    private float hp;
    public float Hp
    {
        get
        {
            return Mathf.Floor(hp);//소수점버리기
        }
        set
        {
            if (value * DefenseRate <= 0)//죽는지 체크
            {
                GameManager.Instance.isDefeat = true;
                Uimanager.Instance.battleDefeatWindow.SetActive(true);
                hp = 0;
                //죽는거 처리
            }
            else
            {
                if(hp< value)//힐 했을 때
                {
                    if(value * healRate >= maxHp)
                    {
                        hp = maxHp;
                        Uimanager.Instance.playerHpSlider.value = 1f;
                    }
                    else
                    {
                        hp = hp + (value - hp) * healRate;
                        Uimanager.Instance.playerHpSlider.value = hp / maxHp;
                    }
                }
                else//맞을 떄
                {
                    if (GameManager.Instance.isBattle)
                    {
                        Debug.Log(value * DefenseRate);
                        hp = hp - (hp - value) * defenseRate;
                        AudioManager.Instance.playerDmgSound.
                            Enqueue(AudioManager.Instance.PopBgm(dmgedAudio, false, transform));
                        Uimanager.Instance.playerHpSlider.value = hp / maxHp;
                        transform.GetComponent<Test>().DmgEmotion(player_Type);
                    }
                }
                
            }
            string tempText = hp.ToString() + " / " + maxHp.ToString();
            Uimanager.Instance.playerHpText.text = tempText;
        }
    }
    [SerializeField]
    private float atk;
    public float Atk
    {
        get
        {
            return Mathf.Floor(atk);//소수점버리기
        }
        set
        {
            if (value < 0) atk = 0;
            else
            {
                atk = value;
            }
        }
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
    private float healRate;
    public float HealRate
    {
        get { return healRate; }
        set
        {
            healRate = value;
        }
    }

    [SerializeField]
    private float ap;//던전 비용
    public float Ap
    {
        get { return ap; }
        set { ap = value; }
    }
    [SerializeField]
    private int gold;
    public int Gold
    {
        get { return gold; }
        set {
            if (GameManager.Instance.isBattle)
            {
                Uimanager.Instance.expGainWindow.transform.Find("GoldText").GetComponent<TextMeshProUGUI>().text
                    = (value - gold).ToString() + "골드 획득";
            }
            gold = value;
            Uimanager.Instance.mainGoldText.text = gold.ToString();
        }
    }
    
    public List<Transform> destination;
    public int destinationNum;
    public Transform lastTransform;
    public int dungeonNum;
    private bool isWalkingEnd;
    [SerializeField]
    private AudioClip walkingAudio;
    [SerializeField]
    private AudioClip dmgedAudio;
    [SerializeField]
    private char_Type player_Type;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        dungeonNum = 0;
        gravityForce = -9.8f;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        playerDetect = GetComponent<Detective>();

        moveSpeed = 3f;

        isWalkingEnd = false;
    }

    private void FixedUpdate()
    {
        if(Mathf.Sqrt(x * x + z * z) > 0)
        rb.rotation = rb.rotation * Quaternion.Euler(0, x * 3f, 0);
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isBattle)
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
        }
        if(Uimanager.Instance.nowSceneName == "MainGame")
        {
            GameObject autoText = Uimanager.Instance.autoButtonObj.transform.Find("Auto").gameObject;
            Button button = Uimanager.Instance.autoButtonObj.transform.GetComponent<Button>();
            Image tempImage = Uimanager.Instance.autoButtonObj.transform.GetComponent<Image>();
            if (dungeonNum != 0 && gameObject.activeSelf)
            {
                button.enabled = true;
                
                tempImage.color =
                    new Color(tempImage.color.r, tempImage.color.g, tempImage.color.b, 1);
                autoText.gameObject.SetActive(true);
            }
            else
            {
                button.enabled = false;
                //Debug.Log(Uimanager.Instance.autoButtonObj.name);
                autoText.gameObject.SetActive(false);
                tempImage.color = new Color(tempImage.color.r, tempImage.color.g, tempImage.color.b, 0);
            }
        }
        //if (Input.GetButtonDown("Fire2"))
        //{
        //    animator.SetBool("isJump", true);
        //}
        if (Uimanager.Instance.autoOn)
        {
            if (playerDetect.IsDetect)
            {
                lastTransform = playerDetect.targetTransform;
                agent.SetDestination(lastTransform.position);
            }
            else
            {
                agent.ResetPath();
                //Debug.Log("주변에 적이 없음");
            }
        }
        if(Mathf.Sqrt(x * x + z * z) > 0)
        {
            Uimanager.Instance.autoOn = false;
            agent.ResetPath();
            transform.Translate(0, 0, z * moveSpeed * Time.deltaTime);
            animator.SetFloat("speed", Mathf.Sqrt(x * x + z * z));
            lastTransform = null;
            if(isWalkingEnd == false)
            {
                if (AudioManager.Instance.walkingSound == null)
                    AudioManager.Instance.walkingSound = AudioManager.Instance.PopBgm(walkingAudio, true, transform);
                isWalkingEnd = true;
            }
        }
        else
        {
            if (isWalkingEnd == true)
            {
                isWalkingEnd = false;
                if(AudioManager.Instance.walkingSound != null)
                    AudioManager.Instance.ReturnBgm(AudioManager.Instance.walkingSound);

                AudioManager.Instance.walkingSound = null;
            }
            animator.SetFloat("speed", Mathf.Sqrt(agent.velocity.x * agent.velocity.x + agent.velocity.z * agent.velocity.z));
            if (Uimanager.Instance.nowSceneName == "MainGame")
            {
                if (AudioManager.Instance.walkingSound != null)
                {
                    if (agent.pathPending == false)
                    {
                        if (agent.hasPath == false)
                        {
                            AudioManager.Instance.ReturnBgm(AudioManager.Instance.walkingSound);
                        }
                    }
                }
                if (AudioManager.Instance.playerDmgSound.Count != 0 )
                {
                    SoundScript tempSc = AudioManager.Instance.playerDmgSound.Peek();
                    if (tempSc.IsPlayingMusic() == false)
                    {
                        AudioManager.Instance.ReturnBgm(AudioManager.Instance.playerDmgSound.Dequeue());

                    }

                }
            }
        }
        
        
    }
    public void autoMove(Transform stageTransform)
    {
        if(AudioManager.Instance.walkingSound == null)
            AudioManager.Instance.walkingSound = AudioManager.Instance.PopBgm(walkingAudio, true, transform);

        Debug.Log("오토무브");
        agent.isStopped = false;
        agent.SetDestination(stageTransform.position);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IChatable>(out IChatable chatable))
        {
            GameManager.Instance.enemy = other.transform.parent.GetComponent<Enemy>();

            GameManager.Instance.enemy.InitEnemyStatus();

            TalkManager.Instance.enemyName = chatable.EnemyName;//스크립트이름 설정
            TalkManager.Instance.textCanvas.SetActive(true);
            TalkManager.Instance.InitText();
            TalkManager.Instance.cameraOn = true;
        }

    }
}
