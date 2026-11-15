using System.Collections.Generic;
using UnityEngine;

public class SkillDB 
{
    public static List<Skill> skills = new List<Skill>()
    {
        new Skill(0, "Basic Shot", SkillType.Active, 3, 2, 1),
        new Skill(1, "Triple Shot", SkillType.Active, 2, 3, 0.5f),
        new Skill(2, "Spiral Shot", SkillType.Active, 3, 1.5f, 0.1f),
        new Skill(3, "Bounce", SkillType.Passive, 0, 0, 0),
        new Skill(4, "Impact", SkillType.Passive, 3, 0, 2.5f),
        new Skill(5, "Sweet Skin", SkillType.Passive, 0, 0, 0),
        new Skill(6, "Thorn Rock", SkillType.Active, 4, 5, 0),
        new Skill(7, "Thorns", SkillType.Passive, 0, 0, 0),
        new Skill(8, "Sour Skin", SkillType.Passive, 0, 0, 0),
        new Skill(9, "Launch", SkillType.Active, 0, 3, 0),
        new Skill(10, "Quintuple Shot", SkillType.Active, 3, 5, 0.4f)
    };

    public static Dictionary<int, string> skillDescriptions = new Dictionary<int, string>()
    {
        {0, "Shoot one bullet at the nearest enemy" },
        {1, "Shoot three bullets at the nearest enemy"},
        {2, "Shoot a bullet that spirals around the user" },
        {3, "Whenever this perishable hits a wall,\nit has a 10% chance to recover 1 hp" },
        {4, "Whenever this perishable hits another perishable,\nit emits a damaging pulse" },
        {5, "Whenever this perishable gets hit, it has a 30% chance to gain 1 Strength" },
        {6, "Place a rock covered in thorns which lasts 3 seconds" },
        {7, "Whenever this perishable hits another perishable,\nthe other perishable takes 1 damage" },
        {8, "Whenever this perishable hits another allied citrus,\nthe other citrus gains 1 Strength" },
        {9, "Launch in a random direction" },
    };
}
