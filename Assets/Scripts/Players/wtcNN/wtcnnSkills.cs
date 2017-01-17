using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class wtcnnSkills : MonoBehaviour
{
    public GameObject shurikenSpawnPoint;
    public GameObject shuriken;
    bool isFacingRight;
    Transform trans;
    PlayerController pc;
    bool isGrounded, canDoubleJump;
	void Start ()
    {
        pc = GetComponent<PlayerController>();
        isGrounded = pc.canJump;
        canDoubleJump = true;

    }
	
	void Update ()
    {
        isGrounded = pc.canJump;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BasicAttack();
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (canDoubleJump)
            {
                DoubleJump();
            }
        }
        if (!isGrounded)
            canDoubleJump = false;
    }

    void BasicAttack()
    {
        Instantiate(shuriken, shurikenSpawnPoint.transform.position, Quaternion.identity);
    }

    void DoubleJump()
    {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
        canDoubleJump = true;
    }
}
