using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterfaceSpace;
using System;

namespace InterfaceSpace
{
    public interface IActiveable//적에게도 필요 //공격이나 효과적용
    {
        public void Active();//target.hp버프라던가 안에서 알아서 처리해주기
    }
    public interface ITakeDamageable
    {
        public float Hp { get; set; }
        public void Hit(float damage);//activeabel
    }
    public interface IBuffable
    {
        public float ClearTime { get; set; }
        public void BuffClear();
    }
}

public abstract class CardStatus
{
    public string name;
    public float atk;
    public float hp;
    public string type;
    public string skillInfo;

    public CardStatus()
    {
        name = string.Empty;
    }

    public abstract void StatusInit();
}
public abstract class ActiveCardStatus : CardStatus, IActiveable
{
    public float coolTime;
    public ActiveCardStatus() : base()
    {
        coolTime = 0;
    }
    public abstract void Active();

}
public abstract class PassiveCardStatus : CardStatus
{
    public PassiveCardStatus()
    {
    }
    public abstract void SetPassive();

}
public class SmiteCardStatus : ActiveCardStatus
{
    public float attDmg;
    public SmiteCardStatus() : base()
    {
        name = "강타";
        type = "Attacker";
        coolTime = 8f;
        hp = 500f;
        atk = 100f;
        attDmg = 3.0f;
        skillInfo = "플레이어 공격력의 300%의 데미지 (쿨타임)" + coolTime.ToString() + "초";
    }

    public override void Active()
    {
        GameManager.Instance.enemy.Hit(GameManager.Instance.player.Atk * 3f);
        //효과 300% 데미지
    }
    public override void StatusInit()
    {
        GameManager.Instance.player.MaxHp += this.hp;
        GameManager.Instance.player.Atk += this.atk;
    }
}
public class QuickCardStatus : ActiveCardStatus
{
    public float attDmg;
    public QuickCardStatus() : base()
    {
        name = "속공";
        type = "Attacker";
        coolTime = 4f;
        hp = 500f;
        atk = 90f;
        attDmg = 1.8f;
        skillInfo = "플레이어 공격력의 180%의 데미지 (쿨타임)" + coolTime.ToString() + "초";
    }

    public override void Active()
    {
        GameManager.Instance.enemy.Hit(GameManager.Instance.player.Atk * 1.8f);
        //효과  180% 데미지
    }
    public override void StatusInit()
    {
        GameManager.Instance.player.MaxHp += this.hp;
        GameManager.Instance.player.Atk += this.atk;
    }
}
public class AngerCardStatus : ActiveCardStatus, IBuffable
{
    public float attUp;
    private float clearTime;
    public float ClearTime
    {
        get { return clearTime; }
        set
        {
            clearTime = value;
        }
    }

    public AngerCardStatus() : base()
    {
        name = "분노";
        type = "Attacker";
        coolTime = 15f;
        hp = 500f;
        atk = 150f;
        attUp = 1.5f;
        clearTime = 10f;
        skillInfo = "플레이어 공격력을 10초간 1.5배";
    }

    public override void Active()
    {
        GameManager.Instance.player.Atk *= 1.5f;
        //효과 공격력 10초동안 1.5배 증가
    }
    public void BuffClear ()
    {
        GameManager.Instance.player.Atk /= 1.5f;
    }
    public override void StatusInit()
    {
        GameManager.Instance.player.MaxHp += this.hp;
        GameManager.Instance.player.Atk += this.atk;
    }
}
public class RageCardStatus : PassiveCardStatus
{
    public float attRate;
    public RageCardStatus() : base()
    {
        name = "격노";
        type = "Attacker";
        hp = 500f;
        atk = 100f;
        attRate = 1.2f;
        skillInfo = "플레이어 공격력의 20%증가";
    }

    public override void SetPassive()
    {
        GameManager.Instance.player.Atk *= attRate;
        //효과 공격력 20% 증가
    }
    public override void StatusInit()
    {
        GameManager.Instance.player.MaxHp += this.hp;
        GameManager.Instance.player.Atk += this.atk;
    }
}
public class LuckCardStatus : PassiveCardStatus
{
    public float attRate;
    public LuckCardStatus() : base()
    {
        name = "행운";
        type = "Attacker";
        hp = 500f;
        atk = 150f;
        skillInfo = "일정확률로 플레이어의 공격력이 더 크게 상승합니다.";
        attRate = 1f;
    }
    public override void StatusInit()
    {
        GameManager.Instance.player.MaxHp += this.hp;
        GameManager.Instance.player.Atk += this.atk;
    }

    public override void SetPassive()
    {
       
        //GameManager.Instance.enemy.Hit(GameManager.Instance.player.Atk * 2f);
    }
    public void LuckActive()
    {
        int temp = UnityEngine.Random.Range(0, 3);
        switch (temp)
        {
            case 0:
                attRate = 1.05f;
                break;
            case 1:
                attRate = 1.15f;
                break;
            case 2:
                attRate = 1.3f;
                break;
        }
        GameManager.Instance.player.Atk *= attRate;
    }
}
public class DefenseCardStatus : ActiveCardStatus, IBuffable
{
    public float defenseRate;
    private float clearTime;
    public float ClearTime
    {
        get { return clearTime; }
        set
        {
            clearTime = value;
        }
    }
    public DefenseCardStatus() : base()
    {
        name = "방어";
        type = "Defensive";
        coolTime = 15f;
        hp = 1000f;
        atk = 50f;
        defenseRate = 0.8f;
        clearTime = 12f;
        skillInfo = "12초간 피해 감소 20%증가 (쿨타임)" + coolTime.ToString() + "초";
    }

    public override void Active()
    {
        GameManager.Instance.player.DefenseRate *= defenseRate; 
        //효과 긴시간(12초) 피해감소 (피해 감소 20%)
    }
    public void BuffClear()
    {
        GameManager.Instance.player.DefenseRate /= defenseRate;
    }
    public override void StatusInit()
    {
        GameManager.Instance.player.MaxHp += this.hp;
        GameManager.Instance.player.Atk += this.atk;
    }
}
public class IronwallCardStatus : ActiveCardStatus, IBuffable
{
    public float defenseRate;
    private float clearTime;
    public float ClearTime
    {
        get { return clearTime; }
        set
        {
            clearTime = value;
        }
    }
    public IronwallCardStatus() : base()
    {
        name = "철벽";
        type = "Defensive";
        coolTime = 10f;
        hp = 600f;
        atk = 50f;
        defenseRate = 0.4f;
        clearTime = 3f;
        skillInfo = "3초간 피해 감소 60%증가 (쿨타임)" + coolTime.ToString() + "초";
    }

    public override void Active()
    {
        GameManager.Instance.player.DefenseRate *= defenseRate;
        //효과 짧은시간(3초) 피해감소 크게증가 (피해감소 60%)
    }
    public void BuffClear()
    {
        GameManager.Instance.player.DefenseRate /= defenseRate;
    }
    public override void StatusInit()
    {
        GameManager.Instance.player.MaxHp += this.hp;
        GameManager.Instance.player.Atk += this.atk;
    }
}
public class SmashCardStatus : ActiveCardStatus, IBuffable
{
    public float defenseBreakRate;
    private float clearTime;
    public float ClearTime
    {
        get { return clearTime; }
        set
        {
            clearTime = value;
        }
    }
    public SmashCardStatus() : base()
    {
        name = "분쇄";
        type = "Defensive";
        coolTime = 20f;
        hp = 500f;
        atk = 80f;
        defenseBreakRate = 1.2f;
        clearTime = 15f;
        skillInfo = "15초간 적 피해 20%증가 (쿨타임)" + coolTime.ToString() + "초";
    }

    public override void Active()
    {
        GameManager.Instance.enemy.DefenseRate *= defenseBreakRate;
        //효과 적 방어력 감소(15초)
    }
    public void BuffClear()
    {
        GameManager.Instance.enemy.DefenseRate /= defenseBreakRate;
    }
    public override void StatusInit()
    {
        GameManager.Instance.player.MaxHp += this.hp;
        GameManager.Instance.player.Atk += this.atk;
    }
}
public class GritCardStatus : PassiveCardStatus
{
    public float hpUp;
    public GritCardStatus() : base()
    {
        name = "근성";
        type = "Defensive";
        hp = 1000f;
        atk = 50f;
        hpUp = 1.2f;
        skillInfo = "플레이어의 체력 20% 증가";
    }

    public override void SetPassive()
    {
        //효과 최대체력 20%증가
        GameManager.Instance.player.MaxHp *= this.hpUp;
    }
    public override void StatusInit()
    {
        GameManager.Instance.player.MaxHp += this.hp;
        GameManager.Instance.player.Atk += this.atk;
    }
}
public class PatienceCardStatus : PassiveCardStatus
{
    public float defenseUp;
    public PatienceCardStatus() : base()
    {
        name = "인내";
        type = "Defensive";
        hp = 1000f;
        atk = 50f;
        defenseUp = 0.9f;
        skillInfo = "피해감소 10% 증가";
    }

    public override void SetPassive()
    {
        //효과 피해감소량 10% 증가
        GameManager.Instance.player.DefenseRate *= this.defenseUp;
    }
    public override void StatusInit()
    {
        GameManager.Instance.player.MaxHp += this.hp;
        GameManager.Instance.player.Atk += this.atk;
    }
}
public class HealCardStatus : ActiveCardStatus
{
    public float healAmount;
    public HealCardStatus() : base()
    {
        name = "치료";
        type = "Healing";
        coolTime = 7f;
        hp = 1000f;
        atk = 50f;
        healAmount = 3000f;
        skillInfo = "플레이어의 체력 3000회복\n 회복량 적용 (쿨타임)" + coolTime.ToString() + "초";
    }

    public override void Active()
    {
        GameManager.Instance.player.Hp += healAmount;
        //효과 짧은 쿨타임의 낮은 회복량의 치유
    }
    public override void StatusInit()
    {
        GameManager.Instance.player.MaxHp += this.hp;
        GameManager.Instance.player.Atk += this.atk;
    }
}
public class FriendShipCardStatus : ActiveCardStatus
{
    public float healAmount;
    public FriendShipCardStatus() : base()
    {
        name = "우정";
        type = "Healing";
        coolTime = 20f;
        hp = 1000f;
        atk = 50f;
        healAmount = 8000f;
        skillInfo = "플레이어의 체력 8000회복\n 회복량 적용 (쿨타임)" + coolTime.ToString() + "초";
    }

    public override void Active()
    {
        GameManager.Instance.player.Hp += healAmount;
        //효과 높은 회복량의 치유
    }
    public override void StatusInit()
    {
        GameManager.Instance.player.MaxHp += this.hp;
        GameManager.Instance.player.Atk += this.atk;
    }
}
public class EncourageCardStatus : PassiveCardStatus
{
    public float healUp;
    public float attUp;
    public float defenseUp;
    public float hpUp;
    public EncourageCardStatus() : base()
    {
        name = "격려";
        type = "Healing";
        hp = 500f;
        atk = 50f;
        healUp = 1.05f;
        attUp = 1.05f;
        defenseUp = 0.95f;
        hpUp = 1.05f;
        skillInfo = "모든 능력치 5% 상승";
    }

    public override void SetPassive()
    {
        Debug.Log(healUp + "/" + attUp + "/" + defenseUp + "/" + hpUp);
        //효과 모든 스탯 증가 hp 방어력 공격력
        GameManager.Instance.player.HealRate *= this.healUp;
        GameManager.Instance.player.Atk *= this.attUp;
        GameManager.Instance.player.DefenseRate *= this.defenseUp;
        GameManager.Instance.player.MaxHp *= this.hpUp;
    }
    public override void StatusInit()
    {
        GameManager.Instance.player.MaxHp += this.hp;
        GameManager.Instance.player.Atk += this.atk;
    }
}
public class PrayCardStatus : PassiveCardStatus
{
    public float healUp;
    public PrayCardStatus() : base()
    {
        name = "기도";
        type = "Healing";
        healUp = 1.2f;
        hp = 1000f;
        atk = 50f;
        skillInfo = "플레이어의 회복량 20%증가";
    }

    public override void SetPassive()
    {
        //효과 회복량 증가
        GameManager.Instance.player.HealRate *= this.healUp;
    }
    public override void StatusInit()
    {
        GameManager.Instance.player.MaxHp += this.hp;
        GameManager.Instance.player.Atk += this.atk;
    }
}

public enum CARD_TYPE
{
    Smite,//강타
    Quick,//속공
    Anger,//분노

    Rage,//격노
    Luck,//행운

    Defense,//방어
    Ironwall,//철벽
    Smash,//분쇄

    Grit,//근성
    Patience,//인내

    Heal,//치료
    Friendship,//우정

    Encourage,//격려
    Pray//기도
}
public class Card : MonoBehaviour,IEquatable<Card>,IComparable<Card>
{
    [SerializeField]
    public CARD_TYPE typeOfCard;//필요한거
    

    [SerializeField]
    public string cardName;
    [SerializeField]
    public float atk;
    [SerializeField]
    public float hp;
    [SerializeField]
    public string type;

    public bool isInit;
    private int exp;
    public int Exp
    {
        get { return exp; }
        set {
            if (level < GameManager.Instance.essentialExp.Count)
            {
                while (value > GameManager.Instance.essentialExp[Level])
                {
                    value = value - GameManager.Instance.essentialExp[Level];
                    exp = value;
                    Level++;//while로 돌리면 좋을거 같긴한데 일단 보류
                }
            }
            exp = value;
        }
    }
    [SerializeField]
    private int level;
    public int Level
    {
        get { return level; }
        set {
            if(value > level)
            {
                atk *= 1.05f;
                hp *= 1.05f;
                level = value;
            }
            else
            {
                level = value;
            }
        }
    }
    public string skillInfo;
    protected void Start()
    {
        //Debug.Log("cardInst");
        isInit = true;
        level = 1;
        //cardStrategy = CardManager.Instance.thisCard[typeOfCard];

        //Debug.Log(cardStrategy.coolTime);
        //Debug.Log(cardStrategy.name);
    }
    public string sortString;
    public bool Equals(Card other)
    {
        
        return sortString.Equals(sortString);
    }

    public int CompareTo(Card other)
    {
        return sortString.CompareTo(other.sortString);
    }
    public void StatusInit()
    {
        GameManager.Instance.player.MaxHp += hp;
        GameManager.Instance.player.Atk += atk;
    }
}
