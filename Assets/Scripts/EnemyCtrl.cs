using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public GameObject rayPos;

    [SerializeField]
    private bool isDead = false;

    [SerializeField]
    private int health = 3;

    [SerializeField]
    private int direction = 1;
    private float moveSpeed = 2f;

    Animator anim;
    SpriteRenderer sprite;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        CheckRay();
        Move();
    }

    private void CheckRay()
    {
        if(!isDead)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(rayPos.transform.position, Vector2.down, 1.1f, LayerMask.GetMask("Platform"));

            if(!rayHit)
            {
                direction *= -1;
            }
        }
    }

    private void Move()
    {
        if(!isDead)
        {
            transform.Translate(new Vector3(moveSpeed * direction * Time.deltaTime, 0, 0));
            //anim.SetBool("isWalk", true);

            sprite.flipX = direction == -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bullet"))
        {
            health--;

            if(health < 1)
            {
                //anim.SetBool("isWalk", false);
                //anim.SetBool("isDead", true);
                anim.SetTrigger("doDead");

                isDead = true;

                Destroy(gameObject, 1f);
            }
        }
    }
}