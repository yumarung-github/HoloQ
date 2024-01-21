using InterfaceSpace;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardCoolTime : MonoBehaviour
{
    public Image cardSlider;

    public  TextMeshProUGUI tempText;
    public float cardCoolDownTime;
    public float updateTime;
    Coroutine coolTime = null;
    public bool coolOn;
    public ActiveCard cardInfo;
    
    // Start is called before the first frame update
    void Start()
    {
        
        updateTime = 0f;
        coolOn = true;
        cardSlider.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (coolOn)
        {
            if(coolTime != null)
            {
                StopCoroutine(coolTime);
                coolTime = null;
                Debug.Log("d");
            }
            cardSlider.gameObject.SetActive(false);
            GetComponent<Button>().enabled = true;
        }
        else
        {
            GetComponent<Button>().enabled = false;
            if (GameManager.Instance.enemy.expGainEnd || GameManager.Instance.isDefeat)
            {
                StopCoroutine(coolTime);
            }
        }
    }
    public void OnclickButton()
    {
        Uimanager.Instance.sceneImage.GetComponent<Image>().sprite = cardInfo.transform.GetComponent<Image>().sprite;
        Uimanager.Instance.sceneImage.transform.Find("SkillName").GetComponent<TextMeshProUGUI>().text =
            cardInfo.cardStatus.name;
        Uimanager.Instance.CutScene();
        //Debug.Log("ss");
        cardSlider.gameObject.SetActive(true);
        tempText = cardSlider.transform.GetComponentInChildren<TextMeshProUGUI>();
        coolOn = false;
        //attack
        if (GameManager.Instance.isBattle)
        {
            coolTime = StartCoroutine("CardCool");
        }
        cardInfo.cardStatus.Active();
    }
    
    IEnumerator CardCool()
    {
        IBuffable ibuffable;

        updateTime = 0f;
        cardSlider.fillAmount = 1.0f;
        if (cardInfo.cardStatus is IBuffable buffableCard)
        {
            ibuffable = buffableCard;//수정하기
            GameManager.Instance.BuffPoolingActive(cardInfo.type);
        }
        else
        {
            ibuffable = null;
        }
        
        bool isClear = false;


        while (cardSlider.fillAmount != 0.0f)
        {
            if (!GameManager.Instance.isBattle)
            {
                if (coolTime != null)
                {
                    StopCoroutine(coolTime);
                    coolTime = null;
                    coolOn = true;
                }
            }
            updateTime += Time.deltaTime;
            cardSlider.fillAmount =1.0f - (Mathf.Lerp(0,10,updateTime/cardCoolDownTime));
            tempText.text = (Mathf.Floor(cardSlider.fillAmount * cardCoolDownTime * 100f ) / 100f).ToString();

            
            if (ibuffable != null)
            {
                
                if ((cardCoolDownTime - cardSlider.fillAmount * cardCoolDownTime) >= ibuffable.ClearTime && !isClear)
                {
                    isClear = true;
                    ibuffable.BuffClear();
                    GameManager.Instance.BuffPoolingActiveReset(cardInfo.type);
                }
            }
            //Debug.Log(cardSlider.fillAmount);
            yield return new WaitForSeconds(Time.deltaTime * 10f);
        }
        cardSlider.fillAmount = 1.0f;
        cardSlider.gameObject.SetActive(false);
        coolOn = true;
        
    }
}
