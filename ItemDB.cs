using System.Collections.Generic;
using UnityEngine;

public class ItemDB 
{
    public static List<Item> items = new List<Item>()
    {
        new Item(0, "Acetic Acid", 1),
        new Item(1, "Benzoic Acid", 3),
        new Item(2, "Sodium Sulphate", 2),
        new Item(3, "Sodium Sulphate", 15),
    };

    public static Dictionary<int, string> itemDescriptions = new Dictionary<int, string>()
    {
        {0, "Perishable has 3 additional health" },
        {1, "Perishable gains 1 additional strength" },
        {2, "Perishable has -0.1s cooldown to all skills" },
        {3, "Increase max fridge count when added to a perishable" }
    };

    public static Item GetRandomItem()
    {
        int randomIndex = Random.Range(0, items.Count);

        return items[randomIndex];  
    }
}
