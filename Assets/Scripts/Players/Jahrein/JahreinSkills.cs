using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JahreinSkills : Photon.PunBehaviour
{
    
    Animator anim;
    public GameObject vidanjor;
    public Sprite[] skillSprites;
    public GameObject skillUiPref;
    public float damage = 4f;

    PlayerController _playerController;
    PhotonView _photonView;
    Controller2D _controller;
    Player _player;
    bool jahAtt = false;
    string[] skillKeyMaps = { "SkillQ", "SkillW", "SkillE", "SkillR" };
    float[] skillCoolDowns = { 4, 7, 10, 25 };
    //Abilities q, w, e, r;

    Controller2D _controller;

    private void Awake()
    {
        if (photonView.isMine)
        {
            /*q.skillKey = "SkillQ";
            q.coolDown = 4f;
            q.damage = 10;

            w.skillKey = "SkillW";
            w.coolDown = 4f;
            w.damage = 10;

            e.skillKey = "SkillE";
            e.coolDown = 4f;
            e.damage = 10;

            r.skillKey = "SkillR";
            r.coolDown = 4f;
            r.damage = 10;*/

            setSkills();
            
        }
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
            //Debug.Log(skillCoolDownCheck[i].abilityButtonAxisName);
        }
    }

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _controller = GetComponent<Controller2D>();
        _player = GetComponent<Player>();
        _photonView = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        anim.SetInteger("State", 0);
        _controller = GetComponent<Controller2D>();
    }

    void Update()
    {
        if (photonView.isMine)
        {
            //This is for running anim and this has to change with raycast system
            //if (GetComponent<Rigidbody2D>().velocity.x != 0f) 

            //This is temporary this we will use this until facing to enemy
            if (_controller.canMove)
            {
                if (Input.GetKey(KeyCode.RightArrow) && _playerController.canJump)
                {
                    anim.Play("Running");
                }
                if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    anim.Play("Idle");
                }

                if (Input.GetKey(KeyCode.LeftArrow) && _playerController.canJump)
                {
                    anim.Play("Running");
                }
                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    anim.Play("Idle");
                }
            }

            if (Input.GetButtonDown("SkillQ") && skillCoolDownCheck[0].itsReady)
            {
                anim.Play("jahRage");
            }
            if (Input.GetButtonDown("SkillW") && skillCoolDownCheck[1].itsReady)
            {
                _controller.canMove = false;
                anim.Play("Kutsama");
                
            }
            if (Input.GetButtonDown("SkillE") && skillCoolDownCheck[2].itsReady)
            {
                anim.Play("PipiSuyu");
                _photonView.RPC("PipiSuyu", PhotonTargets.All);
            }
            if (Input.GetButtonDown("SkillR") && skillCoolDownCheck[3].itsReady)
            {
                _photonView.RPC("jahUlti", PhotonTargets.All);
            }
            if (Input.GetButtonDown("Attack"))
            {
                anim.Play("BasicAttack");
                _photonView.RPC("basicAttack", PhotonTargets.All);
            }
            else
            {
                jahAtt = false;
            }
        }
    }

    [PunRPC]
    private void basicAttack()
    {
        jahAtt = true;
    }

    [PunRPC]
    void jahRageSkill()
    {
        //GetComponent<Rigidbody2D>().AddForce(new Vector2(6, 0), ForceMode2D.Impulse);
        _player.velocity.x = 80f;
        damage = damage + (damage * 0.25f);
    }

    [PunRPC]
    void PipiSuyu()
    {
        //Trow particle
    }

    [PunRPC]
    void jahUlti()
    {
        Instantiate(vidanjor, this.transform.position, this.transform.rotation);
    }

    //This is used for animation event
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !photonView.isMine && jahAtt)
        {
            Debug.Log("Take dmg");
            collision.GetComponent<PlayerController>().takeHit(damage);
        }
    }
}
