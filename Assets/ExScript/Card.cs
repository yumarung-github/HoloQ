using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterfaceSpace;
using System;

namespace InterfaceSpace
{
    public interface IActiveable//�����Ե� �ʿ� //�����̳� ȿ������
    {
        public void Active();//target.hp��������� �ȿ��� �˾Ƽ� ó�����ֱ�
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
        name = "��Ÿ";
        type = "Attacker";
        coolTime = 8f;
        hp = 500f;
        atk = 100f;
        attDmg = 3.0f;
        skillInfo = "�÷��̾� ���ݷ��� 300%�� ������ (��Ÿ��)" + coolTime.ToString() + "��";
    }

    public override void Active()
    {
        GameManager.Instance.enemy.Hit(GameManager.Instance.player.Atk * 3f);
        //ȿ�� 300% ������
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
        name = "�Ӱ�";
        type = "Attacker";
        coolTime = 4f;
        hp = 500f;
        atk = 90f;
        attDmg = 1.8f;
        skillInfo = "�÷��̾� ���ݷ��� 180%�� ������ (��Ÿ��)" + coolTime.ToString() + "��";
    }

    public override void Active()
    {
        GameManager.Instance.enemy.Hit(GameManager.Instance.player.Atk * 1.8f);
        //ȿ��  180% ������
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
        name = "�г�";
        type = "Attacker";
        coolTime = 15f;
        hp = 500f;
        atk = 150f;
        attUp = 1.5f;
        clearTime = 10f;
        skillInfo = "�÷��̾� ���ݷ��� 10�ʰ� 1.5��";
    }

    public override void Active()
    {
        GameManager.Instance.player.Atk *= 1.5f;
        //ȿ�� ���ݷ� 10�ʵ��� 1.5�� ����
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
        name = "�ݳ�";
        type = "Attacker";
        hp = 500f;
        atk = 100f;
        attRate = 1.2f;
        skillInfo = "�÷��̾� ���ݷ��� 20%����";
    }

    public override void SetPassive()
    {
        GameManager.Instance.player.Atk *= attRate;
        //ȿ�� ���ݷ� 20% ����
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
        name = "���";
        type = "Attacker";
        hp = 500f;
        atk = 150f;
        skillInfo = "����Ȯ���� �÷��̾��� ���ݷ��� �� ũ�� ����մϴ�.";
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
        name = "���";
        type = "Defensive";
        coolTime = 15f;
        hp = 1000f;
        atk = 50f;
        defenseRate = 0.8f;
        clearTime = 12f;
        skillInfo = "12�ʰ� ���� ���� 20%���� (��Ÿ��)" + coolTime.ToString() + "��";
    }

    public override void Active()
    {
        GameManager.Instance.player.DefenseRate *= defenseRate; 
        //ȿ�� ��ð�(12��) ���ذ��� (���� ���� 20%)
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
        name = "ö��";
        type = "Defensive";
        coolTime = 10f;
        hp = 600f;
        atk = 50f;
        defenseRate = 0.4f;
        clearTime = 3f;
        skillInfo = "3�ʰ� ���� ���� 60%���� (��Ÿ��)" + coolTime.ToString() + "��";
    }

    public override void Active()
    {
        GameManager.Instance.player.DefenseRate *= defenseRate;
        //ȿ�� ª���ð�(3��) ���ذ��� ũ������ (���ذ��� 60%)
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
        name = "�м�";
        type = "Defensive";
        coolTime = 20f;
        hp = 500f;
        atk = 80f;
        defenseBreakRate = 1.2f;
        clearTime = 15f;
        skillInfo = "15�ʰ� �� ���� 20%���� (��Ÿ��)" + coolTime.ToString() + "��";
    }

    public override void Active()
    {
        GameManager.Instance.enemy.DefenseRate *= defenseBreakRate;
        //ȿ�� �� ���� ����(15��)
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
        name = "�ټ�";
        type = "Defensive";
        hp = 1000f;
        atk = 50f;
        hpUp = 1.2f;
        skillInfo = "�÷��̾��� ü�� 20% ����";
    }

    public override void SetPassive()
    {
        //ȿ�� �ִ�ü�� 20%����
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
        name = "�γ�";
        type = "Defensive";
        hp = 1000f;
        atk = 50f;
        defenseUp = 0.9f;
        skillInfo = "���ذ��� 10% ����";
    }

    public override void SetPassive()
    {
        //ȿ�� ���ذ��ҷ� 10% ����
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
        name = "ġ��";
        type = "Healing";
        coolTime = 7f;
        hp = 1000f;
        atk = 50f;
        healAmount = 3000f;
        skillInfo = "�÷��̾��� ü�� 3000ȸ��\n ȸ���� ���� (��Ÿ��)" + coolTime.ToString() + "��";
    }

    public override void Active()
    {
        GameManager.Instance.player.Hp += healAmount;
        //ȿ�� ª�� ��Ÿ���� ���� ȸ������ ġ��
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
        name = "����";
        type = "Healing";
        coolTime = 20f;
        hp = 1000f;
        atk = 50f;
        healAmount = 8000f;
        skillInfo = "�÷��̾��� ü�� 8000ȸ��\n ȸ���� ���� (��Ÿ��)" + coolTime.ToString() + "��";
    }

    public override void Active()
    {
        GameManager.Instance.player.Hp += healAmount;
        //ȿ�� ���� ȸ������ ġ��
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
        name = "�ݷ�";
        type = "Healing";
        hp = 500f;
        atk = 50f;
        healUp = 1.05f;
        attUp = 1.05f;
        defenseUp = 0.95f;
        hpUp = 1.05f;
        skillInfo = "��� �ɷ�ġ 5% ���";
    }

    public override void SetPassive()
    {
        Debug.Log(healUp + "/" + attUp + "/" + defenseUp + "/" + hpUp);
        //ȿ�� ��� ���� ���� hp ���� ���ݷ�
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
        name = "�⵵";
        type = "Healing";
        healUp = 1.2f;
        hp = 1000f;
        atk = 50f;
        skillInfo = "�÷��̾��� ȸ���� 20%����";
    }

    public override void SetPassive()
    {
        //ȿ�� ȸ���� ����
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
    Smite,//��Ÿ
    Quick,//�Ӱ�
    Anger,//�г�

    Rage,//�ݳ�
    Luck,//���

    Defense,//���
    Ironwall,//ö��
    Smash,//�м�

    Grit,//�ټ�
    Patience,//�γ�

    Heal,//ġ��
    Friendship,//����

    Encourage,//�ݷ�
    Pray//�⵵
}
public class Card : MonoBehaviour,IEquatable<Card>,IComparable<Card>
{
    [SerializeField]
    public CARD_TYPE typeOfCard;//�ʿ��Ѱ�
    

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
                    Level++;//while�� ������ ������ �����ѵ� �ϴ� ����
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
