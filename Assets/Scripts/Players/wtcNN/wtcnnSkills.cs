using System.Collections;
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
            if (pc.canMove)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    anim.Play("wtcnRunning");
                    anim.SetInteger("State",3);
                }
                if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    anim.Play("wtcnIdle");
                    anim.SetInteger("State",0);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    anim.Play("wtcnWalking");
                    anim.SetInteger("State",4);
                }
                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    anim.Play("wtcnIdle");
                    anim.SetInteger("State",0);
                }
            }

            if (Input.GetButtonDown("SkillQ") && skillCoolDownCheck[0].itsReady)
            {
                anim.Play("wtcnGasm");
                anim.SetInteger("State",5);
            }

            if (Input.GetButtonDown("SkillW") && skillCoolDownCheck[1].itsReady)
            {
                anim.Play("casper");
                anim.SetInteger("State",2);
            }

            if (Input.GetButtonDown("SkillR") && skillCoolDownCheck[3].itsReady)
            {
                anim.Play("AWP");
                anim.SetInteger("State",6);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.Play("BasicAttack");
                anim.SetInteger("State",1);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                anim.Play("wtcnJump");
                anim.SetInteger("State", 7);
                if (pc.canJump)
                {
                    pc.canJump = false;
                    canDoubleJump = true;
                }
                else
                {
                    if (canDoubleJump)
                    {
                        GetComponent<Player>().velocity.y = 5f;
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
