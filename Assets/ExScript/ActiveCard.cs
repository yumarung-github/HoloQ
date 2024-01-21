using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCard : Card
{
    [SerializeField]
    public ActiveCardStatus cardStatus = null;//필요한거
    public float coolTime;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        coolTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInit)
        {
            cardStatus = (ActiveCardStatus)CardManager.Instance.thisCard[typeOfCard];
            atk = cardStatus.atk;
            hp = cardStatus.hp;
            type = cardStatus.type;
            coolTime = cardStatus.coolTime;
            skillInfo = cardStatus.skillInfo;
            //Debug.Log(cardStatus.name);
            //Debug.Log(cardStatus.coolTime);

            isInit = false;
        }
    }
}
