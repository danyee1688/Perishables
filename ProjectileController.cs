using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileController : MonoBehaviour
{
    private bool isFriendly = true;
    private Rigidbody2D rb;
    private Skill parentSkill;
    private ProjectileBehavior behavior = ProjectileBehavior.Normal;
    private Vector2 initialDirection;
    private float initialForce;
    private float additionalForce = 1;
    private float angleOfSpiral = 5f;
    private float lifetime = 0;
    private Vector2 initialPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        lifetime += Time.fixedDeltaTime;

        if (behavior == ProjectileBehavior.Spiral)
        {
            additionalForce += additionalForce * Time.fixedDeltaTime;

            rb.linearVelocity = Vector2.zero;

            rb.AddForce(initialDirection * (initialForce + additionalForce));

            initialDirection = (Quaternion.Euler(0, 0, angleOfSpiral) * initialDirection).normalized;
        }
    }

    public void Init(Skill parentSkill, bool isFriendly, Vector2 direction, float force, ProjectileBehavior behavior)
    {
        this.isFriendly = isFriendly;
        this.parentSkill = parentSkill;
        this.behavior = behavior;

        initialDirection = direction;
        initialForce = force;
        initialPos = transform.position;

        Move();
    }

    private void Move()
    {
        if (behavior == ProjectileBehavior.Normal)
        {
            rb.AddForce(initialDirection * initialForce, ForceMode2D.Impulse);
        }
        else if (behavior == ProjectileBehavior.Spiral)
        {
            additionalForce = 5;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }

        if ((isFriendly && collision.gameObject.layer == 7) || (!isFriendly && (collision.gameObject.layer == 6)))
        {
            EntityController entityController = collision.gameObject.GetComponent<EntityController>();
            entityController.TakeDamage(parentSkill.SkillDamage);
            entityController.ApplyKnockback(rb.linearVelocity.normalized * parentSkill.SkillKnockback);

            Instantiate(InstantiableLibrary.Instance.CollisionPE, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public enum ProjectileBehavior
{
    Normal,
    Spiral
}