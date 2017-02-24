﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedHero : Photon.PunBehaviour {
    GameObject p1, p2;
    GameObject[] characters;
    int playerID;
    // Use this for initialization
    void Start () {

        /*(int i = 0; i < parentChr.transform.childCount; i++) {

        }*/
        playerID = PlayerPrefs.GetInt("player");
        switch(playerID) {
            case 1:
                p1 = PhotonNetwork.Instantiate("PlayerOne", new Vector3(-500, 0, 0), Quaternion.identity, 0);
                p1.transform.SetParent(this.transform);
                break;
            case 2:
                p2=PhotonNetwork.Instantiate("PlayerTwo", new Vector3(-500, 0, 0), Quaternion.identity, 0);
                p2.transform.SetParent(this.transform);
                break;


            default:
                break;
        }
    }

    // Update is called once per frame
    GameObject[] players;
    void Update() {
        
        if(PhotonNetwork.room.PlayerCount==2) {

            players = GameObject.FindGameObjectsWithTag("Player");
            if(players[0].GetComponent<PlayerChrSelect>().imready && players[1].GetComponent<PlayerChrSelect>().imready) {
                //PhotonNetwork.DestroyAll();
                PhotonNetwork.automaticallySyncScene = true;
                PhotonNetwork.LoadLevel("Game");
            }

        }else if(PhotonNetwork.offlineMode) {
            //OFFLINE MODE
            if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerChrSelect>().imready) {
                PhotonNetwork.LoadLevel("Game");

            }

        }
    }

    public void selectChr(Transform transform) {
        switch(playerID) {
            case 1:
                if(!p1.GetComponent<PlayerChrSelect>().imready) {
                    p1.transform.SetParent(transform);
                    p1.transform.localScale = Vector3.one;
                    p1.transform.localPosition = Vector3.zero;
                }
                break;
            case 2:
                if(!p2.GetComponent<PlayerChrSelect>().imready) {
                    p2.transform.SetParent(transform);
                    p2.transform.localScale = Vector3.one;
                    p2.transform.localPosition = Vector3.zero;
                }
                break;
            default:
                break;
        }
    }
    
    public void setChrID(int id) {
        switch(playerID) {
            case 1:
                if(!p1.GetComponent<PlayerChrSelect>().imready) {
                    PlayerPrefs.SetInt("chrID", id);
                }
                break;
            case 2:
                if(!p2.GetComponent<PlayerChrSelect>().imready) {
                    PlayerPrefs.SetInt("chrID", id);
                }
                break;
            default:
                break;
        }


    }
    public void IamReady() {
        switch(playerID) {
            case 1:
                p1.GetComponent<PhotonView>().RPC("ready", PhotonTargets.All);

                break;
            case 2:
                p2.GetComponent<PhotonView>().RPC("ready", PhotonTargets.All);


                break;


            default:
                
                break;
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer other) {
       // Debug.Log("OnPhotonPlayerConnected() "); // not seen if you're the player connecting

        if(PhotonNetwork.isMasterClient) {
            //Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected



        }

    }
}
