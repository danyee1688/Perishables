using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField]
    private Slider healthBarSlider;
    [SerializeField]
    private Image sliderFill;
    [SerializeField]
    private TextMeshProUGUI entityName;
    [SerializeField]
    private Image entityImage;
    [SerializeField]
    private GameObject statusIconPrefab;
    [SerializeField]
    private Transform statusLayout;
    [SerializeField]
    private TextMeshProUGUI amountText;

    private Entity parentEntity;

    private Dictionary<int, StatusIconController> statusIconDict = new Dictionary<int, StatusIconController>();

    public void Init(Entity entity, bool isFriendly)
    {
        parentEntity = entity;

        entityName.text = parentEntity.EntityName;
        entityImage.sprite = parentEntity.GetSprite();

        if (isFriendly)
        {
            sliderFill.color = Color.green;
        }
        else
        {
            sliderFill.color = Color.red;
        }

        SetValue(entity.EntityBaseHealth, entity.EntityBaseHealth);
    }

    public void SetValue(float value)
    {
        healthBarSlider.value = value;
    }

    public void SetValue(float current, float max)
    {
        float percentage = current / max;

        healthBarSlider.value = percentage;

        amountText.text = $"{current.ToString()}/{max.ToString()}";
    }

    public void AddStatus(Status status)
    {
        GameObject statusGO = Instantiate(statusIconPrefab, statusLayout);
        StatusIconController statusIconController = statusGO.GetComponent<StatusIconController>();

        statusIconController.Init(status);

        statusIconDict.Add(status.StatusId, statusIconController);
    }

    public void UpdateStatus(Status status)
    {
        StatusIconController statusIconController = statusIconDict[status.StatusId];
        statusIconController.UpdateStatus(status);
    }

    public void RemoveStatus(Status status)
    {
        StatusIconController statusIconController = statusIconDict[status.StatusId];
        Destroy(statusIconController.gameObject);

        statusIconDict.Remove(status.StatusId);
    }
}
