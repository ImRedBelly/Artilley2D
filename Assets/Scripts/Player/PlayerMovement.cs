using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    bool imMain = false;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer playerSpriteRenderer;

    [SerializeField] SpriteRenderer crown;
    [SerializeField] float speed = 5;
    PlayerHealth playerHealth;
    float inputX;



    [Header("Ground")]
    bool isGround;
    bool facingRight = true;
    public LayerMask layerMask;

    [Header("Weapons")]
    public bool isAttacking;


    public TextMesh namePlayer;
    StatePlayer statePlayer;
    enum StatePlayer
    {
        Attack,
        Move
    }
    void Awake()
    {
        if (instance == null) instance = this;
        playerHealth = GetComponent<PlayerHealth>();
        statePlayer = StatePlayer.Move;
    }
    private void Start()
    {
        namePlayer.text =  PlayerCreator.instance.namePlayer;
        playerSpriteRenderer.color = PlayerCreator.instance.colorPlayer;
    }

    void Update()
    {
        // тут способность принимать урон
        // тут анимация

        if (!imMain)
        {
            crown.color = Color.red;
            animator.SetFloat("Speed", 0);
            return;
        }

        isGround = Physics2D.Raycast(transform.position, -transform.up, 0.5f, layerMask);
        crown.color = Color.green;
        switch (statePlayer)
        {
            case StatePlayer.Move:
                if (isAttacking)
                    statePlayer = StatePlayer.Attack;
                if (imMain)
                    Move();
                break;
            case StatePlayer.Attack:
                if (!isAttacking)
                    statePlayer = StatePlayer.Move;
                WeaponManager.instance.Attack();
                break;
        }

        if (inputX > 0 && facingRight)
            Flip();

        else if (inputX < 0 && !facingRight)
            Flip();

    }
    void Move()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                float middleScreen = Screen.width / 3 + Screen.width / 3;


                if (touch.tapCount == 2 && isGround && touch.position.x < middleScreen && touch.position.x > Screen.width / 3)
                {
                    Jump(new Vector2(0, 1));
                    return;
                }
                else if (touch.tapCount == 2 && isGround && touch.position.x > middleScreen)
                {
                    Jump(new Vector2(0.5f, 1));
                    return;
                }
                else if (touch.tapCount == 2 && isGround && touch.position.x < Screen.width / 3)
                {
                    Jump(new Vector2(-0.5f, 1));

                    return;
                }
            }


            if (Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended)// if (touch.tapCount == 1)  //(touch.phase == TouchPhase.Moved)
            {

                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;

                float middleScreen = Screen.width / 3;
                if (touch.position.x > middleScreen * 2)
                {
                    inputX = 1;
                }
                if (touch.position.x < middleScreen)
                {
                    inputX = -1;
                }

                animator.SetFloat("Speed", Mathf.Abs(inputX));
                rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
        }
    }

    void Jump(Vector2 direction)
    {
        animator.SetTrigger("Jump");
        rb.AddForce(direction * 25, ForceMode2D.Impulse);
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        namePlayer.transform.localScale = new Vector2(namePlayer.transform.localScale.x * -1, namePlayer.transform.localScale.y);
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up);
    }

    public void SetImMain(bool enable)
    {
        imMain = enable;
        isAttacking = false;
    }
}
