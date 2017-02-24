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
    public Player _player;
    public bool faceDir;
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;
        Destroy(gameObject, 2);
        Vector2 targetDir = GetComponent<Player>().target.transform.position - transform.position;
        Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDir), 500f);
        transform.rotation = rot;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);

    }

	void Update ()
    {
        rb.velocity = speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Did damage");
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
