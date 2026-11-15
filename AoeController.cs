using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AoeController : MonoBehaviour
{
    [SerializeField]
    private ColorTransition colorTransition;

    private Skill parentSkill;
    private bool isFriendly = true;
    private Collider2D col;

    private List<Collider2D> previousCollisions = new List<Collider2D>();

    private float lifetime = 1;

    private void Awake()
    {
       col = GetComponent<Collider2D>();
    }

    public void Init(Skill parentSkill, bool isFriendly, float lifetime)
    {
        this.parentSkill = parentSkill;
        this.isFriendly = isFriendly;
        this.lifetime = lifetime;

        StartCoroutine(LifetimeCoroutine());
    }

    public IEnumerator LifetimeCoroutine()
    {
        yield return new WaitForSeconds(Time.deltaTime);

        lifetime -= Time.deltaTime;

        if (lifetime <= 0)
        {
            StartCoroutine(FadeToDestroy());
        }
        else
        {
            StartCoroutine(LifetimeCoroutine());
        }
    }

    public IEnumerator FadeToDestroy()
    {
        colorTransition.StartTransition(ColorTransition.TransitionDirection.startToEnd);

        yield return new WaitUntil(() => colorTransition.transitionDone);

        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!previousCollisions.Contains(collision) && lifetime > 0)
        {
            if ((isFriendly && collision.gameObject.layer == 7) || (!isFriendly && (collision.gameObject.layer == 6)))
            {
                previousCollisions.Add(collision);

                Vector2 dir = collision.gameObject.transform.position - gameObject.transform.position;

                EntityController entityController = collision.gameObject.GetComponent<EntityController>();
                entityController.TakeDamage(parentSkill.SkillDamage);
                entityController.ApplyKnockback(dir * parentSkill.SkillKnockback);

                Instantiate(InstantiableLibrary.Instance.CollisionPE, collision.gameObject.transform.position, Quaternion.identity);
            }
        }
    }
}
