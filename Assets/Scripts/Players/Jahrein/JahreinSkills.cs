using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JahreinSkills : Photon.PunBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(photonView.isMine) {
            if(Input.GetKeyDown(KeyCode.Q)) {
               jahRageSkill();
            }
        }
		
	}
    [PunRPC]
    void jahRageSkill() {

    }
}
