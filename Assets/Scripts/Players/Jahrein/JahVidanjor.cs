using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JahVidanjor : MonoBehaviour
{
    private void Update()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.right * 50f);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().takeHit(.25f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
