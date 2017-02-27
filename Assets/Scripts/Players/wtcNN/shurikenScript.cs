using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class shurikenScript : MonoBehaviour
{
    [SerializeField]
    Vector2 speed = new Vector2(1, 0);
    int shurikenDamage = 2;
    int bulletDamage = 40;
    Rigidbody2D rb;
    public bool faceDir;
    float direction;

    GameObject wtcn;

    void Start ()
    {
        wtcn = GameObject.Find("wtcn");
        Destroy(gameObject, 2);
        rb = GetComponent<Rigidbody2D>();
        direction = wtcn.GetComponent<wtcnnSkills>().SendDirection();
        rb.AddForce(new Vector2(direction * 10, transform.position.y), ForceMode2D.Impulse);
        rb.velocity = speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
            Destroy(gameObject);
            if (this.gameObject.CompareTag("bullet"))
            {
                other.gameObject.GetComponent<Stats>().TakeDamage(bulletDamage);
            }
            else if (this.gameObject.CompareTag("shuriken"))
            {
                other.gameObject.GetComponent<Stats>().TakeDamage(shurikenDamage);
            }
        }
    } 
}
