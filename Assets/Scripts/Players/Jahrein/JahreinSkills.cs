using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JahreinSkills : Photon.PunBehaviour {

    public GameObject vidanjor;

    public float damage = 4f,speed=1f;
    [HideInInspector]
    public bool kutsama;

    PlayerController _playerController;
    PhotonView _photonView;
    bool jahAtt = false;
    float jahrageDamage, jahoHealth;
    

    private void Awake() {
        
            
    }

     
    void Start () {
        _playerController = GetComponent<PlayerController>();
        _photonView = GetComponent<PhotonView>();


    }
	
	void Update () {
        if(photonView.isMine) {

            //send info to playerController
            _playerController.damage = damage;
            _playerController.speed = speed;


            if(Input.GetButtonDown("SkillQ")&& _playerController.skillCoolDownCheck[0].itsReady) {
                //_playerController.canMove = false;
                jahrageDamage = damage;
                _photonView.RPC("jahRageSkill",PhotonTargets.All);
                
            }


            if(Input.GetButtonDown("SkillE")&& _playerController.skillCoolDownCheck[2].itsReady) {
                jahoHealth = _playerController.health;
                _photonView.RPC("jahKutsama", PhotonTargets.All);


            }

            if(Input.GetButtonDown("SkillR")&& _playerController.skillCoolDownCheck[3].itsReady) {
                _photonView.RPC("jahUlti", PhotonTargets.All);
            }

            if(Input.GetButtonDown("Attack")) {
                _photonView.RPC("basicAttack",PhotonTargets.All);
            }

            //jahRage finished
            if(_playerController.skillCoolDownCheck[0].durationEnd&&!_playerController.skillCoolDownCheck[0].itsReady) {
                //Debug.Log("Jahrage bitti");
                damage = jahrageDamage;
            }
            //------------

            //kutsama finished
            if(_playerController.skillCoolDownCheck[2].durationEnd&&kutsama) {

                _playerController.canMove = true;
                    
                kutsama = false;
                
            } else {
                if(kutsama && damage<6.9) {
                    float takenDmgPercentage = (jahoHealth - _playerController.health) * 15 / 100;
                    damage += takenDmgPercentage;
                    //Debug.Log(damage);
                    _playerController.health = jahoHealth;
                }
            }
            //------------

        }
		
	}

    [PunRPC]
    private void basicAttack() {
        jahAtt=true;
    }

    [PunRPC]
    void jahRageSkill() {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(4, 0), ForceMode2D.Impulse);
        damage += (damage * 0.25f);

    }

    void jahPipiSuyu() {

    }

    [PunRPC]
    void jahKutsama() {
        kutsama = true;
        _playerController.canMove = false;
    }

    [PunRPC]
    void jahUlti() {
        Instantiate(vidanjor, this.transform.position, this.transform.rotation);
    }

    void jahPasif() {
        _playerController.skillCoolDownCheck[4].passiveTriggered = true;

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        if(photonView.isMine && _playerController.skillCoolDownCheck[4].itsReady && kutsama && collision.tag== "shuriken" || collision.tag == "jahBalta") {
            jahPasif();
        }
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.tag == "Player" && jahAtt) {
            Debug.Log("Take dmg");
            Debug.Log(damage);
            collision.GetComponent<PhotonView>().RPC("takeHit", PhotonTargets.All, new object[] { damage });
            jahAtt = false;

        }
    }

}
