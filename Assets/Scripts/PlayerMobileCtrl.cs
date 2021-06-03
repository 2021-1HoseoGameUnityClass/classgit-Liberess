using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMobileCtrl : MonoBehaviour
{
    private bool isMove = false;
    
    private int direction;
    private float moveSpeed = 4f;
    private float jumpPower = 6f;
    private int jumpCount = 0;

    private float shotTime = 0f;
    private float shotDelayTime = 0.5f;

    [SerializeField]
    private Transform shotPos;

    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    AudioSource myAudio;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        myAudio = GetComponent<AudioSource>();
    }

    private void Update() => shotTime += Time.deltaTime;

    private void FixedUpdate()
    {
        if(DataManager.Instance.isPlay)
        {
            Move();
            LandingPlatform();
        }
    }

    private void LandingPlatform()
    {
        //Landing Platform
        if (rigid.velocity.y <= 0)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1f, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                {
                    jumpCount = 0;
                    anim.SetBool("isJump", false);
                }
            }
        }
    }

    private void Move()
    {
        if(isMove)
        {
            transform.Translate(new Vector2(direction * moveSpeed * Time.deltaTime, 0));
        }
    }

    public void Jump()
    {
        if (DataManager.Instance.isPlay)
        {
            if (jumpCount < 1)
            {
                jumpCount++;

                anim.SetBool("isWalk", false);
                anim.SetBool("isJump", true);

                rigid.velocity = Vector2.up * jumpPower;
            }
        }
    }

    public void Shot()
    {
        if (DataManager.Instance.isPlay)
        {
            if (shotTime >= shotDelayTime)
            {
                shotTime = 0f;

                float direc = 0;

                if (sprite.flipX)
                {
                    direc = -1; //¿ÞÂÊ
                }
                else
                {
                    direc = 1; //¿À¸¥ÂÊ
                }

                myAudio.Play();

                GameObject bulletPrefab = Instantiate(Resources.Load<GameObject>("Bullet"), shotPos.position, Quaternion.identity);
                bulletPrefab.GetComponent<Bullet>().direction = direc;
            }
        }
    }

    public void OnMove(string _direction)
    {
        if(DataManager.Instance.isPlay)
        {
            if (_direction == "Right")
            {
                direction = 1;
                sprite.flipX = false;
            }
            else
            {
                direction = -1;
                sprite.flipX = true;
            }

            isMove = true;
            anim.SetBool("isWalk", true);
        }
    }

    public void OffMove()
    {
        direction = 0;
        isMove = false;
        anim.SetBool("isWalk", false);
    }

    IEnumerator Hit()
    {
        sprite.color = new Color(1f, 0.3f, 0.3f);

        Instantiate(Resources.Load("Particles/Blood"), transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        sprite.color = new Color(1f, 1f, 1f);

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            StartCoroutine(Hit());
            DataManager.Instance.playerHp--;
            UIManager.Instance.SetHp();
        }

        if(collision.CompareTag("GameController"))
        {
            DataManager.Instance.GameOver();
        }
    }
}