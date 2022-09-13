using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Public Variables
    [HideInInspector]public int maxHealth = 100;
    public Animator animator;
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;
    public GameObject hitbox;
    #endregion

    #region Private Variables
    private int currentHealth;
    private float distance;
    private bool attackMode;
    private bool cooling;
    private float intTimer;
    private DamagePlayer damage;
    #endregion

    private void Awake()
    {
        damage = hitbox.GetComponent<DamagePlayer>();
        SelectTarget();
        currentHealth = maxHealth;
        intTimer = timer;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!attackMode)
        {
            Move();
        }

        if (!InsideLimits() && !inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            EnemyLogic();
        }
    }

    private void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if (attackDistance >= distance && !cooling) Attack();

        if (cooling)
        {
            Cooldown();
            animator.SetBool("CanAttack", false);
        }
    }

    private void Move()
    {
        animator.SetInteger("AnimState", 2);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        timer = intTimer;
        attackMode = true;

        animator.SetInteger("AnimState", 1);
        animator.SetBool("CanAttack", true);
    }

    private void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    private void StopAttack()
    {
        cooling = false;
        attackMode = false;
        animator.SetBool("CanAttack", false);
    }

    public void TriggerCooldown()
    {
        cooling = true;
    }

    private bool InsideLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        target = distanceToLeft > distanceToRight ? leftLimit:rightLimit;

        Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);

        /*Vector3 rotation = transform.eulerAngles;

        rotation.y = transform.position.x > target.position.x ?   180f:0f;

        transform.eulerAngles = rotation;*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitbox.CompareTag(collision.tag))
        {
            currentHealth -= damage.damage;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Death");

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
