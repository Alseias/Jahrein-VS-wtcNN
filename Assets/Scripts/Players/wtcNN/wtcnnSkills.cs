using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Controller2D))]
public class wtcnnSkills : Photon.PunBehaviour {
    public GameObject shurikenSpawnPoint, shuriken, bulletSpawnPoint, bullet;
    public Transform rayStart, rayEnd;

    [Header("Skill Settings")]
    public GameObject skillUiPref;
    public Sprite[] skillSprites;
    public AudioClip[] skillSounds;
    public float[] skillCoolDowns = { 4, 4, 7, 25 };
    public float[] skillDurations = { 1, 1, 1, 1 };

    //PRIVATE SKILL SETTINGS!!
    string[] skillKeyMaps = { "SkillQ", "SkillW", "SkillE", "SkillR" };
    AbilityCoolDown[] skillACD = new AbilityCoolDown[4];
    int usedSkill;



    Animator anim = new Animator();
    Player _player;
    PhotonView pv;
    Controller2D controller;
    bool canJump, isGrounded, OnTrigger, _wtcnGasm;


    float distance;


    void Start() {
        _player = GetComponent<Player>();
        pv = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();

        isGrounded = false;
        OnTrigger = false;
        canJump = true;
        _wtcnGasm = false;
        _player.velocity.y = 0;

        if(photonView.isMine)
            setSkills();

    }

    void setSkills() {
        GameObject skillCanvas = GameObject.Find("SkillSet");
        GameObject skillUI;
        for(int i = 0; i < 4; i++) {
            skillUI = Instantiate(skillUiPref, Vector3.zero, skillCanvas.transform.rotation, skillCanvas.transform) as GameObject;
            skillUI.GetComponentInChildren<Image>().sprite = skillSprites[i];

            skillACD[i] = skillUI.GetComponent<AbilityCoolDown>();
            skillACD[i].abilityButtonAxisName = skillKeyMaps[i];
            skillACD[i].coolDownDuration = skillCoolDowns[i];
            skillACD[i].durationTime = skillDurations[i];

        }
    }
    
    void Update()
    {
             

        if(pv.isMine && _player.gamestart)
        {
            //Raycasting();



            /*if(OnTrigger && !_wtcnGasm)
            {
                if(Raycasting()) {
                    _player.target.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, 10f);
                    //_wtcnGasm = false;

                    Debug.Log("Damage given");
                }
            }*/

            if(GetComponent<Stats>().isAlive) {
                if(_player.canMove) {
                    if(Input.GetKey(KeyCode.LeftArrow)) {
                        //pv.RPC("AnimTrigger", PhotonTargets.All, "wtcnRunning");
                        anim.SetInteger("State", 4);
                    }
                    if(Input.GetKeyUp(KeyCode.RightArrow)) {
                        //pv.RPC("AnimTrigger", PhotonTargets.All, "wtcnIdle");
                        anim.SetInteger("State", 0);
                    }
                    if(Input.GetKey(KeyCode.RightArrow)) {
                        //pv.RPC("AnimTrigger", PhotonTargets.All, "wtcnWalking");
                        anim.SetInteger("State", 3);
                    }
                    if(Input.GetKeyUp(KeyCode.LeftArrow)) {
                       //pv.RPC("AnimTrigger", PhotonTargets.All, "wtcnIdle");
                        anim.SetInteger("State", 0);
                    }
                }

                if(Input.GetButtonDown("Attack")) //Basic Attack
                {
                    
                    pv.RPC("AnimTrigger", PhotonTargets.All, "BasicAttack");
                }

                if(Input.GetKeyDown(KeyCode.UpArrow)) // This will be used for DoubleJump
                {
                    pv.RPC("AnimTrigger", PhotonTargets.All, "wtcnJump");
                    anim.SetInteger("State", 7);
                    if(!isGrounded && canJump) {
                        pv.RPC("AnimTrigger", PhotonTargets.All, "wtcnJump 0");
                        _player.velocity.y = 20f;
                        anim.SetInteger("State", 7);
                        canJump = false;
                    }
                }

                if(_player.canUseSkill) {
                    if(Input.GetButtonDown("SkillQ") && skillACD[0].itsReady) {
                        
                        _player.canUseSkill = false;
                        usedSkill = 0;
                        skillACD[0].use();
                        pv.RPC("AnimTrigger", PhotonTargets.All, "wtcnGasm");
                        _wtcnGasm = true;
                        Debug.Log("_wtcnGasm" + _wtcnGasm);

                    }

                    if(Input.GetButtonDown("SkillW") && skillACD[1].itsReady) {
                        
                        _player.canUseSkill = false;
                        distance = Vector2.Distance(_player.target.transform.position, transform.position);
                        usedSkill = 1;
                        skillACD[1].use();
                        pv.RPC("AnimTrigger", PhotonTargets.All, "casper");
                        pv.RPC("playSound", PhotonTargets.All, usedSkill);
                        if(distance < 5f) {
                            Debug.Log("Korkuttum xd");
                            //_player.target.SendMessage("Fear"); //SENDMESSAGE KULLANMA DEMEDİK Mİ!!!!!
                        }
                    }

                    if(Input.GetButtonDown("SkillR") && skillACD[3].itsReady) {

                        _player.canUseSkill = false;
                        usedSkill = 3;
                        skillACD[3].use();
                        pv.RPC("AnimTrigger", PhotonTargets.All, "AWP");
                        pv.RPC("playSound", PhotonTargets.All, usedSkill);
                    }
                } else {
                    //if player cant use skill
                    if(skillACD[usedSkill].durationEnd) {
                        //if duration ends player can use skill again
                        _player.canUseSkill = true;

                    }
                    /*if(skillACD[0].durationEnd) {
                        _wtcnGasm = false;
                        Debug.Log("_wtcnGasm" + _wtcnGasm);

                    }*/
                }
                _player.canMove = skillACD[0].durationEnd;
            }
        }
    }

    private bool Raycasting() {
        Debug.DrawLine(rayStart.position, rayEnd.position, Color.green);
        bool rayHit = Physics2D.Linecast(rayStart.position, rayEnd.position, 1 << LayerMask.NameToLayer("enemy"));
        return rayHit;
    }


    #region Animation events

    void CheckRayCast() {
        OnTrigger = !OnTrigger;
        Debug.Log("ontrigger:" + OnTrigger);
        if(photonView.isMine && OnTrigger) {
            InvokeRepeating("wtcnGasmDamage", 2f, 0.5f);

        }
    }

    void AttackTrigger() {
        if(photonView.isMine) {
            pv.RPC("BasicAttack", PhotonTargets.All);

        }
    }

    void ChangeToIdle() {
        if(photonView.isMine) {

            pv.RPC("AnimTrigger", PhotonTargets.All, "wtcnIdle");
            anim.SetInteger("State", 0);
        }
    }

    void WtcnGasmTrigger() {
        if(photonView.isMine) {

            _player.velocity.x = 150f * (_player.isfacingRight ? 1 : -1);
            pv.RPC("playSound", PhotonTargets.All, usedSkill);
        }
    }

    void wtcnGasmDamage() {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x+5f,transform.position.y), Vector2.right * (_player.isfacingRight ? 1 : -1), 10f, 10);
        Debug.DrawRay(transform.position, Vector2.right * (_player.isfacingRight ? 1 : -1), Color.green);
        Debug.Log(hit.transform.name);


        if(OnTrigger && _wtcnGasm) {
            if(hit) {
                _player.target.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, 10f);
                Debug.Log("Damage given");
                _wtcnGasm = false;
                Debug.Log("gasm:" + _wtcnGasm);

            }
        }
    }

    void ChangeVelocity() {
        _player.velocity.x = 0;
    }
    #endregion

    #region Animation RPC
    [PunRPC]
    void AnimTrigger(string animName)
    {
        anim.Play(animName);
    }

    #endregion
    
    #region PunRPC

    [PunRPC]
    void AwpTrigger() {
        Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity);
    }

    [PunRPC]
    void BasicAttack() {
        shurikenScript objShur = Instantiate(shuriken, shurikenSpawnPoint.transform.position, Quaternion.identity).GetComponent<shurikenScript>();
        /*objShur.SendMessage("dir", _player.isfacingRight, SendMessageOptions.RequireReceiver);
        objShur.SendMessage("id", pv.viewID, SendMessageOptions.RequireReceiver);*/
        objShur.pvID = pv.viewID;
        objShur.fDir = _player.isfacingRight ? 1 : -1;
        
    }

    [PunRPC]
    public void playSound(int skillID) {
        AudioSource.PlayClipAtPoint(skillSounds[skillID], transform.position, 2f);

    }
    #endregion
}