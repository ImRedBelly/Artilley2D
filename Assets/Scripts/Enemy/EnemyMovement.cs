using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public TextMesh nameEnemy;
    public GameObject bomb;
    public Transform bombPosition;
    public SpriteRenderer crown;

    [SerializeField] float speed = 3;

    Rigidbody2D rb;
    Animator animator;
    GameObject emptyBomb;

    float health = 5;
    bool imMain = false;
    bool imAttack = false;
    bool facingRight;
    void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        nameEnemy.text = gameObject.name;
        GameManager.instance.AddEnemy(this);
    }
    void Update()
    {

        if (!imMain)
        {
            crown.color = Color.red;
            animator.SetFloat("Speed", 0);
            return;
        }
        else
        {
            crown.color = Color.green;

            if (imAttack)
                StartCoroutine(DoDamage());

        }
    }


    IEnumerator DoDamage()
    {
        imAttack = false;
        float timerMove = 2;
        while (timerMove > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);

            timerMove -= Time.deltaTime;
            Move();
        }
        yield return new WaitForSeconds(0.1f);

        emptyBomb = Instantiate(bomb, bombPosition.position, Quaternion.identity);
        emptyBomb.gameObject.layer = LayerMask.NameToLayer("BombEnemy");
        emptyBomb.GetComponent<Bomb>().Shot();
        animator.SetTrigger("AttackBomb");
    }

    void Move()
    {
        float distToPlayer = PlayerMovement.instance.transform.position.x - transform.position.x;

        if (Mathf.Abs(distToPlayer) < 12)
        {
            animator.SetFloat("Speed", 0);
            return;
        }

        rb.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * speed, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        if (distToPlayer < 0 && facingRight)
            Flip();

        else if (distToPlayer > 0 && !facingRight)
            Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        nameEnemy.transform.localScale = new Vector2(nameEnemy.transform.localScale.x * -1, nameEnemy.transform.localScale.y);
    }


    public void ApplyDamage()
    {
        health--;
        animator.SetTrigger("Hit");
        if (health <= 0)
        {
            //animator.SetTrigger("Dead");
            GameManager.instance.RemoveEnemy(this);
            Destroy(gameObject, 0.1f);
        }
    }

    public void SetImMain(bool enable)
    {
        imMain = enable;
        imAttack = enable;
    }
}
