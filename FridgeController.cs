using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class FridgeController : MonoBehaviour
{
    [Header("Layouts/Panels")]
    [SerializeField]
    private Transform entityIconLayout;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject entityIconPrefab;

    [Header("Text")]
    [SerializeField]
    private TextMeshProUGUI currencyText;
    [SerializeField]
    private TextMeshProUGUI capacityText;

    private List<Entity> entities = new List<Entity>();
    private List<GameObject> entityIcons = new List<GameObject>();
    private int maxFridgeSize = 4;

    public int MaxFridgesSize
    {
        get
        {
            return maxFridgeSize;
        }
        set
        {
            maxFridgeSize = value;
        }
    }

    public List<Entity> Entities
    {
        get
        {
            return entities;
        }
    }

    public void AddToFridge(Entity entity)
    {
        entities.Add(entity);

        UpdateFridgeIcons();
        UpdateCapacityText();
    }

    public void RemoveFromFridge(Entity entity)
    {
        entities.Remove(entity);

        UpdateFridgeIcons();
        UpdateCapacityText();
    }

    public void UpdateFridge()
    {
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            entities[i].EntityRotTime--;

            if (entities[i].EntityRotTime <= 0)
            {
                UIHelper.Instance.ShowPopup($"{entities[i].EntityName} has rotted away!");

                entities.RemoveAt(i);
            }
        }

        UpdateFridgeIcons();
        UpdateCapacityText();
    }

    public void ClearFridgeIcons()
    {
        for (int i = entityIcons.Count - 1; i >= 0; i--)
        {
            Destroy(entityIcons[i]);
            entityIcons.RemoveAt(i);
        }
    }

    public void UpdateFridgeIcons()
    {
        ClearFridgeIcons();

        for (int i = 0; i < entities.Count; i++)
        {
            GameObject entityIconGO = Instantiate(entityIconPrefab, entityIconLayout);
            EntityIconController entityIconController = entityIconGO.GetComponent<EntityIconController>();

            entityIconController.Init(this, entities[i], i);

            entityIcons.Add(entityIconGO);
        }
    }

    public void SetCurrencyText(string text)
    {
        currencyText.text = text;
    }

    public bool IsFridgeFull()
    {
        if (entities.Count < maxFridgeSize)
        {
            return false;
        }

        return true;
    } 

    public void UpdateCapacityText()
    {
        capacityText.text = $"{entities.Count.ToString()}/{maxFridgeSize.ToString()}";
    }

    public void AddItemToEntity(Item item, int index)
    {
        Entity entity = entities[index];

        entity.EquippedItem = item;
    }
}
