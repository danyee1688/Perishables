using UnityEngine;

public class Skill 
{
    private int skillId;
    private string skillName;
    private string skillDescription;
    private int skillLevel;
    private int skillBaseDamage;
    private int skillDamage;
    private float skillBaseCd;
    private float skillCd;
    private float skillBaseKnockback;
    private float skillKnockback;
    private SkillType skillType;

    public string SkillName
    {
        get
        {
            return skillName;
        }
    }
    public int SkillId
    {
        get
        {
            return skillId;
        }
    }

    public SkillType SkillType
    {
        get
        {
            return skillType;
        }
    }


    public int SkillDamage
    {
        get
        {
            return skillDamage;
        }
        set
        {
            skillDamage = value;
        }
    }

    public float SkillKnockback
    {
        get
        {
            return skillKnockback;
        }
    }

    public float SkillCd
    {
        get
        {
            return skillCd;
        }
        set
        {
            skillCd = value;
        }
    }

    public Skill(int id, string skillName, SkillType skillType, int skillBaseDamage, float skillBaseCd, float skillBaseKnockback)
    {
        skillId = id;
        this.skillName = skillName;

        this.skillType = skillType;

        this.skillBaseDamage = skillBaseDamage;
        skillDamage = skillBaseDamage;

        this.skillBaseCd = skillBaseCd;
        skillCd = skillBaseCd;

        this.skillBaseKnockback = skillBaseKnockback;
        skillKnockback = skillBaseKnockback;
    }

    public Skill(Skill skill)
    {
        this.skillId = skill.SkillId;
        this.skillType = skill.skillType;
        this.skillBaseDamage = skill.skillBaseDamage;
        skillDamage = this.skillBaseDamage;
        this.skillBaseCd = skill.skillBaseCd;
        skillCd = skillBaseCd;
        this.skillBaseKnockback = skill.skillBaseKnockback;
        skillKnockback = skillBaseKnockback;
    }

    public string GetDescription()
    {
        return SkillDB.skillDescriptions[skillId];
    }
}

[System.Serializable]
public enum SkillType
{
    Active,
    Passive
}