﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Photon.PunBehaviour {



    //call functions from other scripts
    static public GameManager Instance;
    public GameObject[] playerPrefabs;


    private void Start() {
        //call from other scripts
        Instance = this;
        int selectedChrID = PlayerPrefs.GetInt("chrID");
        if(playerPrefabs == null) {
            Debug.LogError("Missing PlayerPrefab.Set up GameObject for GameManager script");
        }else {
            PhotonNetwork.Instantiate(this.playerPrefabs[selectedChrID].name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        }

    }

    public void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    void LoadArena()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.playerCount);
        PhotonNetwork.LoadLevel("Game");
    }
    #region Photon Messages


    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerConnected() " + other.name); // not seen if you're the player connecting


        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected


            //LoadArena();
        }
    }


    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerDisconnected() " + other.name); // seen when other disconnects


        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected


            //LoadArena();
        }
    }


    #endregion
}