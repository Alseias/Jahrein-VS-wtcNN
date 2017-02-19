using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wtcnnSkills : Photon.PunBehaviour
{
    public GameObject shurikenSpawnPoint;
    public GameObject shuriken;
    public Sprite[] skillSprites;
    public GameObject skillUiPref;
    public AudioClip[] skillSounds;

    Player pc;
    PhotonView pv;
    bool canDoubleJump = false;
    string[] skillKeyMaps = { "SkillQ", "SkillW", "SkillE", "SkillR" };
    float[] skillCoolDowns = { 4, 4, 7, 25 };

    Animator anim = new Animator();

    void Start()
    {
        pc = GetComponent<Player>();
        pv = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();

        if (photonView.isMine)
            setSkills();

    }
    AbilityCoolDown[] skillACD = new AbilityCoolDown[4];
    void setSkills()
    {
        GameObject skillCanvas = GameObject.Find("SkillSet");
        GameObject skillUI;
        for (int i = 0; i < 4; i++)
        {
            skillUI = Instantiate(skillUiPref, new Vector3(100 + (100 * i), 50, 0), skillCanvas.transform.rotation, skillCanvas.transform) as GameObject;
            skillUI.GetComponentInChildren<Image>().sprite = skillSprites[i];

            skillACD[i] = skillUI.GetComponent<AbilityCoolDown>();
            skillACD[i].abilityButtonAxisName = skillKeyMaps[i];
            skillACD[i].coolDownDuration = skillCoolDowns[i];
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

            if (Input.GetButtonDown("SkillQ") && skillACD[0].itsReady)
            {
                skillACD[0].use();
                anim.Play("wtcnGasm");
                anim.SetInteger("State",5);
                AudioSource.PlayClipAtPoint(skillSounds[0], transform.position, 2f);
            }

            if (Input.GetButtonDown("SkillW") && skillACD[1].itsReady)
            {
                skillACD[1].use();
                anim.Play("casper");
                anim.SetInteger("State",2);
                AudioSource.PlayClipAtPoint(skillSounds[1], transform.position, 2f);
            }

            if (Input.GetButtonDown("SkillR") && skillACD[3].itsReady)
            {
                skillACD[3].use();
                anim.Play("AWP");
                anim.SetInteger("State",6);
                AudioSource.PlayClipAtPoint(skillSounds[3], transform.position, 2f);
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
                if (true)//pc.canjump??
                {
                    //
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
