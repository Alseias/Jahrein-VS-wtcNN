using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class shurikenScript : MonoBehaviour
{
    [SerializeField]
    Vector2 speed = new Vector2(1,0);
    float damage = 2f;

    Rigidbody2D rb;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;
        Destroy(gameObject, 2);
	}
	
	void Update ()
    {
        rb.velocity = speed;
	}

    void OnTriggerEnter2D(Collider2D other)//was collision2d
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            other.gameObject.GetComponent<PlayerController>().takeHit(damage);//was gameobject.sendmessage or smt
        }
    } 
}
