using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public enum OwnerType { Player, Enemy }
    public OwnerType owner;

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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

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
            }
        }

        // Damage enemy
        if (owner == OwnerType.Player)
        {
            EnemyTank enemy = other.GetComponent<EnemyTank>();
            if (enemy != null)
            {
                enemy.TakeDamage();
                Destroy(gameObject);
                return;
            }
        }

        // Brick destruction (unchanged)
        TilemapCollider2D tilemapCollider = other.GetComponent<TilemapCollider2D>();
        if (tilemapCollider != null)
        {
            Tilemap tilemap = tilemapCollider.GetComponent<Tilemap>();
            if (tilemap != null && tilemap.name == "BrickTilemap")
            {
                BoundsInt area = new BoundsInt(
                    tilemap.WorldToCell(transform.position) - new Vector3Int(1, 1, 0),
                    new Vector3Int(3, 3, 1)
                );

                Vector3Int bestTile = Vector3Int.zero;
                float bestScore = float.MaxValue;
                bool found = false;

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

                if (found)
                {
                    tilemap.SetTile(bestTile, null);
                }
            }
        }

        Destroy(gameObject);
    }
}