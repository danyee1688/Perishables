using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private Transform entityLayout;
    [SerializeField]
    private Transform itemLayout;
    [SerializeField]
    private GameObject shopItemPrefab;
    [SerializeField]
    private FridgeController fridgeController;
    [SerializeField]
    private TextMeshProUGUI entityRefreshCostText;
    [SerializeField]
    private TextMeshProUGUI itemRefreshCostText;
    [SerializeField]
    private TextMeshProUGUI equipItemNameText;
    [SerializeField]
    private Transform equipPanel;

    private List<GameObject> shopItemEntityIcons = new List<GameObject>();
    private List<GameObject> shopItemItemIcons = new List<GameObject>();

    private int entityRefreshCost = 1;
    private int itemRefreshCost = 1;

    private bool queryingForEquip = false;
    private Item queriedItem = null;
    private int selectedEntity = -1;

    public bool QueryingForEquip
    {
        get
        {
            return queryingForEquip;
        }
        set
        {
            queryingForEquip = value;
        }
    }

    public Item QueriedItem
    {
        get
        {
            return queriedItem;
        }
        set
        {
            queriedItem = value;
        }
    }

    public int SelectedEntity
    {
        get
        {
            return selectedEntity;
        }
        set
        {
            selectedEntity = value;

            Debug.Log("new selected entity: " + selectedEntity);
        }
    }

    public int EntityRefreshCost
    {
        get
        {
            return entityRefreshCost;
        }
        set
        {
            entityRefreshCost = value;

            entityRefreshCostText.text = $"{entityRefreshCost.ToString()}x";
        }
    }

    public int ItemRefreshCost
    {
        get
        {
            return itemRefreshCost;
        }
        set
        {
            itemRefreshCost = value;

            itemRefreshCostText.text = $"{itemRefreshCost.ToString()}x";
        }
    }

    public Transform EquipPanel
    {
        get
        {
            return equipPanel;
        }
    }

    public void SetupEntities()
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnShopItem(ShopItemType.Entity, entityLayout);
        };
    }

    public void SetupItems()
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnShopItem(ShopItemType.Preservative, itemLayout);
        };
    }

    public void ClearEntities()
    {
        for (int i = shopItemEntityIcons.Count - 1; i >= 0; i--)
        {
            Destroy(shopItemEntityIcons[i]);
            shopItemEntityIcons.RemoveAt(i);
        }
    }

    public void ClearItems()
    {
        for (int i = shopItemItemIcons.Count - 1; i >= 0; i--)
        {
            Destroy(shopItemItemIcons[i]);
            shopItemItemIcons.RemoveAt(i);
        }
    }

    public void RefreshEntities(bool hasCost)
    {
        if (hasCost)
        {
            CurrencyManager.Instance.Magnets -= entityRefreshCost;
            GameManager.Instance.UpdateCurrencyText();
        }

        ClearEntities();
        SetupEntities();

        EntityRefreshCost++;
    }

    public void RefreshItems(bool hasCost)
    {
        if (hasCost)
        {
            CurrencyManager.Instance.Magnets -= itemRefreshCost;
            GameManager.Instance.UpdateCurrencyText();
        }

        ClearItems();
        SetupItems();

        ItemRefreshCost++;
    }

    public void ResetEntityRefreshCost()
    {
        EntityRefreshCost = 1;
    }

    public void ResetItemRefreshCost()
    {
        ItemRefreshCost = 1;
    }

    public void SpawnShopItem(ShopItemType itemType, Transform parent)
    {
        GameObject shopItemGO = Instantiate(shopItemPrefab, parent);  
        ShopItemIconController shopItemController = shopItemGO.GetComponent<ShopItemIconController>();  

        shopItemController.Init(this, itemType);

        switch (itemType)
        {
            case ShopItemType.Entity:
                shopItemEntityIcons.Add(shopItemGO);

                break;
            case ShopItemType.Preservative:
                shopItemItemIcons.Add(shopItemGO);

                break;
        }
    }

    public void PurchaseItem(GameObject itemIcon, int cost, Entity entity)
    {
        CurrencyManager.Instance.Magnets -= cost;
        GameManager.Instance.UpdateCurrencyText();

        fridgeController.AddToFridge(entity);

        UIHelper.Instance.ClearInfoPanels();

        RemoveItem(itemIcon);

        AudioManager.Instance.Play(5);
    }

    public void PurchaseItem(GameObject itemIcon, int cost, Item item)
    {
        CurrencyManager.Instance.Magnets -= cost;
        GameManager.Instance.UpdateCurrencyText();

        fridgeController.AddItemToEntity(item, SelectedEntity);
        fridgeController.UpdateFridgeIcons();

        RemoveItem(itemIcon);

        UIHelper.Instance.ClearInfoPanels();

        if (item.ItemID == 3)
        {
            fridgeController.MaxFridgesSize++;
            fridgeController.UpdateCapacityText();
        }

        AudioManager.Instance.Play(5);
    }

    public void RemoveItem(GameObject item)
    {
        shopItemEntityIcons.Remove(item);
        Destroy(item);
    }

    public void SetEquipPanel(bool active)
    {
        equipPanel.gameObject.SetActive(active);
    }

    public void SetEquipText(string text)
    {
        equipItemNameText.text = text;
    }
}

[System.Serializable]
public enum ShopItemType
{
    Entity,
    Preservative
}