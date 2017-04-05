using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JahreinSkills : Photon.PunBehaviour
{
    
    Animator anim;
    public GameObject vidanjor, pipiSuyu, vidanjorSpawn, pipiSuyuSpawn;
    public bool kutsaBeni;
    public Transform rayStart, rayEnd;

    [Header("Skill Settings")]
    public GameObject skillUiPref;
    public Sprite[] skillSprites;
    public AudioClip[] skillSounds;
    public float[] skillCoolDowns = { 4, 7, 10, 25 };
    public float[] skillDurations = { 1, 1, 1, 1 };

    //PRIVATE SKILL SETTINGS!!
    string[] skillKeyMaps = { "SkillQ", "SkillW", "SkillE", "SkillR" };
    AbilityCoolDown[] skillACD = new AbilityCoolDown[4];
    int usedSkill;

    //PlayerController _playerController;
    PhotonView _photonView;
    Controller2D _controller;
    Player _player;

    bool jahAtt = false;
    bool OnTrigger = true;
    bool basicAttackCheck;
    float mikailHealth;
    


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
            skillUI = Instantiate(skillUiPref, Vector3.zero, skillCanvas.transform.rotation, skillCanvas.transform) as GameObject;
            skillUI.GetComponentInChildren<Image>().sprite = skillSprites[i];

            skillACD[i] = skillUI.GetComponent<AbilityCoolDown>();
            skillACD[i].abilityButtonAxisName = skillKeyMaps[i];
            skillACD[i].coolDownDuration = skillCoolDowns[i];
            skillACD[i].durationTime = skillDurations[i];
        }
    }

    void Start()
    {
        _controller = GetComponent<Controller2D>();
        _player = GetComponent<Player>();
        _photonView = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        anim.SetInteger("State", 0);
        if(photonView.isMine) {

            setSkills();

        }
    }

    [PunRPC]
    public void Fear()
    {
        Debug.Log("Korktum :("); 
        _controller.enabled = false; 
        _player.enabled = false;
        StartCoroutine("FearCounter");
    }

    IEnumerator FearCounter()
    {
        yield return new WaitForSeconds(3); //Burayada giriyor
        _controller.enabled = true;
        _player.enabled = true;
    }

    void Update()
    {
        if (photonView.isMine&&_player.gamestart)
        {
            
            if (GetComponent<Stats>().isAlive)
            {
                #region animationInput
                if(_controller.canMove)
                {
                    if (Input.GetKey(KeyCode.RightArrow))//set can jump!!!!!
                    {
                        anim.SetInteger("State", 4);
                        //anim.Play("jahreinRunning");
                        //_photonView.RPC("AnimTrigger", PhotonTargets.All, "jahreinRunning");
                    }
                    if (Input.GetKeyUp(KeyCode.RightArrow))
                    {
                        anim.SetInteger("State", 0);
                        //anim.Play("jahIdle");
                        //_photonView.RPC("AnimTrigger", PhotonTargets.All, "jahIdle");
                    }

                    if (Input.GetKey(KeyCode.LeftArrow))//set can jump!!!!!
                    {
                        anim.SetInteger("State", 4);
                        //anim.Play("jahreinRunning");
                        //_photonView.RPC("AnimTrigger", PhotonTargets.All, "jahreinRunning");
                    }
                    if (Input.GetKeyUp(KeyCode.LeftArrow))
                    {
                        anim.SetInteger("State", 0);
                        //anim.Play("jahIdle");
                        //_photonView.RPC("AnimTrigger", PhotonTargets.All, "jahIdle");
                    }
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        anim.SetInteger("State", 5);
                        //anim.Play("jahJump");
                        //_photonView.RPC("AnimTrigger", PhotonTargets.All, "jahJump");
                    }

                }
                #endregion

                if(Input.GetButtonDown("Attack")) // Basic Attack
                {
                    //anim.SetInteger("State", 7);
                    basicAttackCheck = true;
                    _photonView.RPC("AnimTrigger", PhotonTargets.All, "BasicAttackv2");
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
                        //anim.SetInteger("State", 1);
                        _photonView.RPC("AnimTrigger", PhotonTargets.All, "jahRagev2");
                        _photonView.RPC("playSound", PhotonTargets.All, usedSkill);
                    }

                    if (Input.GetButtonDown("SkillW") && skillACD[1].itsReady)
                    {
                        _player.canUseSkill = false;
                        skillACD[1].use();
                        usedSkill = 1;

                        _controller.canMove = false;
                        kutsaBeni = true;
                        mikailHealth = _player.health;

                        anim.Play("kutsamav2");
                        //anim.SetInteger("State", 2);
                        _photonView.RPC("AnimTrigger", PhotonTargets.All, "kutsamav2");
                        _photonView.RPC("playSound", PhotonTargets.All, usedSkill);
                    }

                    if (Input.GetButtonDown("SkillE") && skillACD[2].itsReady)
                    {

                        _player.canUseSkill = false;
                        skillACD[2].use();
                        usedSkill = 2;

                        anim.Play("PipiSuyu");
                        //anim.SetInteger("State", 3);
                        _photonView.RPC("PipiSuyu", PhotonTargets.All);
                        _photonView.RPC("AnimTrigger", PhotonTargets.All, "PipiSuyu");
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
        basicAttackCheck = true;
    }

    [PunRPC]
    void jahRageSkill()
    {
        _player.velocity = Vector2.zero;
        _player.velocity.x = 22f * (_player.isfacingRight ? 1 : -1);
        
    }

    [PunRPC]
    void PipiSuyu()
    {
        JahPipiSuyu objPipiSuyu = Instantiate(pipiSuyu, new Vector3(pipiSuyuSpawn.transform.position.x, pipiSuyuSpawn.transform.position.y, 0), this.transform.rotation).GetComponent<JahPipiSuyu>();
        objPipiSuyu.pvID = _photonView.viewID;
        objPipiSuyu.fDir = _player.isfacingRight ? 1 : -1;
    }

    [PunRPC]
    void jahUlti()
    {
        Instantiate(vidanjor, new Vector3(-18f,-2f,0f), this.transform.rotation);
    }

    [PunRPC]
    public void playSound(int skillID)
    {
        AudioSource.PlayClipAtPoint(skillSounds[skillID], transform.position, 2f);

    }

    #endregion

    [PunRPC]
    void AnimTrigger(string animName)
    {
        anim.Play(animName);
    }

   /* [PunRPC]
    void giveDamage() {
        _player.target.GetComponent<Stats>().TakeDamage(_player.damage);
    }*/

    #region Animation events

    private bool Raycasting()
    {
        Debug.DrawLine(rayStart.position, rayEnd.position, Color.green);
        bool rayHit = Physics2D.Linecast(rayStart.position, rayEnd.position, 1 << LayerMask.NameToLayer("enemy"));
        Debug.Log("raycasting:" + rayHit);
        return rayHit;
    }

    void CheckRayCast(int trigger) {
        OnTrigger = trigger==1?true:false;
        if(photonView.isMine&&OnTrigger) {
            Debug.Log("Invoke repeat");
            InvokeRepeating("basicAttTrigger", 0f, 0.01f);
        }else {
            Debug.Log("Cancel invoke");
            CancelInvoke("basicAttTrigger");
        }
    }

    void basicAttTrigger()
    {
        
        if(OnTrigger&&basicAttackCheck)
        {
            if(Raycasting()) {

                _player.target.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, _player.damage);
                basicAttackCheck = false;

            }

        }
    }

    void JahRageTrigger()
    {
        _photonView.RPC("jahRageSkill", PhotonTargets.All);
    }

    void ChangeToIdle()
    {
        _photonView.RPC("AnimTrigger", PhotonTargets.All, "jahIdle");
        anim.SetInteger("State", 0);
    }

    void CanMove()
    {
        _controller.canMove = true;
        kutsaBeni = false;
    }
    void kutsamaFinish() {
        float givenDmg = mikailHealth - _player.health;
        _player.health = mikailHealth;
        if(_player.damage < 7f) {
            _player.damage += (givenDmg * 12) / 100;

        }
        photonView.RPC("OnChangeHealth", PhotonTargets.All, mikailHealth/100);
        //photonView.RPC("TakeDamage", PhotonTargets.All, 0);
        Debug.Log(givenDmg+" new dmg: "+ _player.damage);

    }
    void ChangeVelocity()
    {
        _player.velocity.x = 0;
    }
    #endregion

}
