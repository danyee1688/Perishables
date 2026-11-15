using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.Play(7);
    }

    public void PlaySelect()
    {
        AudioManager.Instance.Play(2);
    }

}
