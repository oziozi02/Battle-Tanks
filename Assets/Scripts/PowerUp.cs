using UnityEngine;
using System.Collections;

public enum PowerUpType
{
    Grenade,
    Helmet,
    Shovel,
    Star,
    Tank,
    Timer
}

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public float lifetime = 10f;
    public float blinkStartTime = 4f; // start blinking when this many seconds remain

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(LifetimeCoroutine());
    }

    IEnumerator LifetimeCoroutine()
    {
        float elapsed;

        // Phase 1: wait until blinking should start
        float normalPhase = lifetime - blinkStartTime;
        yield return new WaitForSeconds(normalPhase);
        elapsed = normalPhase;

        // Phase 2: blink with increasing speed
        while (elapsed < lifetime)
        {
            float timeRemaining = lifetime - elapsed;
            float blinkInterval = Mathf.Lerp(0.08f, 0.4f, timeRemaining / blinkStartTime);

            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            int idx = pc != null ? pc.playerIndex : 1;
            PowerUpManager.Instance.ActivatePowerUp(type, pc, ph);
            ScoreManager.Instance.RegisterPowerUp(idx, transform.position);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.powerUpPickup);
            Destroy(gameObject);
        }
    }
}