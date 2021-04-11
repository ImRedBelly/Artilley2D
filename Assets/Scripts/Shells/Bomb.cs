using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Bomb : MonoBehaviour
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
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<AudioSource>().Play();
        if (collision.gameObject)
        {
            GetComponent<Animator>().SetTrigger("Boom");
            StartCoroutine(DestroyGround(transform.position, 4, 0.01f));
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            
        }
    }
    public void BoomEffect()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3);
        foreach (var coll in colliders)
        {
            if (coll.gameObject.CompareTag("Player") || coll.gameObject.CompareTag("Enemy"))
                coll.SendMessage("ApplyDamage");
        }
        Destroy(gameObject, 0.2f);
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
                {
                    terrain.SetTile(tilePosition, null);
                }

            }
        }

    }

    public void Shot()
    {
        float AngleInDegrees = 45f;
        const float g = 9.8f;

        Vector3 direction = PlayerMovement.instance.transform.position - transform.position;
        direction.z = 0;

        float x = direction.magnitude;
        float y = direction.y;

        float AngleInRadians = AngleInDegrees * Mathf.PI / 180;

        float v2 = (g * x * x) / (2 * (y - Mathf.Tan(AngleInRadians) * x) * Mathf.Pow(Mathf.Cos(AngleInRadians), 2));
        float v = Mathf.Sqrt(Mathf.Abs(v2));

        GetComponent<Rigidbody2D>().isKinematic = false;
        if (transform.position.x > PlayerMovement.instance.transform.position.x)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 1).normalized * v;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1).normalized * v;
        }

    }
}

