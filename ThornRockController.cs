using System.Collections;
using UnityEngine;

public class ThornRockController : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 3;
    private Skill parentSkill;
    private bool isFriendly;

    private void Start()
    {
        StartCoroutine(LifetimeCoroutine());
    }

    public void Init(Skill parentSkill, bool isFriendly)
    {
        this.parentSkill = parentSkill;
        this.isFriendly = isFriendly;
    }

    private IEnumerator LifetimeCoroutine()
    {
        yield return new WaitForSeconds(Time.deltaTime);

        lifetime -= Time.deltaTime;

        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }

        StartCoroutine(LifetimeCoroutine());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((isFriendly && collision.gameObject.layer == 7) || (!isFriendly && (collision.gameObject.layer == 6)))
        {
            EntityController entityController = collision.gameObject.GetComponent<EntityController>();

            entityController.TakeDamage(parentSkill.SkillDamage);
        }
    }
}
