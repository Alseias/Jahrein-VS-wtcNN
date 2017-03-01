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
    void dir(bool _dir)
    {
        faceDir = _dir;
    }
    void Start ()
    {
        Destroy(gameObject, 2);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed * (faceDir ? 1 : -1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("Player"))
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
