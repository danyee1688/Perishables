using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationPopupPanelController : MonoBehaviour
{
    [SerializeField]
    private RectTransform layout;
    [SerializeField]
    private GameObject informationPopupPrefab;

    private List<GameObject> popups = new List<GameObject>(); 

    public void AddPopup(string text)
    {
        GameObject popupGO = Instantiate(informationPopupPrefab, layout);
        PopupPanelController popupController = popupGO.GetComponent<PopupPanelController>();

        popupController.Init(this, text);
        popups.Add(popupGO);

        LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
    }

    public void RemovePopup(GameObject gameObject)
    {
        popups.Remove(gameObject);
    }
}
