using UnityEngine;

public class Item 
{
    private int itemId;
    private string itemName;
    private string itemDescription;
    private int cost;

    public int ItemID
    {
        get
        {
            return itemId;
        }
    }

    public string ItemName
    {
        get
        {
            return itemName;
        }
    }

    public int Cost
    {
        get
        {
            return cost;
        }
    }

    public Item(int itemId, string itemName, int cost)
    {
        this.itemId = itemId;
        this.itemName = itemName;
        this.cost = cost;
    }

    public override bool Equals(object other)
    {
        if (other == null)
        {
            return false;
        }

        Item item = other as Item;

        if (item.itemId == itemId)
        {
            return true;
        }

        return false;
    }

    public Sprite GetSprite()
    {
        if (itemId < SpriteDB.Instance.itemSprites.Count)
        {
            return SpriteDB.Instance.itemSprites[itemId];
        }

        return SpriteDB.Instance.itemSprites[0];
    }

    public string GetDescription()
    {
        return ItemDB.itemDescriptions[itemId];
    }
}
