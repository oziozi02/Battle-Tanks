using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public int playerIndex = 1; // 1 or 2

    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public Transform barrelTip;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastDirection = Vector2.up;
    private bool canShoot = true;
    private bool onIce = false;
    public float iceSpeedMultiplier = 1.8f;
    private int starLevel = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (playerIndex == 1)
        {
            if (Keyboard.current.wKey.isPressed) input = Vector2.up;
            else if (Keyboard.current.sKey.isPressed) input = Vector2.down;
            else if (Keyboard.current.aKey.isPressed) input = Vector2.left;
            else if (Keyboard.current.dKey.isPressed) input = Vector2.right;
        }
        else
        {
            if (Keyboard.current.upArrowKey.isPressed) input = Vector2.up;
            else if (Keyboard.current.downArrowKey.isPressed) input = Vector2.down;
            else if (Keyboard.current.leftArrowKey.isPressed) input = Vector2.left;
            else if (Keyboard.current.rightArrowKey.isPressed) input = Vector2.right;
        }

        moveInput = input;

        if (moveInput != Vector2.zero)
        {
            lastDirection = moveInput;
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        bool shootPressed = playerIndex == 1
            ? Keyboard.current.spaceKey.isPressed
            : Keyboard.current.enterKey.isPressed;

        if (shootPressed && canShoot)
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        float currentSpeed = onIce ? moveSpeed * iceSpeedMultiplier : moveSpeed;
        rb.linearVelocity = moveInput * currentSpeed;
    }

    public void SetOnIce(bool value)
    {
        onIce = value;
    }

    public void UpgradeStar()
    {
        starLevel = Mathf.Min(starLevel + 1, 3);
    }

    public void ResetStarLevel()
    {
        starLevel = 0;
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
        b.playerIndex = playerIndex;

        if (starLevel >= 1) b.speed = 15f;
        if (starLevel >= 3) b.canDestroySteel = true;

        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        AudioManager.Instance.PlaySFX(AudioManager.Instance.playerShoot);
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }

    public void ResetShootState()
    {
        StopAllCoroutines();
        canShoot = true;
    }
}