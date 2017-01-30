using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class wtcnnSkills : Photon.PunBehaviour
{
    public GameObject shurikenSpawnPoint;
    public GameObject shuriken;


    bool isFacingRight;
    Transform trans;
    PlayerController pc;
    PhotonView pv;
    bool canDoubleJump=false;

    void Start ()
    {
        pc = GetComponent<PlayerController>();
        pv = GetComponent<PhotonView>();

    }

    void Update ()
    {
        if(pv.isMine) {
            
            if(Input.GetKeyDown(KeyCode.Space)) {
                pv.RPC("BasicAttack", PhotonTargets.All);
            }
            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                if(pc.canJump) {
                    pc.canJump = false;
                    
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
                    canDoubleJump = true;
                } else {
                    if(canDoubleJump) {

                        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
                        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
                        canDoubleJump = false;
                    }
                }
                 
            }

        }
    }

    [PunRPC]
    void BasicAttack()
    {
        Instantiate(shuriken, shurikenSpawnPoint.transform.position, Quaternion.identity);
    }

    void DoubleJump()
    {

        
        canDoubleJump = false;
    }
    void fnc123asdkAAn2k17() {

    }
}
