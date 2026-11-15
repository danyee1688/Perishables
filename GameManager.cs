using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject entityPrefab;
    [SerializeField]
    private GameObject encounterButtonPrefab;
    [SerializeField]
    private GameObject healthBarPrefab;

    [Header("Layouts/Panels")]
    [SerializeField]
    private Transform encounterButtonLayout;
    [SerializeField]
    private Transform lootPanel;
    [SerializeField]
    private Transform gamePanel;
    [SerializeField]
    private Transform gameOverPanel;
    [SerializeField]
    private Transform friendlyHealthBarLayout;
    [SerializeField]
    private Transform enemyHealthBarLayout;
    [SerializeField]
    private Transform lotteryPanel;
    [SerializeField]
    private Transform pausePanel;

    [Header("Misc")]
    [SerializeField]
    private FridgeController fridgeController;
    [SerializeField]
    private ShopController shopController;

    private List<GameObject> friendlyEntities = new List<GameObject>();
    private List<GameObject> enemyEntities = new List<GameObject>();
    private List<GameObject> activeFriendlyEntities = new List<GameObject>();
    private List<GameObject> activeEnemyEntities = new List<GameObject>();
    private List<GameObject> entityStructures = new List<GameObject>();
    private List<GameObject> encounterButtons = new List<GameObject>();
    private List<GameObject> healthBars = new List<GameObject>();

    private Dictionary<GameObject, GameObject> healthBarDict = new Dictionary<GameObject, GameObject>();

    private int encounterCount = 0;
    private EncounterType currentEncounter = EncounterType.None;
    [SerializeField]
    private bool encounterFinished = false;

    public List<GameObject> FriendlyEntities
    {
        get
        {
            return friendlyEntities;
        }
    }

    public List<GameObject> EnemyEntities
    {
        get
        {
            return enemyEntities;
        }
    }

    public List<GameObject> ActiveFriendlyEntities
    {
        get
        {
            return activeFriendlyEntities;
        }
    }

    public List<GameObject> ActiveEnemyEntities
    {
        get
        {
            return activeEnemyEntities;
        }
    }

    public List<GameObject> EntityStructures
    {
        get
        {
            return entityStructures;
        }
        set
        {
            entityStructures = value;
        }
    }

    public Transform GamePanel
    {
        get
        {
            return gamePanel;
        }
    }

    public FridgeController FridgeController
    {
        get
        {
            return fridgeController;
        }
    }

    public ShopController ShopController
    {
        get
        {
            return shopController;
        }
    }

    public bool EncounterFinished
    {
        get
        {
            return encounterFinished;
        }
    }

    private const float MAX_Y = 0.5f;
    private const float MAX_X = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetupInitialFridge();

        StartCoroutine(NextEncounterCoroutine());
    }

    private void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            UIHelper.Instance.ToggleFridgePanel();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePausePanel();
        }
    }

    private IEnumerator NextEncounterCoroutine()
    {
        encounterCount++;
        currentEncounter = EncounterType.None;

        ShowNextEncounters();

        yield return new WaitUntil(() => currentEncounter != EncounterType.None);

        Debug.Log("Setting up next encounter");
        encounterFinished = false;

        SetupCurrentEncounter();

        yield return new WaitForSeconds(0.5f);

        if (currentEncounter == EncounterType.NormalFight || currentEncounter == EncounterType.HardFight || currentEncounter == EncounterType.BossFight)
        {
            yield return new WaitUntil(() => activeFriendlyEntities.Count == 0 || activeEnemyEntities.Count == 0);

            if (activeFriendlyEntities.Count == 0)
            {
                Debug.Log("Game Over!");

                SetGameOverPanel(true);
            }
        }
        else if (currentEncounter == EncounterType.GroceryStore)
        {
            yield return new WaitUntil(() => !UIHelper.Instance.MarketPanelActive);
        }
        else if (currentEncounter == EncounterType.Lottery)
        {
            yield return new WaitUntil(() => !lotteryPanel.gameObject.activeInHierarchy);
        }

        encounterFinished = true;

        if (!gameOverPanel.gameObject.activeInHierarchy)
        {
            if (currentEncounter == EncounterType.NormalFight || currentEncounter == EncounterType.HardFight || currentEncounter == EncounterType.BossFight)
            {
                fridgeController.UpdateFridge();
            }

            ClearHealthBars();

            StartCoroutine(NextEncounterCoroutine());
        }
    }

    private void SetupInitialFridge()
    {
        fridgeController.AddToFridge(new Entity(EntityDB.entities[0]));
        fridgeController.AddToFridge(new Entity(EntityDB.entities[0]));
        fridgeController.AddToFridge(new Entity(EntityDB.entities[0]));
    }

    public void SpawnEntity(Entity baseEntity, bool isFriendly)
    {
        if (baseEntity.EntityRarity == Rarity.Legendary)
        {
            GameObject entityGO = null;

            switch (baseEntity.EntityId)
            {
                case 7:
                    entityGO = Instantiate(InstantiableLibrary.Instance.WatermelonBoss);

                    break;
            }

            EntityController entityController = entityGO.GetComponent<EntityController>();

            Entity entity = ModifyEntity(baseEntity);

            entityController.Init(entity, isFriendly);

            if (!isFriendly)
            {
                enemyEntities.Add(entityGO);
                activeEnemyEntities.Add(entityGO);
            }
            else
            {
                friendlyEntities.Add(entityGO);
                activeFriendlyEntities.Add(entityGO);
            }

            SpawnHealthBar(entity, entityGO, isFriendly);
        }
        else
        {
            GameObject entityGO = Instantiate(entityPrefab, GetRandomCoordinate(), Quaternion.identity);
            EntityController entityController = entityGO.GetComponent<EntityController>();

            Entity entity = ModifyEntity(baseEntity);

            entityController.Init(entity, isFriendly);

            if (!isFriendly)
            {
                enemyEntities.Add(entityGO);
                activeEnemyEntities.Add(entityGO);
            }
            else
            {
                friendlyEntities.Add(entityGO);
                activeFriendlyEntities.Add(entityGO);
            }

            SpawnHealthBar(entity, entityGO, isFriendly);
        }
    }

    public Entity ModifyEntity(Entity baseEntity)
    {
        Entity entity = new Entity(baseEntity);

        if (baseEntity.EquippedItem != null)
        {
            switch (baseEntity.EquippedItem.ItemID)
            {
                case 0:
                    entity.EntityBaseHealth += 3;
                    break;
                case 1:
                    entity.Strength += 1;
                    break;
                case 2:
                    entity.AdditionalSkillCd += -0.1f;
                    break;
            }
        }

        return entity;
    }

    public void ClearHealthBars()
    {
        for (int i = healthBars.Count - 1; i >= 0; i--)
        {
            Destroy(healthBars[i]);
            healthBars.RemoveAt(i);
        }

        for (int i = healthBarDict.Keys.Count - 1;i >= 0;i--)
        {
            healthBarDict.Remove(new List<GameObject>(healthBarDict.Keys)[i]);
        }
    }
    
    public void SpawnHealthBar(Entity entity, GameObject entityGO, bool isFriendly)
    {
        GameObject healthBarGO = null;
        HealthBarController healthBarController = null;

        if (isFriendly)
        {
            healthBarGO = Instantiate(healthBarPrefab, friendlyHealthBarLayout);
        }
        else
        {
            healthBarGO = Instantiate(healthBarPrefab, enemyHealthBarLayout);
        }

        healthBarController = healthBarGO.GetComponent<HealthBarController>();
        healthBarController.Init(entity, isFriendly);

        healthBars.Add(healthBarGO);
        healthBarDict.Add(entityGO, healthBarGO);
        
        EntityController entityController = entityGO.GetComponent<EntityController>();  
        entityController.SetHealthBar(healthBarController);
    }

    public void UpdateHealthBar(EntityController entityController, Entity entity)
    {
        if (entityController.gameObject != null && healthBarDict.ContainsKey(entityController.gameObject))
        {
            HealthBarController healthBarController = healthBarDict[entityController.gameObject].GetComponent<HealthBarController>();

            healthBarController.SetValue(entityController.Health, entity.EntityBaseHealth);
        }
    }

    public Vector2 GetRandomCoordinate()
    {
        return new Vector2(Random.Range(-MAX_X, MAX_X), Random.Range(-MAX_Y, MAX_Y));
    }

    public void ClearEncounterButtons()
    {
        for (int i = encounterButtons.Count - 1; i >= 0; i--)
        {
            Destroy(encounterButtons[i]);
            encounterButtons.RemoveAt(i);
        }
    }

    public void ShowNextEncounters()
    {
        ClearEncounterButtons();

        if (encounterCount == 1)
        {
            GameObject encounterButtonGo = Instantiate(encounterButtonPrefab, encounterButtonLayout);
            EncounterButtonController encounterButtonController = encounterButtonGo.GetComponent<EncounterButtonController>();

            encounterButtonController.Init(EncounterType.NormalFight);

            encounterButtons.Add(encounterButtonGo);
        }
        else if (encounterCount % 5 == 0)
        {
            GameObject encounterButtonGo = Instantiate(encounterButtonPrefab, encounterButtonLayout);
            EncounterButtonController encounterButtonController = encounterButtonGo.GetComponent<EncounterButtonController>();

            encounterButtonController.Init(EncounterType.BossFight);

            encounterButtons.Add(encounterButtonGo);
        }
        else
        {
            EncounterType randEncounterType = EncounterDB.GetRandomEncounterWeighted();

            GameObject encounterButtonGo = Instantiate(encounterButtonPrefab, encounterButtonLayout);
            EncounterButtonController encounterButtonController = encounterButtonGo.GetComponent<EncounterButtonController>();

            encounterButtonController.Init(randEncounterType);

            encounterButtons.Add(encounterButtonGo);

            if (Chance(20))
            {
                randEncounterType = EncounterDB.GetRandomEncounterWeighted();

                encounterButtonGo = Instantiate(encounterButtonPrefab, encounterButtonLayout);
                encounterButtonController = encounterButtonGo.GetComponent<EncounterButtonController>();

                encounterButtonController.Init(randEncounterType);

                encounterButtons.Add(encounterButtonGo);
            }

            encounterButtonGo = Instantiate(encounterButtonPrefab, encounterButtonLayout);
            encounterButtonController = encounterButtonGo.GetComponent<EncounterButtonController>();

            encounterButtonController.Init(EncounterType.GroceryStore);

            encounterButtons.Add(encounterButtonGo);
        }
    }

    public void ClearAllEntities()
    {
        for (int i = FriendlyEntities.Count - 1; i >= 0; i--)
        {
            Destroy(FriendlyEntities[i]);
            FriendlyEntities.RemoveAt(i);
        }

        for (int i = EnemyEntities.Count - 1; i >= 0; i--)
        {
            Destroy(EnemyEntities[i]);
            EnemyEntities.RemoveAt(i);
        }

        for (int i = activeFriendlyEntities.Count - 1; i >= 0; i--)
        {
            Destroy(activeFriendlyEntities[i]);
            activeFriendlyEntities.RemoveAt(i);
        }

        for (int i = activeEnemyEntities.Count - 1; i >= 0; i--)
        {
            Destroy(activeEnemyEntities[i]);
            activeEnemyEntities.RemoveAt(i);
        }

        for (int i = entityStructures.Count - 1; i >= 0; i--)
        {
            Destroy(entityStructures[i]);
            entityStructures.RemoveAt(i);
        }
    }

    private void SetupCurrentEncounter()
    {
        ClearAllEntities();

        if (currentEncounter == EncounterType.NormalFight)
        {
            int enemyCount = Random.Range(2, 3);

            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEntity(EntityDB.GetRandomEntityByRarity(Rarity.Common), false);
            }

            foreach (Entity entity in fridgeController.Entities)
            {
                SpawnEntity(entity, true);
            }
        }
        else if (currentEncounter == EncounterType.HardFight)
        {
            int enemyCount = Random.Range(3, 5);

            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEntity(EntityDB.GetRandomEntityWeighted(), false);
            }

            foreach (Entity entity in fridgeController.Entities)
            {
                SpawnEntity(entity, true);
            }
        }
        else if (currentEncounter == EncounterType.BossFight)
        {
            SpawnEntity(EntityDB.GetRandomEntityByRarity(Rarity.Legendary), false);

            foreach (Entity entity in fridgeController.Entities)
            {
                SpawnEntity(entity, true);
            }
        }
        else if (currentEncounter == EncounterType.GroceryStore)
        {
            shopController.RefreshEntities(false);
            shopController.ResetEntityRefreshCost();
            shopController.RefreshItems(false);
            shopController.ResetItemRefreshCost();

            UIHelper.Instance.SetMarketPanel(true);
            UIHelper.Instance.SetFridgePanel(true);
        }
        else if (currentEncounter == EncounterType.Lottery)
        {
            SetLotteryPanel(true);
        }
        else if (currentEncounter == EncounterType.Random)
        {
            currentEncounter = EncounterDB.GetRandomEncounterUnweighted();

            SetupCurrentEncounter();
        }

        ClearEncounterButtons();
    }

    public IEnumerator DramaticPauseCoroutine(float duration)
    {
        Time.timeScale = 0.01f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1;
    }

    public bool Chance(float percentage)
    {
        if (Random.Range(0, 100f) < percentage)
        {
            return true;
        }

        return false;
    }

    public void SetEncounterType(EncounterType encounterType)
    {
        currentEncounter = encounterType;
    }

    public void SetLootPanel(bool active)
    {
        lootPanel.gameObject.SetActive(active);
    }

    public void UpdateCurrencyText()
    {
        fridgeController.SetCurrencyText("x" + CurrencyManager.Instance.Magnets.ToString());
    }

    public void SetGameOverPanel(bool active)
    {
        gameOverPanel.gameObject.SetActive(active);
    }

    public void SetLotteryPanel(bool active)
    {
        lotteryPanel.gameObject.SetActive(active);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void SetPausePanel(bool active)
    {
        pausePanel.gameObject.SetActive(active);

        if (active)
        {
            Time.timeScale = 0.00001f;
        }
        else
        {
            AudioManager.Instance.Play(3);

            Time.timeScale = 1f;
        }
    }

    public void TogglePausePanel()
    {
        if (pausePanel.gameObject.activeInHierarchy)
        {
            AudioManager.Instance.Play(3);

            SetPausePanel(false);
        }
        else
        {
            SetPausePanel(true);
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

[System.Serializable]
public enum EncounterType
{
    None,
    NormalFight,
    HardFight,
    BossFight,
    GroceryStore,
    Lottery,
    Random,
}