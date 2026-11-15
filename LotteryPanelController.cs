using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LotteryPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject wheel;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Image winningImage;
    [SerializeField]
    private Image losingImage;
    [SerializeField]
    private Button spinButton;
    [SerializeField]
    private Button continueButton;

    private bool wheelSpun = false;
    private List<Option> options = new List<Option>();
    private Option winningOption;
    private Option losingOption;

    private enum Option
    {
        Apple,
        Pear,
        Orange,
        Cabbage
    }

    private int appleMin = 0;
    private int appleMax = 89;
    private int orangeMin = -1;
    private int orangeMax = -89;
    private int cabbageMin = -90;
    private int cabbageMax = -179;
    private int pearMin = 90; 
    private int pearMax = 179; 

    private void OnEnable()
    {
        Reset();

        StartCoroutine(LotteryCoroutine());
    }

    private void Update()
    {
        if (wheelSpun)
        {
            if (Mathf.Approximately(wheel.transform.rotation.z * Mathf.Rad2Deg, 0))
            {
                AudioManager.Instance.Play(10);
            }
        }
    }

    private void Reset()
    {
        wheel.transform.rotation = Quaternion.identity;
        wheelSpun = false;
        spinButton.enabled = true;
    }

    private void GenerateStanding()
    {
        options.Add(Option.Apple);
        options.Add(Option.Pear);
        options.Add(Option.Orange);
        options.Add(Option.Cabbage);

        int randomIndex = Random.Range(0, options.Count);

        winningOption = options[randomIndex];
        winningImage.sprite = GetOptionSprite(winningOption);

        options.Remove(winningOption);

        randomIndex = Random.Range(0, options.Count);

        losingOption = options[randomIndex];
        losingImage.sprite = GetOptionSprite(losingOption);
    }

    public IEnumerator LotteryCoroutine()
    {
        GenerateStanding();

        yield return new WaitUntil(() => wheelSpun);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Mathf.Approximately(rb.angularVelocity, 0));

        Debug.Log("Wheel stopped spinning");

        wheelSpun = false;

        CalculateResult();
    }

    public void CalculateResult()
    {
        float rot = wheel.transform.rotation.z * Mathf.Rad2Deg;

        Option option;

        if (rot >= appleMin && rot <= appleMax)
        {
            option = Option.Apple;
        }
        else if (rot >= pearMin && rot <= pearMax)
        {
            option = Option.Pear;
        }
        else if (rot >= orangeMin && rot <= orangeMax)
        {
            option = Option.Orange;
        }
        else
        {
            option = Option.Cabbage;
        }

        Debug.Log(option);

        if (option == winningOption)
        {
            AudioManager.Instance.Play(9);

            Victory();
        }
        else if (option == losingOption)
        {
            AudioManager.Instance.Play(6);

            Loss();
        }

        SetSpinButton(false);
        SetContinueButton(true);
    }

    public void SetContinueButton(bool active)
    {
        continueButton.gameObject.SetActive(active);
    }

    public void SetSpinButton(bool active)
    {
        spinButton.gameObject.SetActive(active);
    }

    public void Victory()
    {
        CurrencyManager.Instance.Magnets += 5;
        GameManager.Instance.UpdateCurrencyText();

        Instantiate(InstantiableLibrary.Instance.VictoryConfettiPE, wheel.transform.position, Quaternion.identity);
    }

    public void Loss()
    {
        CurrencyManager.Instance.Magnets -= 5;
        GameManager.Instance.UpdateCurrencyText();
    }

    public void SpinWheel()
    {
        spinButton.enabled = false;
        wheelSpun = true;

        float randomTorque = Random.Range(1000, 2000);
        rb.AddTorque(randomTorque);
    }

    private Sprite GetOptionSprite(Option option)
    {
        switch (option)
        {
            case Option.Apple:
                return SpriteDB.Instance.entitySprites[0];
            case Option.Pear:
                return SpriteDB.Instance.entitySprites[2];
            case Option.Orange:
                return SpriteDB.Instance.entitySprites[1];
            case Option.Cabbage:
                return SpriteDB.Instance.entitySprites[3];
            default:
                return null;
        }
    }
}
