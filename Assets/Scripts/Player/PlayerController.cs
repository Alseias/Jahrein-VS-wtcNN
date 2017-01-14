using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Photon.PunBehaviour,IPunObservable {

    public float health = 1f;
    public GameObject playerUiPrefab;
    void Start () {
        if(playerUiPrefab == null) {
            Debug.LogError("Missing playerUiPrefab!!");
        }else {
            GameObject _uiGo = Instantiate(playerUiPrefab) as GameObject;
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

    }

    void Update () {
        playerInputs();
        if(!photonView.isMine) {
            this.transform.position = correctPosition;
        }
        
        if(health <= 0) {
            GameManager.Instance.LeaveRoom();
        }
		
	}

    private void playerInputs() {
        if(Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector2.left * Time.deltaTime*10f);
        }
        if(Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector2.right * Time.deltaTime*10f);
        }

        if(Input.GetKey(KeyCode.Space)) {
            if(!photonView.isMine) {
                return;
            }
            health -= 0.1f * Time.deltaTime;
            Debug.Log("Health: " + health);
        }
    }

    Vector2 correctPosition;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting) {
            stream.SendNext(this.transform.position);
            stream.SendNext(health);
        }else {
            correctPosition = (Vector3)stream.ReceiveNext();
            this.health = (float)stream.ReceiveNext();
        }
    }
}
