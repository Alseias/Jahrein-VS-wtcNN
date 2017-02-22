using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wtcnnSkills : Photon.PunBehaviour
{
    public GameObject shurikenSpawnPoint, shuriken, bulletSpawnPoint, bullet, skillUiPref;
    public Sprite[] skillSprites;
    public AudioClip[] skillSounds;
    
    Animator anim = new Animator();
    Player _player;
    PhotonView pv;
    bool canJump, isGrounded;
    string[] skillKeyMaps = { "SkillQ", "SkillW", "SkillE", "SkillR" };
    float[] skillCoolDowns = { 4, 4, 7, 25 };
    AbilityCoolDown[] skillACD = new AbilityCoolDown[4];


    void Start()
    {
        _player = GetComponent<Player>();
        pv = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();

        isGrounded = false;
        canJump = true;

        if (photonView.isMine)
            setSkills();

    }

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
            if (true)//can move
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

            if (Input.GetKeyDown(KeyCode.Space)) //Basic Attack
            {
                anim.Play("BasicAttack");
                anim.SetInteger("State",1);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow)) // This will be used for DoubleJump
            {
                anim.Play("wtcnJump");
                anim.SetInteger("State", 7);
                if (!isGrounded && canJump)
                {
                    _player.velocity.y = 20f;
                    anim.Play("wtcnJump");
                    anim.SetInteger("State", 7);
                    canJump = false;
                }
            }
        }
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    //These are used for animation event

    void AttackTrigger()
    {
        pv.RPC("BasicAttack", PhotonTargets.All);
    }

    void ChangeToIdle()
    {
        anim.SetInteger("State", 0);
    }

    void WtcnGasmTrigger()
    {
        _player.velocity.x = -150f;
        AudioSource.PlayClipAtPoint(skillSounds[0], transform.position, 2f);
    }

    void ChangeVelocity()
    {
        _player.velocity.x = 0;
    }

    [PunRPC]
    void AwpTrigger()
    {
        Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity);
    }

    [PunRPC]
    void BasicAttack()
    {
        Instantiate(shuriken, shurikenSpawnPoint.transform.position, Quaternion.identity);
    }
}
