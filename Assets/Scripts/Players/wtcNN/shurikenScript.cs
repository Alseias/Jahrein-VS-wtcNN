using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class shurikenScript : MonoBehaviour
{
    int shurikenDamage = 2;
    Rigidbody2D rb;
    public float fDir;
    public int pvID;

    void Start ()
    {
        Destroy(gameObject, 2);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.AddForce(transform.right * 10f * fDir);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<PhotonView>() != null)
        {
            if(other.GetComponent<PhotonView>().viewID != pvID)
            {
                if(other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("Player"))
                {
                    Destroy(gameObject);
                    other.gameObject.GetComponent<Stats>().TakeDamage(shurikenDamage);
                }
            }
        }
    } 
}
