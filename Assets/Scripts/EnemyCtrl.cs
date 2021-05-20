using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public GameObject rayPos;

    private bool isDead = false;

    private int health = 3;
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
        if (!isDead)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(rayPos.transform.position, Vector2.down, 1.1f, LayerMask.GetMask("Platform"));

            if (!rayHit)
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