using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JahreinSkills : Photon.PunBehaviour
{
    
    Animator anim;
    public GameObject vidanjor, pipiSuyu, vidanjorSpawn, pipiSuyuSpawn;
    public Sprite[] skillSprites;
    public GameObject skillUiPref;
    public float damage = 4f;
    public AudioClip[] skillSounds;

    //PlayerController _playerController;
    PhotonView _photonView;
    Controller2D _controller;
    Player _player;

    bool jahAtt = false;
    string[] skillKeyMaps = { "SkillQ", "SkillW", "SkillE", "SkillR" };
    public float[] skillCoolDowns = { 4, 7, 10, 25 };
    public float[] skillDurations = { 1, 1, 1, 1 };
    AbilityCoolDown[] skillACD = new AbilityCoolDown[4];

    int usedSkill;
    //Abilities q, w, e, r;

    private void Awake()
    {

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
            skillACD[i].durationTime = skillDurations[i];
            //Debug.Log(skillCoolDownCheck[i].abilityButtonAxisName);
        }
    }

    void Start()
    {
        //_playerController = GetComponent<PlayerController>();
        _controller = GetComponent<Controller2D>();
        _player = GetComponent<Player>();
        _photonView = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        anim.SetInteger("State", 0);
        if(photonView.isMine) {

            setSkills();

        }
    }

    void Fear()
    {
        _controller.canMove = false;
        _player.canUseSkill = false;
        StartCoroutine("FearCounter");
    }

    IEnumerator FearCounter()
    {
        yield return new WaitForSeconds(3);
        _controller.canMove = true;
        _player.canUseSkill = true;
    }

    void Update()
    {
        if (photonView.isMine)
        {
            if (GetComponent<Stats>().isAlive)
            {
                if (_controller.canMove)
                {
                    if (Input.GetKey(KeyCode.RightArrow))//set can jump!!!!!
                    {
                        anim.Play("jahreinRunning");
                        anim.SetInteger("State", 4);
                    }
                    if (Input.GetKeyUp(KeyCode.RightArrow))
                    {
                        anim.Play("jahIdle");
                        anim.SetInteger("State", 0);
                    }

                    if (Input.GetKey(KeyCode.LeftArrow))//set can jump!!!!!
                    {
                        anim.Play("jahreinRunning");
                        anim.SetInteger("State", 4);
                    }
                    if (Input.GetKeyUp(KeyCode.LeftArrow))
                    {
                        anim.Play("jahIdle");
                        anim.SetInteger("State", 0);
                    }
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        anim.Play("jahJump");
                        anim.SetInteger("State", 5);
                    }

                }

                if (Input.GetButtonDown("Attack")) // Basic Attack
                {
                    anim.Play("BasicAttackv2");
                    anim.SetInteger("State", 7);
                    _photonView.RPC("basicAttack", PhotonTargets.All);
                }

                else
                {
                    jahAtt = false;
                }


                if (_player.canUseSkill)
                {

                    if (Input.GetButtonDown("SkillQ") && skillACD[0].itsReady && _controller.collisions.below)
                    {
                        _player.canUseSkill = false;
                        skillACD[0].use();
                        usedSkill = 0;
                        _player.canMove = false;
                        anim.Play("jahRagev2");
                        anim.SetInteger("State", 1);
                        //AudioSource.PlayClipAtPoint(skillSounds[0], transform.position, 2f);
                        _photonView.RPC("playSound", PhotonTargets.All, usedSkill);
                    }

                    if (Input.GetButtonDown("SkillW") && skillACD[1].itsReady)
                    {
                        _player.canUseSkill = false;
                        skillACD[1].use();
                        usedSkill = 1;

                        _controller.canMove = false;
                        anim.Play("kutsamav2");
                        anim.SetInteger("State", 2);
                        // AudioSource.PlayClipAtPoint(skillSounds[1], transform.position, 2f);
                        _photonView.RPC("playSound", PhotonTargets.All, usedSkill);
                    }

                    if (Input.GetButtonDown("SkillE") && skillACD[2].itsReady)
                    {

                        _player.canUseSkill = false;
                        skillACD[2].use();
                        usedSkill = 2;

                        anim.Play("PipiSuyu");
                        anim.SetInteger("State", 3);
                        _photonView.RPC("PipiSuyu", PhotonTargets.All);
                        //AudioSource.PlayClipAtPoint(skillSounds[2], transform.position, 2f);
                        _photonView.RPC("playSound", PhotonTargets.All, usedSkill);

                    }

                    if (Input.GetButtonDown("SkillR") && skillACD[3].itsReady)
                    {
                        _player.canUseSkill = false;
                        skillACD[3].use();
                        usedSkill = 3;

                        _photonView.RPC("jahUlti", PhotonTargets.All);
                        _photonView.RPC("playSound", PhotonTargets.All, usedSkill);

                    }
                }

                else
                {
                    //if player cant use skill
                    if (skillACD[usedSkill].durationEnd)
                    {
                        //if duration ends player can use skill again
                        _player.canUseSkill = true;
                    }
                }
                //if game started 
                _player.canMove = skillACD[0].durationEnd;
            }
        }
    }

    #region PunRPC's
    [PunRPC]
    private void basicAttack()
    {
        jahAtt = true;
    }

    [PunRPC]
    void jahRageSkill()
    {
        //GetComponent<Rigidbody2D>().AddForce(new Vector2(6, 0), ForceMode2D.Impulse);
        _player.velocity = Vector2.zero;
        _player.velocity.x = 150f * (_player.isfacingRight ? 1 : -1);
        damage = damage + (damage * 0.25f);
    }

    [PunRPC]
    void PipiSuyu()
    {
        
        Instantiate(pipiSuyu, new Vector3(pipiSuyuSpawn.transform.position.x, pipiSuyuSpawn.transform.position.y, 0), this.transform.rotation);

    }

    [PunRPC]
    void jahUlti()
    {
        Instantiate(vidanjor, new Vector3(-18f,-2f,0f), this.transform.rotation);
        //this.transform.position.x, this.transform.position.y/2,0
    }
    [PunRPC]
    public void playSound(int skillID) {
        AudioSource.PlayClipAtPoint(skillSounds[skillID], transform.position, 2f);

    }

    #endregion


    #region Animation events
    void JahRageTrigger()
    {
        _photonView.RPC("jahRageSkill", PhotonTargets.All);
    }

    void ChangeToIdle()
    {
        anim.SetInteger("State", 0);
    }

    void CanMove()
    {
        _controller.canMove = true;
    }

    void ChangeVelocity()
    {
        _player.velocity.x = 0;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !photonView.isMine && jahAtt)
        {
            Debug.Log("Take dmg");
            collision.GetComponent<PlayerController>().takeHit(damage);
        }
    }
}
