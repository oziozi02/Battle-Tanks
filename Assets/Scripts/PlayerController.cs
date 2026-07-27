using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastDirection = Vector2.up;

    public GameObject bulletPrefab;
    public Transform barrelTip;
    private bool canShoot = true;
    private int starLevel = 0; // 0 = base, 1 = faster bullets, 2 = double bullets, 3 = destroys steel

    public void UpgradeStar()
    {
        starLevel = Mathf.Min(starLevel + 1, 3);
    }

    public void ResetStarLevel()
    {
        starLevel = 0;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            input = Vector2.up;
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            input = Vector2.down;
        else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            input = Vector2.left;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            input = Vector2.right;

        moveInput = input;
        // Rotation
        if (moveInput != Vector2.zero)
        {
            lastDirection = moveInput;
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        // Shooting
        if (Keyboard.current.spaceKey.isPressed && canShoot)
        {
            Shoot();
        }
    }

    private bool onIce = false;
    public float iceSpeedMultiplier = 1.8f;

    public void SetOnIce(bool value)
    {
        onIce = value;
    }

    void FixedUpdate()
    {
        float currentSpeed = onIce ? moveSpeed * iceSpeedMultiplier : moveSpeed;
        rb.linearVelocity = moveInput * currentSpeed;
    }

    void Shoot()
    {
        canShoot = false;
        FireSingleBullet();

        if (starLevel >= 2)
        {
            StartCoroutine(FireSecondBulletDelayed());
        }

        StartCoroutine(ShootCooldown());
    }

    IEnumerator FireSecondBulletDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        FireSingleBullet();
    }

    void FireSingleBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, barrelTip.position, Quaternion.identity);
        Bullet b = bullet.GetComponent<Bullet>();
        b.SetDirection(lastDirection);
        b.SetOwner(Bullet.OwnerType.Player);

        if (starLevel >= 1)
        {
            b.speed = 15f;
        }

        if (starLevel >= 3)
        {
            b.canDestroySteel = true;
        }

        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }
}