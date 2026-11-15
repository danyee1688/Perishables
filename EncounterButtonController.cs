using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncounterButtonController : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Button button;
    [SerializeField]
    private TextMeshProUGUI descriptionText;

    private EncounterType encounterType;

    private void Start()
    {
        button.onClick.AddListener(SetGameManagerEncounter);
    }

    public void Init(EncounterType encounterType)
    {
        this.encounterType = encounterType;

        Setup();
    }

    public void Setup()
    {
        switch (encounterType)
        {
            case EncounterType.NormalFight:
                descriptionText.text = "Fight";
                image.sprite = SpriteDB.Instance.encounterSprites[0];
                break;
            case EncounterType.HardFight:
                descriptionText.text = "Elite";
                image.sprite = SpriteDB.Instance.encounterSprites[0];
                break;
            case EncounterType.BossFight:
                descriptionText.text = "Boss";
                image.sprite = SpriteDB.Instance.encounterSprites[4];
                break;
            case EncounterType.GroceryStore:
                descriptionText.text = "Market";
                image.sprite = SpriteDB.Instance.encounterSprites[3];
                break;
            case EncounterType.Lottery:
                descriptionText.text = "Gamble";
                image.sprite = SpriteDB.Instance.encounterSprites[1];
                break;
            case EncounterType.Random:
                descriptionText.text = "Random";
                image.sprite = SpriteDB.Instance.encounterSprites[2];
                break;
            default:
                descriptionText.text = "None";
                break;
        }
    }

    public void SetGameManagerEncounter()
    {
        GameManager.Instance.SetEncounterType(encounterType);
    }
}
