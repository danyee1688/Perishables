using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI tagsText;
    [SerializeField]
    private TextMeshProUGUI middleText;
    [SerializeField]
    private TextMeshProUGUI bottomText;
    [SerializeField]
    private TextMeshProUGUI extraText;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnDisable()
    {
        if (!titleText.gameObject.activeInHierarchy)
        {
            titleText.gameObject.SetActive(true);
        }

        titleText.text = "";
        titleText.color = Color.black;

        if (!middleText.gameObject.activeInHierarchy)
        {
            middleText.gameObject.SetActive(true);
        }

        middleText.text = "";
        middleText.color = Color.black;

        if (!bottomText.gameObject.activeInHierarchy)
        {
            bottomText.gameObject.SetActive(true);
        }

        bottomText.text = "";
        bottomText.color = Color.black;

        if (extraText.gameObject.activeInHierarchy)
        {
            extraText.gameObject.SetActive(false);
        }

        extraText.text = "";
        extraText.color = Color.black;
    }

    public void Reset()
    {
        if (!titleText.gameObject.activeInHierarchy)
        {
            titleText.gameObject.SetActive(true);
        }

        titleText.text = "";
        titleText.color = Color.black;

        if (!middleText.gameObject.activeInHierarchy)
        {
            middleText.gameObject.SetActive(true);
        }

        middleText.text = "";
        middleText.color = Color.black;

        if (!bottomText.gameObject.activeInHierarchy)
        {
            bottomText.gameObject.SetActive(true);
        }

        bottomText.text = "";
        bottomText.color = Color.black;

        if (extraText.gameObject.activeInHierarchy)
        {
            extraText.gameObject.SetActive(false);
        }

        extraText.text = "";
        extraText.color = Color.black;
    }

    // Info panel for entity
    public void Instantiate(Entity entity, float fontSize)
    {
        titleText.text = entity.EntityName;
        titleText.fontSize = fontSize;

        string tagString = "";

        for (int i = 0; i < entity.Tags.Count; i++)
        {
            if (i != entity.Tags.Count - 1)
            {
                tagString += entity.Tags[i] + ", ";
            }
            else
            {
                tagString += entity.Tags[i];
            }
        }

        tagsText.text = tagString;
        tagsText.fontSize = fontSize * 0.6f;

        middleText.text = ConvertRarityToString(entity.EntityRarity);
        middleText.fontSize = fontSize * 0.6f;

        bottomText.text = $"HP: {entity.EntityBaseHealth} FRE: {entity.EntityRotTime}";
        bottomText.fontSize = fontSize * 0.75f;

        extraText.gameObject.SetActive(true);
        extraText.text = entity.GetEntityDescription();
        extraText.fontSize = fontSize * 0.75f;
    }

    // Info panel for skills
    public void Instantiate(string title, int skillDamage, float skillCd, string description, float fontSize)
    {
        titleText.text = title;
        titleText.fontSize = fontSize;

        tagsText.gameObject.SetActive(false);

        middleText.text = $"DMG: {skillDamage.ToString()} CD: {skillCd.ToString()}";
        middleText.fontSize = fontSize * 0.75f;

        bottomText.text = description;
        bottomText.fontSize = fontSize * 0.75f;
    }

    // Info panel for items
    public void Instantiate(Item item, float fontSize)
    {
        titleText.text = item.ItemName;
        titleText.fontSize = fontSize;

        tagsText.gameObject.SetActive(false);

        middleText.text = $"Cost: {item.Cost}";
        middleText.fontSize = fontSize * 0.75f;

        bottomText.text = item.GetDescription();
        bottomText.fontSize = fontSize * 0.75f;
    }

    public void SetAnchor(TextAnchor textAnchor)
    {
        switch (textAnchor)
        {
            case TextAnchor.MiddleCenter:
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);

                break;
            case TextAnchor.UpperCenter:
                rect.anchorMin = new Vector2(0.5f, 1);
                rect.anchorMax = new Vector2(0.5f, 1);
                rect.pivot = new Vector2(0.5f, 1);

                break;
            case TextAnchor.LowerCenter:
                rect.anchorMin = new Vector2(0.5f, 0);
                rect.anchorMax = new Vector2(0.5f, 0);
                rect.pivot = new Vector2(0.5f, 0);

                break;
            case TextAnchor.MiddleRight:
                rect.anchorMin = new Vector2(1, 0.5f);
                rect.anchorMax = new Vector2(1, 0.5f);
                rect.pivot = new Vector2(1, 0.5f);

                break;
            case TextAnchor.MiddleLeft:
                rect.anchorMin = new Vector2(0, 0.5f);
                rect.anchorMax = new Vector2(0, 0.5f);
                rect.pivot = new Vector2(0, 0.5f);

                break;
            default:
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);

                break;
        }
    }

    private string ConvertRarityToString(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                middleText.color = Color.gray;

                return "common";
            case Rarity.Rare:
                middleText.color = Color.blue;

                return "rare";
            case Rarity.Epic:
                middleText.color = Color.pink;

                return "epic";
            case Rarity.Legendary:
                middleText.color = Color.orange;

                return "legendary";
            default:
                Debug.LogWarning("Rarity not specified.");

                return "";
        }
    }
}
