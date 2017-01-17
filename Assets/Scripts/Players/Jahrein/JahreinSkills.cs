using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JahreinSkills : Photon.PunBehaviour {

    public GameObject vidanjor;
    public Sprite[] skillSprites;
    public GameObject skillUiPref;
    public float damage = 4f;

    PlayerController _playerController;
    PhotonView _photonView;
    bool jahAtt = false;

    // Use this for initialization
    private void Awake() {
        if(photonView.isMine)
        setSkills();
    }
    void Start () {
        _playerController = GetComponent<PlayerController>();
        _photonView = GetComponent<PhotonView>();
        
    }
	
	// Update is called once per frame
	void Update () {
        if(photonView.isMine) {
            if(Input.GetKeyDown(KeyCode.Q)) {
                _playerController.canMove = false;
                Invoke("jahRageSkill", 0);
                
            }
            if(Input.GetKeyDown(KeyCode.R)) {
                _photonView.RPC("jahUlti", PhotonTargets.All);
            }
            if(Input.GetKeyDown(KeyCode.Space)) {
                _photonView.RPC("basicAttack",PhotonTargets.All);
            } else {
                jahAtt = false;
            }
        }
		
	}
    void setSkills() {
        GameObject skillCanvas = GameObject.Find("SkillSet");
        GameObject skillUI;
        for(int i = 0; i < 4; i++) {
            skillUI = Instantiate(skillUiPref, new Vector3(100 + (100 * i), 50, 0), skillCanvas.transform.rotation, skillCanvas.transform);
            skillUI.GetComponent<Image>().sprite = skillSprites[i];
        }
    }
    [PunRPC]
    private void basicAttack() {
        jahAtt=true;
    }

    [PunRPC]
    void jahRageSkill() {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(4, 0), ForceMode2D.Impulse);
        damage = damage + (damage * 0.25f);

    }

    [PunRPC]
    void jahUlti() {
        Instantiate(vidanjor, this.transform.position, this.transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player" && !photonView.isMine && jahAtt) {
            Debug.Log("Take dmg");
            collision.GetComponent<PlayerController>().takeHit(damage);

        }
    }

}
