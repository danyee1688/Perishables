using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootPickupable : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private Image image;

    private LootType type;
    [SerializeField]
    private float speed = 0.1f;

    private void Update()
    {
        TryMove();

        transform.rotation *= Quaternion.Euler(0, 0, 0.25f);
    }

    public void Init(LootType type)
    {
        this.type = type;
    }

    public void TryMove()
    {
        Vector2 mousePos = UIHelper.Instance.GetMousePos();

        float distance = Vector2.Distance(mousePos, transform.position);

        float speedFactor = Mathf.Clamp(Mathf.Pow(3 * (1 - distance), 5), 0, 50);

        if (Vector2.Distance(mousePos, transform.position) < 0.5f)
        {
            transform.position = Vector2.MoveTowards(transform.position, mousePos, (speed / 100) * speedFactor * Time.deltaTime);
        }
        else if (Vector2.Distance(mousePos, transform.position) < 0.75f)
        {
            transform.position = Vector2.MoveTowards(transform.position, mousePos, (speed / 100) * Time.deltaTime);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (type)
        {
            case LootType.Currency:
                CurrencyManager.Instance.Magnets++;

                GameManager.Instance.UpdateCurrencyText();

                AudioManager.Instance.Play(8);

                break;
            default:
                break;
        }

        Instantiate(InstantiableLibrary.Instance.CollectItemPE, transform.position, Quaternion.Euler(0, 0, 67.5f));

        Destroy(gameObject);
    }
}

[System.Serializable]
public enum LootType
{
    Currency,
}
