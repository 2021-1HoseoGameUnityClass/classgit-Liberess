using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private float moveSpeed = 4f;
    private float jumpPower = 6f;

    private float shotTime = 0f;
    private float shotDelayTime = 0.5f;

    private bool isPlatform;

    public Transform shotPos;
    public Transform checkPos;

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

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        Jump();
        Shot();
    }

    private void FixedUpdate()
    {
        Move();
        CheckPlatform();
    }

    #region 움직임
    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal");

        transform.Translate(new Vector2(inputX * moveSpeed * Time.deltaTime, 0));

        if (inputX > 0)
        {
            sprite.flipX = false;
            anim.SetBool("isWalk", true);
        }
        else if (inputX < 0)
        {
            sprite.flipX = true;
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJump"))
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isJump", true);

            rigid.velocity = Vector2.up * jumpPower;
        }
    }
    #endregion

    private void Shot()
    {
        if (shotTime >= shotDelayTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                shotTime = 0f;

                float direction = 0;

                if (sprite.flipX)
                {
                    direction = -1; //왼쪽
                }
                else
                {
                    direction = 1; //오른쪽
                }

                myAudio.Play();

                GameObject bulletPrefab = Instantiate(Resources.Load<GameObject>("Bullet"), shotPos.position, Quaternion.identity);
                bulletPrefab.GetComponent<Bullet>().direction = direction;
            }
        }
        else
        {
            shotTime += Time.deltaTime;
        }
    }

    private void CheckPlatform()
    {
        isPlatform = Physics2D.OverlapCircle(checkPos.transform.position, 0.05f, LayerMask.GetMask("Platform"));

        if (isPlatform)
        {
            anim.SetBool("isJump", false);
        }
    }
}