using TMPro;
using UnityEngine;

public class PopupPanelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI primaryText;

    private InformationPopupPanelController panelController;

    private float lifetime = 5;

    private void Update()
    {
        lifetime -= Time.deltaTime;

        if (lifetime <= 0)
        {
            panelController.RemovePopup(gameObject);
            Destroy(gameObject);
        }
    }

    public void Init(InformationPopupPanelController panelController, string text)
    {
        this.panelController = panelController;
        primaryText.text = text;
    }
}
