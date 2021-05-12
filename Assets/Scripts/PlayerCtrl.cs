using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private float moveSpeed = 4f;
    private float jumpPower = 6f;

    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer sprite;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal");

        //rigid.velocity = new Vector2(inputX * moveSpeed, rigid.velocity.y);
        transform.Translate(new Vector2(inputX * moveSpeed * Time.deltaTime, 0));

        if(inputX > 0)
        {
            sprite.flipX = false;
            anim.SetBool("isWalk", true);
        }
        else if(inputX < 0)
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
        if (Input.GetButtonDown("Jump"))
        {
            rigid.velocity = Vector2.up * jumpPower;
        }
    }
}