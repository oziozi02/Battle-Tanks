using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;
    private bool hasHit = false;

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;
        hasHit = true;

        TilemapCollider2D tilemapCollider = other.GetComponent<TilemapCollider2D>();
        if (tilemapCollider != null)
        {
            Tilemap tilemap = tilemapCollider.GetComponent<Tilemap>();
            if (tilemap != null && tilemap.name == "BrickTilemap")
            {
                // Get the closest tile by checking a small area around the bullet
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

                        // Check tile is in front of bullet direction
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