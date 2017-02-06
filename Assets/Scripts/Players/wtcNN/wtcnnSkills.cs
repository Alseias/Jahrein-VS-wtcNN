﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class wtcnnSkills : Photon.PunBehaviour
{
    public GameObject shurikenSpawnPoint;
    public GameObject shuriken;
    public Sprite[] skillSprites;
    public GameObject skillUiPref;

    bool isFacingRight;
    Transform trans;
    PlayerController pc;
    PhotonView pv;
    bool canDoubleJump = false;
    string[] skillKeyMaps = { "SkillQ", "SkillW", "SkillE", "SkillR" };
    float[] skillCoolDowns = { 4, 4, 7, 25 };

    Animator anim = new Animator();

    void Start()
    {
        pc = GetComponent<PlayerController>();
        pv = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();

        if (photonView.isMine)
            setSkills();

    }
    AbilityCoolDown[] skillCoolDownCheck = new AbilityCoolDown[4];
    void setSkills()
    {
        GameObject skillCanvas = GameObject.Find("SkillSet");
        GameObject skillUI;
        for (int i = 0; i < 4; i++)
        {
            skillUI = Instantiate(skillUiPref, new Vector3(100 + (100 * i), 50, 0), skillCanvas.transform.rotation, skillCanvas.transform) as GameObject;
            skillUI.GetComponentInChildren<Image>().sprite = skillSprites[i];

            skillCoolDownCheck[i] = skillUI.GetComponent<AbilityCoolDown>();
            skillCoolDownCheck[i].abilityButtonAxisName = skillKeyMaps[i];
            skillCoolDownCheck[i].coolDownDuration = skillCoolDowns[i];
        }
    }
    void Update()
    {
        if (pv.isMine)
        {
            //___________This is for running anim and this has to change with raycast system__________
            //if (GetComponent<Rigidbody2D>().velocity.x != 0f) 

            //This is temporary this we will use this until facing to enemy
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                anim.SetInteger("State", 3);
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                anim.SetInteger("State", 0);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                anim.SetInteger("State", 3);
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                anim.SetInteger("State", 0);
            }
            //________________________________________________________________________________________
            if (Input.GetButtonDown("SkillW") && skillCoolDownCheck[1].itsReady)
            {
                anim.SetInteger("State", 2);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetInteger("State", 1);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (pc.canJump)
                {
                    pc.canJump = false;
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
                    canDoubleJump = true;
                }
                else
                {
                    if (canDoubleJump)
                    {
                        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
                        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
                        canDoubleJump = false;
                    }
                }
            }
        }
    }

    void AttackTrigger()
    {
        pv.RPC("BasicAttack", PhotonTargets.All);
    }

    void ChangeToIdle()
    {
        anim.SetInteger("State", 0);
    }

    [PunRPC]
    void BasicAttack()
    {
        Instantiate(shuriken, shurikenSpawnPoint.transform.position, Quaternion.identity);
    }
}
