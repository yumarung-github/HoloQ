using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveCard : Card
{
    [SerializeField]
    public PassiveCardStatus cardStatus = null;//필요한거
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        if (isInit)
        {
            cardStatus = (PassiveCardStatus)CardManager.Instance.thisCard[typeOfCard];
            atk = cardStatus.atk;
            hp = cardStatus.hp;
            type = cardStatus.type;
            skillInfo = cardStatus.skillInfo;
            //Debug.Log(cardStatus.name);
            //cardStatus.Active();
            isInit = false;
        }
    }
}
