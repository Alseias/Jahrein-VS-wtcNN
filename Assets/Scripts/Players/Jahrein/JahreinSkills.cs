using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JahreinSkills : Photon.PunBehaviour {

    PlayerController _playerController;
	// Use this for initialization
	void Start () {
        _playerController = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if(photonView.isMine) {
            if(Input.GetKeyDown(KeyCode.Q)) {
                _playerController.canMove = false;
                Invoke("jahRageSkill", 0);
                
            }
            if(Input.GetKeyDown(KeyCode.Space)) {
                basicAttack();
            }
        }
		
	}

    private void basicAttack() {
        //asdasd
    }

    [PunRPC]
    void jahRageSkill() {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(4, 0), ForceMode2D.Impulse);
        _playerController.canMove = true;

    }
    [PunRPC]
    void jahRageSkill3() {


    }
    void jahRage2()
    {

    }
}
