using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public Rigidbody2D rb;
    private Vector3 direction;
    public Animator animator;
    public GameObject[] deathEffect;
    public GameObject bullet;
    public Transform firePoint;

    public float enemySpeed;
    public float playerRange;
    public int health = 100;
    public float attackCooldown = 2f;
    private float attackTimer;
    public bool chasePlayer;
    public bool runAway;
    public float runRange;
    public bool shouldShoot;
    public float rateOfFire;
    private float fireCounter;
    public float shootRange;
    private float timeSinceLastMovement;
    public float maxTimeBetweenMovements = 3f;
    public float changeDirectionCooldown = 1.0f;
    private float lastDirectionChangeTime = 0.0f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartInactive();
        enemySpeed = 3;
        playerRange = 7;
    }

    void Update()
    {
        if (PlayerManager.instance.playerSprite.isVisible && PlayerManager.instance.gameObject.activeInHierarchy)
        {
            direction = new Vector3 (0f ,0f, 0f);
            if (chasePlayer && Vector3.Distance(transform.position, PlayerManager.instance.transform.position) < playerRange)
            {
                direction = PlayerManager.instance.transform.position - transform.position;
                float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
                angle = Mathf.Round(angle / 180f) * 180f;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

            }
            if (runAway && Vector3.Distance(transform.position, PlayerManager.instance.transform.position) < runRange)
            {
                direction = transform.position - PlayerManager.instance.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                angle = Mathf.Round(angle / 180f) * 180f;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            direction.Normalize();
            rb.velocity = direction * enemySpeed;

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position + direction * 0.5f, 0.1f);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                direction = Vector3.zero;
                break;
            }
        }

            if (direction != new Vector3(0f, 0f, 0f))
            {
                animator.SetBool("isMoving", true);
                timeSinceLastMovement = 0f;
            }
            else
            {
                animator.SetBool("isMoving", false);
                timeSinceLastMovement += Time.deltaTime;

                if (timeSinceLastMovement > maxTimeBetweenMovements)
                {
                    direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
                    timeSinceLastMovement = 0f;
                }
                if (runAway)
                {
                    ChangeDirection();
                }
            }

            if (shouldShoot && Vector3.Distance(transform.position, PlayerManager.instance.transform.position) < shootRange)
            {
                fireCounter -= Time.deltaTime;
                if (fireCounter <= 0)
                {
                    fireCounter = rateOfFire;
                    Instantiate(bullet, firePoint.position, firePoint.rotation);
                }
            }

            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
        }
        else
        {
            rb.velocity = new Vector2(0f, 0f);
        }
    }

    private void ChangeDirection()
    {
        if (Time.time - lastDirectionChangeTime > changeDirectionCooldown)
        {
            direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
            rb.velocity = direction * enemySpeed;
            timeSinceLastMovement = 0f;
            lastDirectionChangeTime = Time.time;
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        AudioManager.instance.PlaySFX(2);
        if (health <= 0)
        {
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(1);
            int randomEffect = Random.Range(0, deathEffect.Length);
            int rotation = Random.Range(0, 4);
            Instantiate(deathEffect[randomEffect], transform.position, Quaternion.Euler(0f, 0f, rotation * 90f));
        }
    }

    private void ApplyDamage(GameObject player)
    {
        if (attackTimer <= 0)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(0.5f);
            attackTimer = attackCooldown;
        }
    }

    public void StartInactive()
    {
        gameObject.SetActive(false);
    }

    public void ActivateEnemy()
    {
        gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ApplyDamage(collision.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ApplyDamage(collision.gameObject);
        }
    }
}
