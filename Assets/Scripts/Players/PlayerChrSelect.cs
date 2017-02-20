﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChrSelect : MonoBehaviour {

    public bool imready;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if(stream.isWriting) {
            // We own this player: send the others our data
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.parent.name);
            stream.SendNext(GetComponent<RectTransform>().localScale);
            stream.SendNext(imready);
        } else {
            // Network player, receive data
            this.transform.localPosition = (Vector3)stream.ReceiveNext();
            this.transform.SetParent ( GameObject.Find((string)stream.ReceiveNext()).transform);
            this.GetComponent<RectTransform>().localScale = (Vector3)stream.ReceiveNext();
            imready = (bool)stream.ReceiveNext();
        }
    }
}
