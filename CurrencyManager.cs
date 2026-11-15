using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    private int magnets;

    public int Magnets
    {
        get
        {
            return magnets;
        }
        set
        {
            if (value < 0)
            {
                magnets = 0;
            }
            else
            {
                magnets = value;
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }
}
