using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JahreinSkills : Photon.PunBehaviour {

    public GameObject vidanjor;

    PlayerController _playerController;
    PhotonView _photonView;
    bool jahAtt = false;
    // Use this for initialization
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
    [PunRPC]
    private void basicAttack() {
        jahAtt=true;
    }

    [PunRPC]
    void jahRageSkill() {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(4, 0), ForceMode2D.Impulse);
        _playerController.canMove = true;

    }

    [PunRPC]
    void jahUlti() {
        Instantiate(vidanjor, this.transform.position, this.transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
       Debug.Log(collision.tag);
        if(collision.tag == "Player" && collision.GetComponent<PhotonView>().photonView.isMine == true && jahAtt) {
            Debug.Log("Take dmg");
            collision.GetComponent<PlayerController>().takeHit(.05f);

        }
    }

}
