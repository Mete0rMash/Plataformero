using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    #region Public Variables
    [HideInInspector] public int maxHealth = 100;
    public Animator animator;
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject hitbox;
    #endregion

    #region Private Variables
    private Vector3 baseScale;
    private int currentHealth;
    private float distance;
    private bool attackMode;
    private bool cooling;
    private float intTimer;
    [SerializeField] private DamagePlayer damage;
    [SerializeField] private float baseCastDist;
    private string facingDirection;
    [SerializeField] private Transform castPos;
    private Rigidbody2D rb2d;
    #endregion

    private void Awake()
    {
        damage = hitbox.GetComponent<DamagePlayer>();

        currentHealth = maxHealth;

        baseScale = transform.localScale;

        facingDirection = "right";

        rb2d = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float vX = moveSpeed;

        if(facingDirection == "left")
        {
            vX = -moveSpeed;
        }

        rb2d.velocity = new Vector2(vX, rb2d.velocity.y);

        animator.SetInteger("AnimState", 2);

        if (IsHittingWall() || IsNearEdge())
        {
            if (facingDirection == "left")
            {
                ChangeFacingDirection("right");
            }
            else ChangeFacingDirection("left");
        }
    }

    void ChangeFacingDirection(string newDirection)
    {
        Vector3 newScale = baseScale;

        if (newDirection == "left")
        {
            newScale.x = -baseScale.x;
        }
        else newScale.x = baseScale.x;

        transform.localScale = newScale;
        facingDirection = newDirection;
    }

    bool IsHittingWall()
    {
        bool val = false;

        float castDist = baseCastDist;

        if (facingDirection == "left")
        {
            castDist = -baseCastDist;
        }
        else castDist = baseCastDist;

        Vector3 targetPos = castPos.position;
        targetPos.x += castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.blue);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = true;
        } else val = false;

        return val;
    }

    bool IsNearEdge()
    {
        bool val = true;

        float castDist = baseCastDist;

        Vector3 targetPos = castPos.position;
        targetPos.y -= castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.red);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = false;
        }
        else val = true;

        return val;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == hitbox.tag)
        {
            currentHealth -= damage.damage;

            animator.SetTrigger("Hurt");

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        animator.SetTrigger("Death");

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
