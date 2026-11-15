using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemIconController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI costText;
    [SerializeField]
    private SizeTransition sizeTransition;

    private int cost;
    private Entity parentEntity;
    private ShopController shopController;
    private Item parentItem;

    public int Cost
    {
        get
        {
            return cost;
        }
        set
        {
            cost = value;

            costText.text = cost.ToString();
        }
    }

    public Entity ParentEntity
    {
        get
        {
            return parentEntity;
        }
    }

    public Item ParentItem
    {
        get
        {
            return parentItem;
        }
    }

    private ShopItemType itemType;

    public void Init(ShopController shopController, ShopItemType itemType)
    {
        this.shopController = shopController;
        this.itemType = itemType;

        switch (itemType)
        {
            case ShopItemType.Entity:
                Entity entity = EntityDB.GetRandomEntityWeighted();
                parentEntity = entity;
                image.sprite = parentEntity.GetSprite();

                switch (parentEntity.EntityRarity)
                {
                    case Rarity.Common:
                        Cost = 1;
                        break;
                    case Rarity.Rare:
                        Cost = 2;
                        break;
                    case Rarity.Epic:
                        Cost = 5;
                        break;
                }

                break;
            case ShopItemType.Preservative:
                Item item = ItemDB.GetRandomItem();
                parentItem = item;
                Cost = parentItem.Cost;
                image.sprite = parentItem.GetSprite();

                break;
            default:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.Play(7, -0.05f, 0.05f);

        sizeTransition.StartTransition(SizeTransition.TransitionDirection.startToEnd);

        if (ParentEntity != null)
        {
            UIHelper.Instance.InstantiateInfoPanel(transform, transform.position, new Vector2(-75f, 0), TextAnchor.MiddleRight, parentEntity);
        }

        if (ParentItem != null)
        {
            UIHelper.Instance.InstantiateInfoPanel(transform, transform.position, new Vector2(-75f, 0), TextAnchor.MiddleRight, parentItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        sizeTransition.StartTransition(SizeTransition.TransitionDirection.endToStart);

        UIHelper.Instance.ClearInfoPanels();
    }

    public void OnPointerDown(PointerEventData eventDAta)
    {
        if (CurrencyManager.Instance.Magnets >= cost)
        {
            GameManager.Instance.UpdateCurrencyText();

            switch (itemType)
            {
                case ShopItemType.Entity:
                    if (!GameManager.Instance.FridgeController.IsFridgeFull())
                    {
                        shopController.PurchaseItem(gameObject, cost, ParentEntity);
                    }
                    else
                    {
                        AudioManager.Instance.Play(6);
                    }

                    break;
                case ShopItemType.Preservative:
                    StartCoroutine(EquipItemCoroutine());

                    break;
                default:
                    break;
            }
        }
        else
        {
            AudioManager.Instance.Play(6);
        }
    }

    public IEnumerator EquipItemCoroutine()
    {
        shopController.SetEquipPanel(true);
        shopController.SetEquipText(parentItem.ItemName);
        shopController.QueryingForEquip = true;
        shopController.QueriedItem = parentItem;

        yield return new WaitUntil(() => shopController.SelectedEntity != -1 || !shopController.EquipPanel.gameObject.activeInHierarchy);

        if (shopController.SelectedEntity != -1)
        {
            shopController.PurchaseItem(gameObject, cost, ParentItem);
            shopController.SetEquipPanel(false);
        }

        shopController.QueryingForEquip = false;
        shopController.QueriedItem = null;
        shopController.SelectedEntity = -1;
    }
}
