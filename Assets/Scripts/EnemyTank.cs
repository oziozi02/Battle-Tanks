using UnityEngine;
using System.Collections;

public class EnemyTank : MonoBehaviour
{
    public enum TankType { Basic, Fast, Power, Armor }
    public TankType tankType = TankType.Basic;

    public float moveSpeed = 2f;
    public float bulletSpeed = 10f;
    public int health = 1;
    public float directionChangeInterval = 2f;
    public GameObject bulletPrefab;
    public Transform barrelTip;

    private Rigidbody2D rb;
    private Vector2 currentDirection = Vector2.down;
    private SpriteRenderer sr;
    private int maxHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        maxHealth = health;
        ConfigureByType();
        StartCoroutine(ChangeDirection());
        StartCoroutine(Shoot());
    }

    void ConfigureByType()
    {
        switch (tankType)
        {
            case TankType.Basic:
                moveSpeed = 2f; bulletSpeed = 8f; health = 1;
                sr.color = new Color(0.937f, 0.325f, 0.314f); // red
                break;
            case TankType.Fast:
                moveSpeed = 4f; bulletSpeed = 10f; health = 1;
                sr.color = new Color(1f, 0.439f, 0.263f); // orange
                break;
            case TankType.Power:
                moveSpeed = 2.5f; bulletSpeed = 14f; health = 1;
                sr.color = new Color(0.671f, 0.278f, 0.737f); // purple
                break;
            case TankType.Armor:
                moveSpeed = 2.5f; bulletSpeed = 10f; health = 4;
                sr.color = new Color(0.4f, 0.733f, 0.416f); // green
                break;
        }
        maxHealth = health;
    }

    public void TakeDamage()
    {
        health--;

        if (tankType == TankType.Armor)
        {
            // Gradually turn gray as it takes damage
            float healthPercent = (float)health / maxHealth;
            sr.color = Color.Lerp(Color.gray, new Color(0.4f, 0.733f, 0.416f), healthPercent);
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = currentDirection * moveSpeed;
    }

    void RotateToDirection()
    {
        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private Vector2 lastPosition;
    private float stuckTimer = 0f;
    private float stuckThreshold = 0.5f;

    void Update()
    {
        if (Vector2.Distance(rb.position, lastPosition) < 0.01f)
        {
            stuckTimer += Time.fixedDeltaTime;
            if (stuckTimer >= stuckThreshold)
            {
                ForceNewDirection();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        lastPosition = rb.position;
    }

    void ForceNewDirection()
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        Vector2 newDirection;
        do
        {
            newDirection = directions[Random.Range(0, directions.Length)];
        } while (newDirection == currentDirection);

        currentDirection = newDirection;
        RotateToDirection();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ForceNewDirection();
    }

    IEnumerator ChangeDirection()
    {
        while (true)
        {
            Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            currentDirection = directions[Random.Range(0, directions.Length)];
            RotateToDirection();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            GameObject bullet = Instantiate(bulletPrefab, barrelTip.position, Quaternion.identity);
            Bullet b = bullet.GetComponent<Bullet>();
            b.SetDirection(currentDirection);
            b.SetOwner(Bullet.OwnerType.Enemy);
            b.speed = bulletSpeed;
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}