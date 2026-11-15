using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIHelper : MonoBehaviour
{
    public static UIHelper Instance;

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private InformationPopupPanelController informationPopupPanelController;
    [SerializeField]
    private SlideTransition fridgeSlideTransition;
    [SerializeField]
    private SlideTransition marketSlideTransition;
    [SerializeField]
    private GameObject infoPanelLayoutPrefab;
    [SerializeField]
    private Transform uiPanel;

    private List<GameObject> infoPanelList = new List<GameObject>();

    private bool fridgePanelActive = false;
    private bool marketPanelActive = false;

    public bool MarketPanelActive
    {
        get
        {
            return marketPanelActive;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public Vector2 GetMousePos()
    {
        return cam.ScreenToWorldPoint(Mouse.current.position.value);
    }

    public void ShowPopup(string text)
    {
        informationPopupPanelController.AddPopup(text);
    }

    public void SetFridgePanel(bool active)
    {
        if (active)
        {
            fridgeSlideTransition.StartTransition(SlideTransition.TransitionDirection.startToEnd);
        }
        else
        {
            fridgeSlideTransition.StartTransition(SlideTransition.TransitionDirection.endToStart);
        }

        fridgePanelActive = active;
    }

    public void ToggleFridgePanel()
    {
        if (!fridgePanelActive)
        {
            SetFridgePanel(true);
        }
        else
        {
            AudioManager.Instance.Play(3);

            SetFridgePanel(false);
        }
    }

    public void SetMarketPanel(bool active)
    {
        if (active)
        {
            marketSlideTransition.StartTransition(SlideTransition.TransitionDirection.startToEnd);
        }
        else
        {
            marketSlideTransition.StartTransition(SlideTransition.TransitionDirection.endToStart);
        }

        marketPanelActive = active;
    }

    public void ClearInfoPanels()
    {
        for (int i = infoPanelList.Count - 1; i >= 0; i--)
        {
            Destroy(infoPanelList[i]);
            infoPanelList.RemoveAt(i);
        }
    }

    public void InstantiateInfoPanel(Transform parent, Vector2 position, Vector2 displacement, TextAnchor anchor, Entity entity)
    {
        ClearInfoPanels();

        GameObject infoPanelLayoutTemp = Instantiate(infoPanelLayoutPrefab, parent);
        infoPanelLayoutTemp.transform.SetParent(parent);

        InfoPanelLayoutController infoPanelLayoutController = infoPanelLayoutTemp.GetComponent<InfoPanelLayoutController>();
        infoPanelLayoutController.Instantiate(position + displacement, anchor);

        infoPanelLayoutTemp.transform.SetParent(uiPanel, true);
        infoPanelLayoutTemp.transform.localScale = Vector3.one;

        infoPanelLayoutController.AddInfoPanel(entity, 22);
        infoPanelLayoutController.AddSubInfoPanels(entity.EntitySkills, 20);
       
        infoPanelList.Add(infoPanelLayoutTemp);
    }

    public void InstantiateInfoPanel(Transform parent, Vector2 position, Vector2 displacement, TextAnchor anchor, Item item)
    {
        ClearInfoPanels();

        GameObject infoPanelLayoutTemp = Instantiate(infoPanelLayoutPrefab, parent);
        infoPanelLayoutTemp.transform.SetParent(parent);

        InfoPanelLayoutController infoPanelLayoutController = infoPanelLayoutTemp.GetComponent<InfoPanelLayoutController>();
        infoPanelLayoutController.Instantiate(position + displacement, anchor);

        infoPanelLayoutTemp.transform.SetParent(uiPanel, true);
        infoPanelLayoutTemp.transform.localScale = Vector3.one;

        infoPanelLayoutController.AddInfoPanel(item, 22);

        infoPanelList.Add(infoPanelLayoutTemp);
    }
}
