using UnityEngine;
using System.Collections;

public class EnemyTank : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float directionChangeInterval = 2f;
    public GameObject bulletPrefab;
    public Transform barrelTip;

    private Rigidbody2D rb;
    private Vector2 currentDirection = Vector2.down;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ChangeDirection());
        StartCoroutine(Shoot());
    }

    private Vector2 lastPosition;
    private float stuckTimer = 0f;
    private float stuckThreshold = 0.5f;

    void FixedUpdate()
    {
        rb.linearVelocity = currentDirection * moveSpeed;

        // Check if stuck
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

    void RotateToDirection()
    {
        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
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
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ForceNewDirection();
    }
}
