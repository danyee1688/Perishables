using UnityEngine;

public class InstantiableLibrary : MonoBehaviour
{
    public static InstantiableLibrary Instance;

    private void Awake()
    {
        Instance = this;
    }

    [Header("Bullets and AoEs")]
    [SerializeField]
    private GameObject smallBulletPrefab;
    [SerializeField]
    private GameObject aoe1Prefab;
    [SerializeField]
    private GameObject thornyRockPrefab;

    [Header("ParticleSystems")]
    [SerializeField]
    private GameObject collisionPE;
    [SerializeField]
    private GameObject largeCollisionPE;
    [SerializeField]
    private GameObject collectItemPE;
    [SerializeField]
    private GameObject victoryConfettiPE;

    [Header("Bosses")]
    [SerializeField]
    private GameObject watermelonBoss;

    [Header("Misc")]
    [SerializeField]
    private GameObject lootPickupPrefab;

    public GameObject SmallBulletPrefab
    {
        get
        {
            return smallBulletPrefab;
        }
    }

    public GameObject Aoe1Prefab
    {
        get
        {
            return aoe1Prefab;
        }
    }

    public GameObject LootPickupPrefab
    {
        get
        {
            return lootPickupPrefab;
        }
    }

    public GameObject CollisionPE
    {
        get
        {
            return collisionPE;
        }
    }

    public GameObject LargeCollisionPE
    {
        get 
        {
            return largeCollisionPE;
        }
    }

    public GameObject CollectItemPE
    {
        get
        {
            return collectItemPE;
        }
    }

    public GameObject VictoryConfettiPE
    {
        get
        {
            return victoryConfettiPE;
        }
    }

    public GameObject ThornyRockPrefab
    {
        get
        {
            return thornyRockPrefab;
        }
    }

    public GameObject WatermelonBoss
    {
        get
        {
            return watermelonBoss;
        }
    }
}
