using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    private bool isDead = false;

    private int health = 3;
    private int direction = 1;
    private float moveSpeed = 2f;

    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer sprite;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        CheckRay();
        Move();
    }

    private void CheckRay()
    {
        if (!isDead)
        {
            Vector2 frontVec = new Vector2(rigid.position.x + direction, rigid.position.y);
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1.1f, LayerMask.GetMask("Platform"));

            if (rayHit.collider == null)
            {
                direction *= -1;
            }
        }
    }

    private void Move()
    {
        if (!isDead)
        {
            transform.Translate(new Vector3(moveSpeed * direction * Time.deltaTime, 0, 0));
            //anim.SetBool("isWalk", true);

            sprite.flipX = direction == -1;
        }
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
        if (collision.CompareTag("Bullet"))
        {
            if(!isDead)
            {
                health--;
                StartCoroutine(Hit());

                if (health < 1)
                {
                    //anim.SetBool("isWalk", false);
                    //anim.SetBool("isDead", true);
                    isDead = true;
                    gameObject.tag = "Untagged";
                    anim.SetTrigger("doDead");

                    Destroy(gameObject, 0.5f);
                }
            }
        }
    }
}