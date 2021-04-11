using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    Tilemap terrain;
    GameObject emptyTilemap;
    private void Start()
    {
        emptyTilemap = GameObject.Find("Tilemap");
        terrain = emptyTilemap.GetComponent<Tilemap>();
    }
    public void StartFly(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            collision.gameObject.SendMessage("ApplyDamage");


        if (collision.gameObject.CompareTag("Ground"))
            StartCoroutine(DestroyGround(transform.position, 2, 0.01f));

        Destroy(gameObject, 0.15f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }



    IEnumerator DestroyGround(Vector3 explosionCentre, float explosionRadius, float time)
    {
        yield return new WaitForSeconds(time);
        for (int x = -(int)explosionRadius; x < explosionRadius; x++)
        {
            for (int y = -(int)explosionRadius; y < explosionRadius; y++)
            {
                Vector3Int tilePosition = terrain.WorldToCell(explosionCentre + new Vector3(x, y, 0));
                if (terrain.GetTile(tilePosition) != null)
                    terrain.SetTile(tilePosition, null);
            }
        }
    }
}
