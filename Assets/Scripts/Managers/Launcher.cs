using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Launcher : Photon.PunBehaviour
{
    // Set game version
    string gameVersion = "1";
    public byte MaxPlayersPerRoom = 4;
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
    public bool offlineMode;
    bool isConnecting;
    public InputField playerName;
    public InputField roomName;
    

    private void Awake()
    {
        //force full log.
        PhotonNetwork.logLevel = Loglevel;

        //it must be false to list lobbies
        PhotonNetwork.autoJoinLobby = false;

        //make sure use photonNetwork on master and clients
        if(offlineMode)
        {
            PhotonNetwork.offlineMode = true;
        }
        else
        {
            PhotonNetwork.automaticallySyncScene = true;
            if(PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)
            {
                // Connect to the photon master-server.
                PhotonNetwork.ConnectUsingSettings(gameVersion);
            }
        }
    }
    void Start ()
    {
        //Connect();
	}

    public void Connect()
    {
        isConnecting = true;
        //check if we are connected,join,initiate con.
        if (PhotonNetwork.connected)
        {
            if(isConnecting)
                PhotonNetwork.JoinRandomRoom();
        }
        else
            PhotonNetwork.ConnectUsingSettings(gameVersion);
    }

    public void checkPlayerName()
    {
        PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
        PhotonNetwork.playerName = playerName.text+" Ping:"+PhotonNetwork.GetPing();
    }
    public void selectCharacter(int id)
    {
        PlayerPrefs.SetInt("chrID", id);
    }
    public void CreateRoom()
    {
        Debug.Log(roomName.text);
        PhotonNetwork.CreateRoom(roomName.text, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }

    //------------------------------------------------------------------------------------------
    public override void OnConnectedToMaster()
    {

        Debug.Log("OnConnectedToMaster() was called by PUN | Ping: "+PhotonNetwork.GetPing() );

    }

    public override void OnDisconnectedFromPhoton()
    {


        Debug.LogWarning("OnDisconnectedFromPhoton() was called by PUN");
    }
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
       
            Debug.Log("We load the 'Room for 1' ");


            // #Critical
            // Load the Room Level. 
            PhotonNetwork.LoadLevel("Game");

    }
    public override void OnCreatedRoom()
    {
        //Debug.Log("OnCreatedRoom");
        PhotonNetwork.LoadLevel("Game");
    }

}
