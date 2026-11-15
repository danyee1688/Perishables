using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityIconController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Image equippedItemImage;

    private FridgeController fridgeController;
    private Entity parentEntity;
    private Item equippedItem;
    private int index;

    public Item EquippedItem
    {
        get
        {
            return equippedItem;
        }
        set
        {
            equippedItem = value;

            if (equippedItem != null)
            {
                equippedItemImage.gameObject.SetActive(true);

                equippedItemImage.sprite = equippedItem.GetSprite();
            }
            else
            {
                equippedItemImage.gameObject.SetActive(false);
            }
        }
    }

    public void Init(FridgeController fridgeController, Entity entity, int index)
    {
        parentEntity = entity;
        image.sprite = entity.GetSprite();
        EquippedItem = parentEntity.EquippedItem;
        this.fridgeController = fridgeController;
        this.index = index;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.ShopController.QueryingForEquip)
        {
            Debug.Log("Selected for item equip");

            GameManager.Instance.ShopController.SelectedEntity = index;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.Play(7, -0.05f, 0.05f);

        UIHelper.Instance.InstantiateInfoPanel(gameObject.transform, transform.position, new Vector2(75f, 0), TextAnchor.MiddleLeft, parentEntity);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIHelper.Instance.ClearInfoPanels();
    }
}
