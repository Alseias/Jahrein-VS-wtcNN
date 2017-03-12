using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class shurikenScript : MonoBehaviour
{
    [SerializeField]
    Vector2 speed = new Vector2(1, 0);
    float shurikenDamage = 2f;
    float bulletDamage = 40f;
    Rigidbody2D rb;
    public bool faceDir;
    public float direction, fDir;
    public int pvID;
    GameObject wtcn;
    void dir(bool _dir)
    {
        fDir = -1;
        if (_dir)
        {
            fDir = 1;
        }
    }

    void id(int _id)
    {
        pvID = _id;
    }

    void Start()
    {
        Destroy(gameObject, 2);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed * (faceDir ? 1 : -1);
        //rb.AddForce(new Vector2(direction * 10, transform.position.y) * (faceDir ? 1 : -1), ForceMode2D.Impulse);
        rb.velocity = speed * fDir;
        //GetComponent<PhotonView>().RPC("setVelocity", PhotonTargets.All);
    }

    [PunRPC]
    void setVelocity()
    {
        rb.velocity = speed * fDir;

    }
    private void Update()
    {
        rb.AddForce(transform.right * 10f * fDir);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PhotonView>() != null)
        {
            if (other.GetComponent<PhotonView>().viewID != pvID)
            {
                if (other.gameObject.CompareTag("enemy"))
                {
                    if (this.gameObject.CompareTag("bullet"))
                    {
                        other.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, bulletDamage);
                    }
                    else if (this.gameObject.CompareTag("shuriken"))
                    {
                        other.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, shurikenDamage);
                    }
                    Destroy(gameObject);


                }
            }
        }

    }
}