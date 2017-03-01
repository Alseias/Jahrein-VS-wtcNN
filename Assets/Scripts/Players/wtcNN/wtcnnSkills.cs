using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof( Controller2D))]
public class wtcnnSkills : Photon.PunBehaviour
{
    public GameObject shurikenSpawnPoint, shuriken, bulletSpawnPoint, bullet, skillUiPref;
    public Sprite[] skillSprites;
    public AudioClip[] skillSounds;
    public Transform rayStart, rayEnd;

    Animator anim = new Animator();
    Player _player;
    PhotonView pv;
    Controller2D controller;
    bool canJump, isGrounded;
    string[] skillKeyMaps = { "SkillQ", "SkillW", "SkillE", "SkillR" };
    float[] skillCoolDowns = { 4, 4, 7, 25 };
    AbilityCoolDown[] skillACD = new AbilityCoolDown[4];
    int usedSkill;


    void Start()
    {
        _player = GetComponent<Player>();
        pv = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        //controller.GetComponent<Controller2D>();

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

    private void FixedUpdate() {
        if(pv.isMine) {
            if(true){
                if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                    anim.Play("wtcnRunning");
                    anim.SetInteger("State", 3);
                }
                if(Input.GetKeyUp(KeyCode.RightArrow)) {
                    anim.Play("wtcnIdle");
                    anim.SetInteger("State", 0);
                }
                if(Input.GetKeyDown(KeyCode.RightArrow)) {
                    anim.Play("wtcnWalking");
                    anim.SetInteger("State", 4);
                }
                if(Input.GetKeyUp(KeyCode.LeftArrow)) {
                    anim.Play("wtcnIdle");
                    anim.SetInteger("State", 0);
                }
            }
        }
    }

    void Update()
    {
        if (pv.isMine)
        {
            Raycasting();


            

            if(Input.GetButtonDown("SkillQ") && skillACD[0].itsReady)
            {

                usedSkill = 0;
                skillACD[0].use();
                anim.Play("wtcnGasm");
                anim.SetInteger("State",5);
            }

            if (Input.GetButtonDown("SkillW") && skillACD[1].itsReady)
            {
                usedSkill = 1;
                skillACD[1].use();
                anim.Play("casper");
                anim.SetInteger("State",2);
                pv.RPC("playSound", PhotonTargets.All, usedSkill);
            }

            if (Input.GetButtonDown("SkillR") && skillACD[3].itsReady)
            {
                usedSkill = 3;
                skillACD[3].use();
                anim.Play("AWP");
                anim.SetInteger("State",6);
                pv.RPC("playSound", PhotonTargets.All, usedSkill);

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

    public float SendDirection()
    {
        float direction = _player.target.transform.position.x - transform.position.x;
        return direction;
    }

    private void Raycasting()
    {
        Debug.DrawLine(rayStart.position, rayEnd.position, Color.green);
        bool rayHit = Physics2D.Linecast(rayStart.position, rayEnd.position, 1<<LayerMask.NameToLayer("enemy"));
       
    }




    #region collision
    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("collision enter w ground obj");
            isGrounded = true;
            canJump = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision) {
        Debug.Log(collision.tag);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
    #endregion
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
        pv.RPC("playSound", PhotonTargets.All, usedSkill);

    }

    void ChangeVelocity()
    {
        _player.velocity.x = 0;
    }

    #region PunRPC's

    [PunRPC]
    void AwpTrigger()
    {
        Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity);
    }

    [PunRPC]
    void BasicAttack() {
        shurikenScript objShur = PhotonNetwork.Instantiate("shuriken", shurikenSpawnPoint.transform.position, Quaternion.identity,0).GetComponent<shurikenScript>();
        /*objShur.SendMessage("dir", _player.isfacingRight, SendMessageOptions.RequireReceiver);
        objShur.SendMessage("id", pv.viewID, SendMessageOptions.RequireReceiver);*/
        objShur.pvID = pv.viewID;
        if(_player.isfacingRight)
            objShur.fDir = 1;
        else
            objShur.fDir = -1;
    }

    [PunRPC]
    public void playSound(int skillID) {
       AudioSource.PlayClipAtPoint(skillSounds[skillID], transform.position, 2f);
    
    }

    #endregion
}
