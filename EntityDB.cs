using System.Collections.Generic;
using UnityEngine;

public class EntityDB 
{

    public static Dictionary<int, List<Skill>> defaultSkillList = new Dictionary<int, List<Skill>>()
    {
        {
            0, new List<Skill>()
            {
                SkillDB.skills[0]
            }
        },
        {
            1, new List<Skill>()
            {
                SkillDB.skills[1]
            }
        },
        {
            2, new List<Skill>()
            {
                SkillDB.skills[2]
            }
        },
        {
            3, new List<Skill>()
            {
                SkillDB.skills[3],
                SkillDB.skills[4],
            } 
        },
        {
            4, new List<Skill>()
            {
                SkillDB.skills[0],
                SkillDB.skills[5]
            }
        },
        {
            5, new List<Skill>()
            {
                SkillDB.skills[6],
                SkillDB.skills[7]
            }
        },
        {
            6, new List<Skill>()
            {
                SkillDB.skills[8],
                SkillDB.skills[9]
            }
        },
        {
            7, new List<Skill>()
            {
                SkillDB.skills[10],
                SkillDB.skills[1],
                SkillDB.skills[0],
                SkillDB.skills[2],
            }
        }
    };


    public static List<Entity> entities = new List<Entity>()
    {
        new Entity(0, Rarity.Common, "Apple", 20, 3, 4,
            new List<string>()
            {
                "Fruit", "Sweet"
            }),
        new Entity(1, Rarity.Common, "Orange", 18, 2, 4,
            new List<string>()
            {
                "Fruit", "Citrus", "Sweet", "Sour"
            }),
        new Entity(2, Rarity.Common, "Pear", 24, 3, 5,
            new List<string>()
            {
                "Fruit", "Sweet"
            }),
        new Entity(3, Rarity.Rare, "Cabbage", 30, 2, 3,
            new List<string>()
            {
                "Vegetable"
            }),
        new Entity(4, Rarity.Rare, "Mango", 24, 3, 2,
            new List<string>()
            {
                "Fruit", "Sweet"
            }),
        new Entity(5, Rarity.Epic, "Durian", 28, 3, 5,
            new List<string>()
            {
                "Fruit", "Sweet"
            }),
        new Entity(6, Rarity.Rare, "Pomelo", 30, 3, 3,
            new List<string>()
            {
                "Fruit", "Citrus", "Bitter"
            }),
        new Entity(7, Rarity.Legendary, "Watermelon", 100, 5, 100,
            new List<string>()
            {
                "Fruit", "Sweet", "Boss"
            }),
    };

    public static Dictionary<int, string> entityDescriptions = new Dictionary<int, string>()
    {
        {0, "Red, sweet, and delicious"},
        {1, "Likely from Florida"},
        {2, "Take a bite out of this pear shaped bod"},
        {3, "Put this in a ShackBurger please" },
        {4, "Mango smoothie from McDonalds" },
        {5, "Smells real bad and hurts real bad" },
        {6, "Humungous big large orange, per se" },
        {7, "Watermelon sugar, HIIIGH" },
    };

    public static List<float> rarityWeights = new List<float>()
    {
        0.8f,
        0.17f,
        0.03f,
    };

    public static Rarity GetRarityWeighted()
    {
        List<Rarity> entityRarities = new List<Rarity>()
        {
            Rarity.Common,
            Rarity.Rare,
            Rarity.Epic,
        };

        List<int> indices = new List<int>();

        for (int i = 0; i < entityRarities.Count; i++)
        {
            for (int j = 0; j < rarityWeights[i] * 10; j++)
            {
                indices.Add(i);
            }
        }

        int randomIndex = Random.Range(0, indices.Count);

        return entityRarities[indices[randomIndex]];
    }

    public static List<Entity> FilterEntitiesByRarity(Rarity rarity)
    {
        List<Entity> retEntityList = new List<Entity>();

        foreach (Entity entity in entities)
        {
            if (entity.EntityRarity == rarity)
            {
                retEntityList.Add(entity);
            }
        }

        return retEntityList;
    }

    public static Entity GetRandomEntityByRarity(Rarity rarity)
    {
        List<Entity> entities = FilterEntitiesByRarity(rarity);

        int randomIndex = Random.Range(0, entities.Count);

        return new Entity(entities[randomIndex]);
    }

    public static Entity GetRandomEntityWeighted()
    {
        Rarity rarity = GetRarityWeighted();
        Entity entity = GetRandomEntityByRarity(rarity);

        return entity;
    }
}
