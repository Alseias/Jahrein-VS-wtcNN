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
<<<<<<< HEAD
    AbilityCoolDown[] skillCoolDownCheck = new AbilityCoolDown[4];
    void setSkills()
    {
        GameObject skillCanvas = GameObject.Find("SkillSet");
        GameObject skillUI;
        for(int i = 0; i < 4; i++) {
            skillUI = Instantiate(skillUiPref, new Vector3(100 + (100 * i), 50, 0), skillCanvas.transform.rotation, skillCanvas.transform) as GameObject;
            skillUI.GetComponentInChildren<Image>().sprite = skillSprites[i];
=======
>>>>>>> origin/master

    void Update ()
    {
        if(pv.isMine)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                pv.RPC("BasicAttack", PhotonTargets.All);
            }
<<<<<<< HEAD
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                if(pc.canJump)
                {
                    Debug.Log("zıpladık");
=======
            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                if(pc.canJump) {
>>>>>>> origin/master
                    pc.canJump = false;
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
                    canDoubleJump = true;
<<<<<<< HEAD
                }
                else
                {
                    if(canDoubleJump)
                    {
                        Debug.Log("zıpladık2x");
=======
                } else {
                    if(canDoubleJump) {

>>>>>>> origin/master
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
<<<<<<< HEAD
=======

    void DoubleJump()
    {

        
        canDoubleJump = false;
    }
    void fnc123asdkAAn2k17() {

    }
>>>>>>> origin/master
}
