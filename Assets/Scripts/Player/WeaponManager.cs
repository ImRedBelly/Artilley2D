using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponManager : MonoBehaviour, IPointerDownHandler
{
    public static WeaponManager instance;
    [SerializeField] Animator animator;
    [SerializeField] Animator hammerAnimator, gunAnimator;

    public GameObject aim;

    [Header("Gun")]
    public GameObject gun;
    public Transform bulletPosition;
    public GameObject bullet;
    GameObject emptyBullet;
    bool isGun;

    [Header("Hammer")]
    public GameObject hammer;
    bool isHammer;

    [Header("Bomb")]
    public GameObject bomb;
    public Transform bombPosition;
    GameObject emptyBomb;
    bool isBomb;


    PlayerMovement playerMovement;
    float pointerDownTimer = 0;

    Vector3 direction;
    bool isButton;
    private void Start()
    {
        if (instance == null) instance = this;
        playerMovement = PlayerMovement.instance;
    }
    private void Update()
    {
        if (!playerMovement.isAttacking)
        {
            WeaponsOff();
        }



        if (isButton)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended)
            {
                pointerDownTimer += Time.deltaTime;
                if (pointerDownTimer > 1)
                    aim.SetActive(true);
            }
            else
            {
                isButton = false;
                pointerDownTimer = 0;
            }
        }
    }
    public void GetGun()
    {
        WeaponsOff();
        playerMovement.isAttacking = true;
        isGun = true;
        gun.SetActive(true);
        hammer.SetActive(false);
        Destroy(emptyBomb);
    }
    public void GetBomb()
    {
        WeaponsOff();
        isBomb = true;

        if (emptyBomb)
            Destroy(emptyBomb);

        emptyBomb = Instantiate(bomb, bombPosition.position, Quaternion.identity);
        emptyBomb.gameObject.layer = LayerMask.NameToLayer("BombPlayer");
        playerMovement.isAttacking = true;

    }
    public void GetHammer()
    {
        WeaponsOff();
        playerMovement.isAttacking = true;
        isHammer = true;
        gun.SetActive(false);
        hammer.SetActive(true);
        Destroy(emptyBomb);
    }

    public void Attack()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                if (isHammer)
                {

                }
                if (isGun)
                {
                    direction = Camera.main.ScreenToWorldPoint(touch.position) - gun.transform.position;
                    direction.z = 0;
                    if (aim)
                    {
                        aim.transform.right = direction * PlayerMovement.instance.transform.localScale.x;
                        gun.transform.right = -direction * PlayerMovement.instance.transform.localScale.x;
                    }
                }
                if (isBomb)
                {
                    if (emptyBomb)
                        emptyBomb.transform.position = bombPosition.position;

                    direction = Camera.main.ScreenToWorldPoint(touch.position) - gun.transform.position;
                    direction.z = 0;

                    if (aim)
                        aim.transform.right = direction * PlayerMovement.instance.transform.localScale.x;
                }
            }
            else if (touch.phase == TouchPhase.Ended && touch.tapCount > 1)
            {
                if (isHammer)
                {
                    animator.SetTrigger("AttackHammer");
                    hammerAnimator.SetTrigger("Attack");

                    AttackHammer();
                }
                if (isGun)
                {
                    animator.SetTrigger("AttackGun");
                    gunAnimator.SetTrigger("Attack");

                    emptyBullet = Instantiate(bullet, bulletPosition.position, Quaternion.identity);

                    if (aim.activeSelf)
                        emptyBullet.GetComponent<Bullet>().StartFly(aim.transform.right * transform.localScale.x / 5 * 50);

                    else
                        emptyBullet.GetComponent<Bullet>().StartFly(new Vector2(1 * -transform.localScale.x / 5, 0) * 50);
                }
                if (isBomb)
                {
                    animator.SetTrigger("AttackBomb");

                    if (aim.activeSelf)
                        emptyBomb.GetComponent<Bomb>().StartFly(aim.transform.right * transform.localScale.x / 5 * 50);
                    else
                        emptyBomb.GetComponent<Bomb>().StartFly(new Vector2(1 * -transform.localScale.x / 5, 1) * 10);
                    
                }
                //////////////////////////////////////////////////////////
                ///   םאהמ סבטגאעü רûÔוופכרען
                ///////////////
            }
        }
    }

    public void AttackHammer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 7);
        foreach (var coll in colliders)
        {
            if (coll.gameObject.CompareTag("Enemy"))
                coll.SendMessage("ApplyDamage");
        }
    }

    void WeaponsOff()
    {
        aim.SetActive(false);
        gun.SetActive(false);
        hammer.SetActive(false);

        isGun = false;
        isBomb = false;
        isHammer = false;

        if (emptyBomb)
            Destroy(emptyBomb);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData != null)
            isButton = true;
    }
}

