using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/Skill", order = 0)]
public class Skill : ScriptableObject
{
    public Skill skill1;
    public Skill skill2;
    [SerializeField]
    private float skillDmgRate;
    public float SkillDmgRate
    {
        get { return skillDmgRate; }
    }
    [SerializeField]
    private string skillName;
    public string SkillName
    {
        get { return skillName; }
    }
    [SerializeField]
    private float skillUseTime;
    public float SkillUseTime
    {
        get { return skillUseTime; }
    }
}
