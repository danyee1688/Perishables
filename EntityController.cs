using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EntityController : MonoBehaviour
{
    [Header("Image")]
    [SerializeField]
    private SpriteRenderer mainSpriteRenderer;
    [SerializeField]
    private SpriteRenderer overlaySpriteRenderer;

    [Header("Transitions")]
    [SerializeField]
    private ColorTransition rottingColorTransition;
    [SerializeField]
    private ColorTransition hurtColorTransition;

    private bool isFriendly;
    private int health = 10;
    private Dictionary<int, Skill> skillDict = new Dictionary<int, Skill>();
    private Entity parentEntity;
    private Rigidbody2D rb;
    private Collider2D col;
    private bool rotted = false;
    private bool isMenuEntity = false;
    private List<Status> statuses = new List<Status>();
    private HealthBarController attachedHealthBar;
    
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            if (value > parentEntity.EntityBaseHealth)
            {
                health = parentEntity.EntityBaseHealth;
            }
            else
            {
                health = value;
            }

            if (value <= 0)
            {
                OnDeath();
            }
        }
    }

    public Entity ParentEntity
    {
        get
        {
            return parentEntity;
        }
    }

    public bool IsFriendly
    {
        get
        {
            return isFriendly;
        }
        set
        {
            isFriendly = value;

            if (isFriendly)
            {
                gameObject.layer = 6;
            }
            else
            {
                gameObject.layer = 7;
            }
        }
    }

    public bool Rotted
    {
        get
        {
            return rotted;
        }
        set
        {
            rotted = value;

            if (rotted)
            {
                rottingColorTransition.StartTransition(ColorTransition.TransitionDirection.startToEnd);

                gameObject.layer = 10;
            }
            else
            {
                rottingColorTransition.StartTransition(ColorTransition.TransitionDirection.endToStart);

                if (isFriendly)
                {
                    gameObject.layer = 6;
                }
                else
                {
                    gameObject.layer = 7;
                }
            }
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!isMenuEntity)
        {
            if (!GameManager.Instance.EncounterFinished)
            {
                UpdateStatuses();
            }
        }
    }

    public void Init(Entity entity, bool isFriendly)
    {
        parentEntity = entity;
        Health = parentEntity.EntityBaseHealth;
        IsFriendly = isFriendly;

        SetupSkills();
        StartUseLoop();

        mainSpriteRenderer.sprite = parentEntity.GetSprite();
        overlaySpriteRenderer.sprite = parentEntity.GetSprite();
    }

    public void Init(Entity entity)
    {
        parentEntity = entity;
        isMenuEntity = true;

        mainSpriteRenderer.sprite = parentEntity.GetSprite();
    }

    public void SetupSkills()
    {
        List<Skill> defaultSkillList = new List<Skill>(parentEntity.EntitySkills);

        for (int i = 0; i < defaultSkillList.Count; i++)
        {
            if (i <= parentEntity.EntitySkillSlots)
            {
                skillDict.Add(i, defaultSkillList[i]);
            }
        }
    }

    public void StartUseLoop()
    {
        for (int i = 0; i < parentEntity.EntitySkillSlots; i++)
        {
            if (skillDict.ContainsKey(i))
            {
                StartCoroutine(UseSkillCoroutine(i));
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!GameManager.Instance.EncounterFinished)
        {
            AudioManager.Instance.Play(0, -0.1f, 0.1f);

            Health -= damage;

            Debug.Log($"Took {damage} damage");

            StartCoroutine(HurtColorCoroutine());

            GameManager.Instance.UpdateHealthBar(this, parentEntity);

            if (HasSkill(5) && GameManager.Instance.Chance(30))
            {
                AddStatus(new Status(StatusDB.statusList[0]));
            }
        }
    }

    public IEnumerator HurtColorCoroutine()
    {
        hurtColorTransition.StartTransition(ColorTransition.TransitionDirection.startToEnd);

        yield return new WaitUntil(() => hurtColorTransition.transitionDone);

        hurtColorTransition.StartTransition(ColorTransition.TransitionDirection.endToStart);
    }

    public void OnDeath()
    {
        AudioManager.Instance.Play(1);

        Debug.Log($"{parentEntity.EntityName} has died");

        if (isFriendly)
        {
            //if (GameManager.Instance.ActiveFriendlyEntities.Count == 1)
            //{
            //    float pauseDuration = 0.25f;

            //    StartCoroutine(GameManager.Instance.DramaticPauseCoroutine(pauseDuration));

            //    yield return new WaitForSecondsRealtime(pauseDuration);
            //}

            UIHelper.Instance.ShowPopup($"{parentEntity.EntityName} has rotted away");

            GameManager.Instance.ActiveFriendlyEntities.Remove(gameObject);

            GameManager.Instance.FridgeController.RemoveFromFridge(parentEntity);
        }
        else
        {
            //if (GameManager.Instance.ActiveEnemyEntities.Count == 1)
            //{
            //    float pauseDuration = 0.25f;

            //    StartCoroutine(GameManager.Instance.DramaticPauseCoroutine(pauseDuration));

            //    yield return new WaitForSecondsRealtime(pauseDuration);
            //}

            DropLoot();

            GameManager.Instance.ActiveEnemyEntities.Remove(gameObject);
        }

        Rotted = true;
    }

    public void UseSkill(Skill baseSkill)
    {
        GameObject goTemp1 = null;
        GameObject goTemp2 = null;
        ProjectileController bulletControllerTemp = null;
        Vector2 vectorTemp;
        int intTemp;
        float floatTemp;

        Skill skill = ModifySkill(baseSkill);

        switch (skill.SkillId)
        {
            case 0:
                goTemp1 = FindTarget();

                if (goTemp1 != null)
                {
                    goTemp2 = Instantiate(InstantiableLibrary.Instance.SmallBulletPrefab, transform.position, Quaternion.identity);
                    bulletControllerTemp = goTemp2.GetComponent<ProjectileController>();

                    vectorTemp = (goTemp1.transform.position - transform.position).normalized;

                    bulletControllerTemp.Init(skill, IsFriendly, vectorTemp, 3, ProjectileBehavior.Normal);
                }

                break;
            case 1:
                goTemp1 = FindTarget();
                    
                if (goTemp1 != null)
                {
                    goTemp2 = Instantiate(InstantiableLibrary.Instance.SmallBulletPrefab, transform.position, Quaternion.identity);
                    bulletControllerTemp = goTemp2.GetComponent<ProjectileController>();

                    vectorTemp = Quaternion.Euler(0, 0, 30) * (goTemp1.transform.position - transform.position).normalized;
                    bulletControllerTemp.Init(skill, IsFriendly, vectorTemp, 3, ProjectileBehavior.Normal);

                    goTemp2 = Instantiate(InstantiableLibrary.Instance.SmallBulletPrefab, transform.position, Quaternion.identity);
                    bulletControllerTemp = goTemp2.GetComponent<ProjectileController>();

                    vectorTemp = Quaternion.Euler(0, 0, -30) * (goTemp1.transform.position - transform.position).normalized;
                    bulletControllerTemp.Init(skill, IsFriendly, vectorTemp, 3, ProjectileBehavior.Normal);

                    goTemp2 = Instantiate(InstantiableLibrary.Instance.SmallBulletPrefab, transform.position, Quaternion.identity);
                    bulletControllerTemp = goTemp2.GetComponent<ProjectileController>();

                    vectorTemp = (goTemp1.transform.position - transform.position).normalized;
                    bulletControllerTemp.Init(skill, IsFriendly, vectorTemp, 3, ProjectileBehavior.Normal);
                }

                break;
            case 2:
                goTemp2 = Instantiate(InstantiableLibrary.Instance.SmallBulletPrefab, transform.position, Quaternion.identity);
                bulletControllerTemp = goTemp2.GetComponent<ProjectileController>();

                bulletControllerTemp.Init(skill, IsFriendly, Quaternion.Euler(0, 0, 45) * transform.up, 10, ProjectileBehavior.Spiral);

                goTemp2 = Instantiate(InstantiableLibrary.Instance.SmallBulletPrefab, transform.position, Quaternion.identity);
                bulletControllerTemp = goTemp2.GetComponent<ProjectileController>();

                bulletControllerTemp.Init(skill, IsFriendly, Quaternion.Euler(0, 0, 225) * transform.up, 10, ProjectileBehavior.Spiral);

                break;
            case 6:
                goTemp1 = Instantiate(InstantiableLibrary.Instance.ThornyRockPrefab, transform.position, Quaternion.identity);
                ThornRockController thornyRockController = goTemp1.GetComponent<ThornRockController>();

                GameManager.Instance.EntityStructures.Add(goTemp1);

                thornyRockController.Init(skill, isFriendly);

                break;
            case 9:
                floatTemp = Random.Range(0, 360);
                vectorTemp = (Quaternion.Euler(0, 0, floatTemp) * Vector2.up).normalized;

                rb.AddForce(vectorTemp * 3, ForceMode2D.Impulse);

                break;
        }
    }

    public IEnumerator UseSkillCoroutine(int skillIndex)
    {
        Skill skill = skillDict[skillIndex];

        if (skill.SkillType == SkillType.Active)
        {
            yield return new WaitForSeconds(skill.SkillCd);

            if (!Rotted && !GameManager.Instance.EncounterFinished)
            {
                UseSkill(skill);

                StartCoroutine(UseSkillCoroutine(skillIndex));
            }
        }
        else
        {
            yield return null;
        }
    }

    public Skill ModifySkill(Skill baseSkill)
    {
        Skill skill = new Skill(baseSkill);

        if (HasStatus(StatusDB.statusList[0]))
        {
            Status strengthStatus = FindStatusById(0);

            skill.SkillDamage += strengthStatus.Stacks;
        }

        skill.SkillDamage += parentEntity.Strength;
        skill.SkillCd += parentEntity.AdditionalSkillCd;

        return skill;
    }

    public void SetHealthBar(HealthBarController healthBarController)
    {
        attachedHealthBar = healthBarController;
    }

    public GameObject FindTarget()
    {
        GameObject target = null;
        float closestDistance = Mathf.Infinity;

        if (isFriendly)
        {
            for (int i = GameManager.Instance.ActiveEnemyEntities.Count - 1; i >= 0; i--)
            {
                GameObject potentialTarget = GameManager.Instance.ActiveEnemyEntities[i];
                float distance = Vector2.Distance(transform.position, potentialTarget.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = potentialTarget;
                }
            }
        }
        else
        {
            for (int i = GameManager.Instance.ActiveFriendlyEntities.Count - 1; i >= 0; i--)
            {
                GameObject potentialTarget = GameManager.Instance.ActiveFriendlyEntities[i];
                float distance = Vector2.Distance(transform.position, potentialTarget.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = potentialTarget;
                }
            }
        }

        return target;
    }

    public void ApplyKnockback(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void DropLoot()
    {
        Debug.Log("Drop loot");

        int amount = 0;

        switch (parentEntity.EntityRarity)
        {
            case Rarity.Common:
                amount = Random.Range(1, 2);

                break;
            case Rarity.Rare:
                amount = Random.Range(2, 4);

                break;
            case Rarity.Epic:
                amount = Random.Range(3, 6);

                break;
            case Rarity.Legendary:
                amount = Random.Range(6, 10);

                break;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject lootPickup = Instantiate(InstantiableLibrary.Instance.LootPickupPrefab, transform.position, Quaternion.identity);
            LootPickupable lootPickupable = lootPickup.GetComponent<LootPickupable>();

            lootPickupable.Init(LootType.Currency);
            lootPickup.transform.SetParent(GameManager.Instance.GamePanel, true);
            lootPickup.transform.localScale = Vector3.one;
        }
    }

    public bool HasSkill(int skillId)
    {
        foreach (Skill skill in skillDict.Values)
        {
            if (skill.SkillId == skillId)
            {
                return true;
            }
        }

        return false;
    }

    public Skill GetSkillById(int skillId)
    {
        foreach (Skill skill in skillDict.Values)
        {
            if (skill.SkillId == skillId)
            {
                return skill;
            }
        }

        return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isMenuEntity && !Rotted && !GameManager.Instance.EncounterFinished)
        {
            if (HasSkill(3) && GameManager.Instance.Chance(10))
            {
                Health += 1;
            }

            if (HasSkill(4) && (collision.gameObject.layer == 6 || collision.gameObject.layer == 7))
            {
                GameObject aoeGO = Instantiate(InstantiableLibrary.Instance.Aoe1Prefab, transform.position, Quaternion.identity);
                AoeController aoeController = aoeGO.GetComponent<AoeController>();

                aoeController.Init(GetSkillById(4), isFriendly, 0.2f);
            }

            if (HasSkill(7) && ((isFriendly && collision.gameObject.layer == 7) || (!isFriendly && (collision.gameObject.layer == 6))))
            {
                EntityController entityController = collision.gameObject.GetComponent<EntityController>();

                entityController.TakeDamage(1);
            }

            if (HasSkill(8) && ((isFriendly && collision.gameObject.layer == 6) || (!isFriendly && (collision.gameObject.layer == 7))))
            {
                EntityController entityController = collision.gameObject.GetComponent<EntityController>();

                entityController.AddStatus(new Status(StatusDB.statusList[0]));
            }
        }
    }

    public bool HasStatus(Status status)
    {
        for (int i = statuses.Count - 1; i >= 0; i--)
        {
            if (statuses[i].Equals(status))
            {
                return true;
            }
        }

        return false;
    }

    public Status FindStatusById(int statusId)
    {
        for (int i = statuses.Count - 1; i >= 0; i--)
        {
            if (statuses[i].StatusId == statusId)
            {
                return statuses[i];
            }
        }

        return null;
    }

    public void AddStatus(Status status)
    {
        if (!Rotted)
        {
            Status statusTemp;

            switch (status.StatusType)
            {
                case StatusType.Stacking:
                    if (!HasStatus(status))
                    {
                        statuses.Add(status);
                        attachedHealthBar.AddStatus(status);
                    }
                    else
                    {
                        FindStatusById(status.StatusId).Stacks++;
                        attachedHealthBar.UpdateStatus(status);
                    }
                    break;
                case StatusType.StackingAndRefreshing:
                    if (!HasStatus(status))
                    {
                        statuses.Add(status);
                        attachedHealthBar.AddStatus(status);
                    }
                    else
                    {
                        statusTemp = FindStatusById(status.StatusId);
                        statusTemp.Stacks++;
                        statusTemp.Duration = status.BaseDuration;
                        attachedHealthBar.UpdateStatus(status);
                    }
                    break;
                case StatusType.Permanent:
                    if (!HasStatus(status))
                    {
                        statuses.Add(status);
                        attachedHealthBar.AddStatus(status);
                    }
                    break;
            }
        }
    }

    public void RemoveStatus(Status status)
    {
        statuses.Remove(status);
    }

    public void UpdateStatuses()
    {
        if (!Rotted && !GameManager.Instance.EncounterFinished)
        {
            for (int i = statuses.Count - 1; i >= 0; i--)
            {
                Status status = statuses[i];

                status.Duration -= Time.deltaTime;

                if (status.Duration <= 0)
                {
                    switch (status.StatusType)
                    {
                        case StatusType.Permanent:
                            break;
                        case StatusType.Stacking:
                            status.Stacks--;
                            status.Duration = status.BaseDuration;
                            attachedHealthBar.UpdateStatus(status);

                            if (status.Stacks <= 0)
                            {
                                RemoveStatus(status);
                                attachedHealthBar.RemoveStatus(status);
                            }

                            break;
                        case StatusType.StackingAndRefreshing:
                            RemoveStatus(status);
                            attachedHealthBar.RemoveStatus(status);

                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public bool HasTag(string tag)
    {
        if (parentEntity.Tags.Contains(tag))
        {
            return true;
        }

        return false;
    }
}
