using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    private int entityId;
    private string entityName;
    private string entityDescription;
    private int entityBaseHealth;
    private int entitySkillSlots;
    private Rarity entityRarity;
    private int entityBaseRotTime;
    private int entityRotTime;
    private List<Skill> entitySkills = new List<Skill>();
    private List<string> tags = new List<string>();
    private Item equippedItem = null;
    private int strength = 0;
    private float additionalSkillCd;

    public int EntityId
    {
        get
        {
            return entityId;
        }
    }

    public Rarity EntityRarity
    {
        get
        {
            return entityRarity;
        }
    }

    public string EntityName
    {
        get
        {
            return entityName;
        }
    }

    public int EntityBaseHealth
    {
        get
        {
            return entityBaseHealth;
        }
        set
        {
            entityBaseHealth = value;
        }
    }

    public int EntitySkillSlots
    {
        get
        {
            return entitySkillSlots;
        }
    }

    public int EntityRotTime
    {
        get
        {
            return entityRotTime;
        }
        set
        {
            entityRotTime = value;
        }
    }

    public List<Skill> EntitySkills
    {
        get
        {
            return entitySkills;
        }
        set
        {
            entitySkills = value;
        }
    }

    public List<string> Tags
    {
        get
        {
            return tags;
        }
    }

    public Item EquippedItem
    {
        get
        {
            return equippedItem;
        }
        set
        {
            equippedItem = value;
        }
    }

    public int Strength
    {
        get
        {
            return strength;
        }
        set
        {
            strength = value;
        }
    }

    public float AdditionalSkillCd
    {
        get
        {
            return additionalSkillCd;
        }
        set
        {
            additionalSkillCd = value;
        }
    }

    public Entity(int entityId, Rarity rarity, string entityName, int entityBaseHealth, int entitySkillSlots, int entityBaseRotTime, List<string> tags)
    {
        this.entityId = entityId;
        this.entityRarity = rarity;
        this.entityName = entityName;
        this.entityBaseHealth = entityBaseHealth;
        this.entitySkillSlots = entitySkillSlots;
        this.entityBaseRotTime = entityBaseRotTime;
        this.tags = tags;
        entityRotTime = entityBaseRotTime;

        entitySkills = new List<Skill>(EntityDB.defaultSkillList[entityId]);
    }

    public Entity(Entity entity)
    {
        this.entityId= entity.entityId;
        this.entityRarity = entity.entityRarity;
        this.entityName = entity.entityName;
        this.entityDescription = entity.entityDescription;
        this.entityBaseHealth = entity.entityBaseHealth;
        this.entitySkillSlots = entity.entitySkillSlots;
        this.entityBaseRotTime = entity.entityBaseRotTime;
        entitySkills = new List<Skill>(entity.entitySkills);
        this.tags = entity.tags;

        entityRotTime = entityBaseRotTime;
        equippedItem = entity.EquippedItem;
        strength = entity.strength;
        additionalSkillCd = entity.additionalSkillCd;
    }

    public Sprite GetSprite()
    {
        if (entityId < SpriteDB.Instance.entitySprites.Count)
        {
            return SpriteDB.Instance.entitySprites[entityId];
        }

        return SpriteDB.Instance.entitySprites[0];
    }

    public string GetEntityDescription()
    {
        return EntityDB.entityDescriptions[entityId];
    }

    public override bool Equals(object obj)
    {
        if (obj == null )
        {
            return false;
        }

        Entity entity = obj as Entity;

        if (entity.entityId == entityId)
        {
            if (entity.equippedItem == null && equippedItem == null)
            {
                return true;
            }

            if (entity.equippedItem.Equals(equippedItem))
            {
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}