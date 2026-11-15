using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusIconController : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI stacksText;
    [SerializeField]
    private SizeTransition sizeTransition;

    private Status parentStatus;

    public void Init(Status status)
    {
        parentStatus = status;
        stacksText.text = 1.ToString();
        image.sprite = status.GetSprite();
    }

    public void UpdateStatus(Status status)
    {
        stacksText.text = status.Stacks.ToString();

        sizeTransition.StartTransition(SizeTransition.TransitionDirection.startToEnd);
    }
}
