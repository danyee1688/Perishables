using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class InfoPanelLayoutController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject infoPanelGO;

    [Header("Panels/Layout")]
    [SerializeField]
    private VerticalLayoutGroup verticalLayoutGroup;
    [SerializeField]
    private VerticalLayoutGroup subVerticalLayoutGroup;

    [Header("Rects")]
    [SerializeField]
    private RectTransform rect;
    [SerializeField]
    private RectTransform subRect;

    private List<GameObject> infoPanelList = new List<GameObject>();
    private List<GameObject> subInfoPanelList = new List<GameObject>();

    private bool anchorSet = false;
    private bool subRectSet = false;
    private string subInfoPosition = "Right";

    public void Instantiate(Vector2 position, TextAnchor anchor)
    {
        ClearInfoPanels();

        StopAllCoroutines();

        rect.anchoredPosition = position;

        StartCoroutine(SetAnchor(anchor));
    }

    public IEnumerator SetAnchor(TextAnchor textAnchor)
    {
        anchorSet = false;

        switch (textAnchor)
        {
            case TextAnchor.MiddleCenter:
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                verticalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;

                break;
            case TextAnchor.UpperCenter:
                rect.anchorMin = new Vector2(0.5f, 1);
                rect.anchorMax = new Vector2(0.5f, 1);
                rect.pivot = new Vector2(0.5f, 1);
                verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;

                if (rect.position.x > 0)
                {
                    subRect.anchorMin = new Vector2(1, 0);
                    subRect.anchorMax = new Vector2(1, 0);
                    subRect.pivot = new Vector2(1, 0);
                    subVerticalLayoutGroup.childAlignment = TextAnchor.UpperRight;

                    subInfoPosition = "Left";
                }
                else
                {
                    subRect.anchorMin = new Vector2(0, 0);
                    subRect.anchorMax = new Vector2(0, 0);
                    subRect.pivot = new Vector2(0, 0);
                    subVerticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;

                    subInfoPosition = "Right";
                }

                break;
            case TextAnchor.LowerCenter:
                rect.anchorMin = new Vector2(0.5f, 0);
                rect.anchorMax = new Vector2(0.5f, 0);
                rect.pivot = new Vector2(0.5f, 0);
                verticalLayoutGroup.childAlignment = TextAnchor.LowerCenter;

                if (rect.position.x > 0)
                {
                    subRect.anchorMin = new Vector2(1, 1);
                    subRect.anchorMax = new Vector2(1, 1);
                    subRect.pivot = new Vector2(1, 1);
                    subVerticalLayoutGroup.childAlignment = TextAnchor.LowerRight;

                    subInfoPosition = "Left";
                }
                else
                {
                    subRect.anchorMin = new Vector2(0, 1);
                    subRect.anchorMax = new Vector2(0, 1);
                    subRect.pivot = new Vector2(0, 1);
                    subVerticalLayoutGroup.childAlignment = TextAnchor.LowerLeft;

                    subInfoPosition = "Right";
                }

                break;
            case TextAnchor.MiddleRight:
                rect.anchorMin = new Vector2(1, 0.5f);
                rect.anchorMax = new Vector2(1, 0.5f);
                rect.pivot = new Vector2(1, 0.5f);
                verticalLayoutGroup.childAlignment = TextAnchor.MiddleRight;

                if (rect.position.x > 0)
                {
                    subRect.anchorMin = new Vector2(1, 1);
                    subRect.anchorMax = new Vector2(1, 1);
                    subRect.pivot = new Vector2(1, 1);
                    subVerticalLayoutGroup.childAlignment = TextAnchor.MiddleRight;

                    subInfoPosition = "Left";
                }
                else
                {
                    subRect.anchorMin = new Vector2(0, 1);
                    subRect.anchorMax = new Vector2(0, 1);
                    subRect.pivot = new Vector2(0, 1);
                    subVerticalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;

                    subInfoPosition = "Right";
                }

                break;
            case TextAnchor.MiddleLeft:
                rect.anchorMin = new Vector2(0, 0.5f);
                rect.anchorMax = new Vector2(0, 0.5f);
                rect.pivot = new Vector2(0, 0.5f);
                verticalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;

                if (rect.position.x > 0)
                {
                    subRect.anchorMin = new Vector2(1, 1);
                    subRect.anchorMax = new Vector2(1, 1);
                    subRect.pivot = new Vector2(1, 1);
                    subVerticalLayoutGroup.childAlignment = TextAnchor.MiddleRight;

                    subInfoPosition = "Left";
                }
                else
                {
                    subRect.anchorMin = new Vector2(0, 1);
                    subRect.anchorMax = new Vector2(0, 1);
                    subRect.pivot = new Vector2(0, 1);
                    subVerticalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;

                    subInfoPosition = "Right";
                }

                break;
            default:
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                verticalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;

                break;
        }

        yield return null;

        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(subRect);

        yield return new WaitForSeconds(0.1f);

        anchorSet = true;

        StartCoroutine(ShowInfoPanels());
    }

    // Info panel for entity
    public void AddInfoPanel(Entity entity, float fontSize)
    {
        GameObject infoPanelGOTemp = Instantiate(infoPanelGO);
        infoPanelGOTemp.transform.SetParent(rect.transform);
        infoPanelGOTemp.transform.localScale = Vector3.one;

        InfoPanelController infoPanelController = infoPanelGOTemp.GetComponent<InfoPanelController>();
        infoPanelController.Instantiate(entity, fontSize);

        infoPanelList.Add(infoPanelGOTemp);
    }

    // Info panel for skills
    public void AddInfoPanel(string title, int skillDamage, float skillCd, string descriptionText, float fontSize)
    {
        GameObject infoPanelGOTemp = Instantiate(infoPanelGO);
        infoPanelGOTemp.transform.SetParent(rect.transform);
        infoPanelGOTemp.transform.localScale = Vector3.one;

        InfoPanelController infoPanelController = infoPanelGOTemp.GetComponent<InfoPanelController>();
        infoPanelController.Instantiate(title, skillDamage, skillCd, descriptionText, fontSize);

        infoPanelList.Add(infoPanelGOTemp);
    }

    // Info panel for entity
    public void AddInfoPanel(Item item, float fontSize)
    {
        GameObject infoPanelGOTemp = Instantiate(infoPanelGO);
        infoPanelGOTemp.transform.SetParent(rect.transform);
        infoPanelGOTemp.transform.localScale = Vector3.one;

        InfoPanelController infoPanelController = infoPanelGOTemp.GetComponent<InfoPanelController>();
        infoPanelController.Instantiate(item, fontSize);

        infoPanelList.Add(infoPanelGOTemp);
    }

    public void AddSubInfoPanels(List<Skill> skills, float fontSize)
    {
        StartCoroutine(InstantiateSubInfoPanels(skills, fontSize));
    }

    public void ClearInfoPanels()
    {
        infoPanelList.Clear();
        subInfoPanelList.Clear();
    }

    public IEnumerator ShowInfoPanels()
    {
        foreach (GameObject infoPanelTemp in infoPanelList)
        {
            infoPanelTemp.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

        yield return new WaitUntil(() => subRectSet);

        ShowSubInfoPanels();
    }

    public IEnumerator InstantiateSubInfoPanels(List<Skill> skills, float fontSize)
    {
        subRectSet = false;

        yield return new WaitUntil(() => anchorSet);

        subRect.anchoredPosition = Vector2.zero;

        yield return new WaitForEndOfFrame();

        if (subInfoPosition.Equals("Right"))
        {
            subRect.anchoredPosition += new Vector2(rect.sizeDelta.x, 0);
        }
        else
        {
            subRect.anchoredPosition -= new Vector2(rect.sizeDelta.x, 0);
        }

        subRect.gameObject.SetActive(true);

        foreach (Skill skill in skills)
        {
            GameObject infoPanelGOTemp = Instantiate(infoPanelGO);
            infoPanelGOTemp.transform.SetParent(subRect.transform);
            infoPanelGOTemp.transform.localScale = Vector3.one;

            InfoPanelController infoPanelController = infoPanelGOTemp.GetComponent<InfoPanelController>();
            infoPanelController.Instantiate(skill.SkillName, skill.SkillDamage, skill.SkillCd, skill.GetDescription(), fontSize);

            subInfoPanelList.Add(infoPanelGOTemp);
        }

        yield return new WaitForEndOfFrame();

        subRectSet = true;
    }

    public void ShowSubInfoPanels()
    {
        foreach (GameObject infoPanelTemp in subInfoPanelList)
        {
            infoPanelTemp.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate(subRect);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(subRect);
    }
}
