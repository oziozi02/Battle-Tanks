using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public enum OwnerType { Player, Enemy }
    public OwnerType owner;
    public bool canDestroySteel = false;
    public int playerIndex = 1;

    public float speed = 10f;
    private Vector2 direction;
    private bool hasHit = false;

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    public void SetOwner(OwnerType ownerType)
    {
        owner = ownerType;
    }

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;

        // Rotate sprite to face travel direction (sprite's default "up" orientation assumed)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        // Check for water/ice pass-through FIRST, before setting hasHit
        TilemapCollider2D tilemapCollider = other.GetComponent<TilemapCollider2D>();
        if (tilemapCollider != null)
        {
            Tilemap tilemap = tilemapCollider.GetComponent<Tilemap>();
            if (tilemap != null && (tilemap.name == "WaterTilemap" || tilemap.name == "IceTilemap"))
            {
                return; // bullets pass through water and ice harmlessly, hasHit stays false
            }
        }

        // Ignore bullets hitting their own side
        if (owner == OwnerType.Player && other.GetComponent<EnemyTank>() == null && other.CompareTag("Player"))
            return;
        if (owner == OwnerType.Enemy && other.CompareTag("Enemy"))
            return;

        hasHit = true;

        // Damage player
        if (owner == OwnerType.Enemy)
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage();
                AudioManager.Instance.PlaySFX(AudioManager.Instance.bulletImpact);
            }
        }

        // Damage enemy
        if (owner == OwnerType.Player)
        {
            EnemyTank enemy = other.GetComponent<EnemyTank>();
            if (enemy != null)
            {
                enemy.TakeDamage(playerIndex);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.bulletImpact);
                Destroy(gameObject);
                return;
            }
        }

        // Brick/Steel destruction
        if (tilemapCollider != null)
        {
            Tilemap tilemap = tilemapCollider.GetComponent<Tilemap>();
            if (tilemap != null && (tilemap.name == "BrickTilemap" || (tilemap.name == "SteelTilemap" && canDestroySteel)))
            {
                Vector3Int currentCell = tilemap.WorldToCell(transform.position);
                bool found = false;
                Vector3Int bestTile = Vector3Int.zero;

                // Priority check: is the bullet already inside/overlapping a valid tile?
                if (tilemap.GetTile(currentCell) != null)
                {
                    bestTile = currentCell;
                    found = true;
                }
                else
                {
                    BoundsInt area = new BoundsInt(
                        currentCell - new Vector3Int(1, 1, 0),
                        new Vector3Int(3, 3, 1)
                    );

                    float bestScore = float.MaxValue;

                    foreach (Vector3Int pos in area.allPositionsWithin)
                    {
                        if (tilemap.GetTile(pos) != null)
                        {
                            Vector3 tileCenter = tilemap.GetCellCenterWorld(pos);
                            Vector3 toTile = tileCenter - transform.position;
                            float dot = Vector2.Dot(direction, new Vector2(toTile.x, toTile.y));
                            float distance = Vector3.Distance(tileCenter, transform.position);

                            if (dot >= 0 && distance < bestScore)
                            {
                                bestScore = distance;
                                bestTile = pos;
                                found = true;
                            }
                        }
                    }

                    if (!found)
                    {
                        bestScore = float.MaxValue;
                        foreach (Vector3Int pos in area.allPositionsWithin)
                        {
                            if (tilemap.GetTile(pos) != null)
                            {
                                Vector3 tileCenter = tilemap.GetCellCenterWorld(pos);
                                float distance = Vector3.Distance(tileCenter, transform.position);
                                if (distance < bestScore)
                                {
                                    bestScore = distance;
                                    bestTile = pos;
                                    found = true;
                                }
                            }
                        }
                    }
                }

                if (found)
                {
                    tilemap.SetTile(bestTile, null);
                }
            }
        }
        Destroy(gameObject);
    }
}