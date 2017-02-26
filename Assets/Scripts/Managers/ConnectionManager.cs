using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ConnectionManager : Photon.PunBehaviour {
    string gameVersion = "1";
    public byte MaxPlayersPerRoom;
    public string roomName="4n4n";
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
    public bool offlineMode;
    public Button btn_play;

    private void Awake() {
        //force full log.
        PhotonNetwork.logLevel = Loglevel;
        btn_play.GetComponentInChildren<Text>().text = "Connecting..";
        //it must be false to list lobbies
        PhotonNetwork.autoJoinLobby = false;

        //make sure use photonNetwork on master and clients
        if(offlineMode) {
            PhotonNetwork.offlineMode = true;
        } else {
            PhotonNetwork.automaticallySyncScene = true;
            if(PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated) {
                // Connect to the photon master-server.
                PhotonNetwork.ConnectUsingSettings(gameVersion);
            }
        }
    }

    // Use this for initialization
    void Start () {
        PlayerPrefs.DeleteAll();
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateRoom() {
        PlayerPrefs.SetInt("player", 1);
        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
        
    }

    //joining same room for 2. player
    public void JoinRoom() {
        if(PhotonNetwork.connectedAndReady) {
            PlayerPrefs.SetInt("player", 2);
            PhotonNetwork.JoinRandomRoom();
        }

    }

    #region Photon Logs
    //------------------------------------------------------------------------------------------
    public override void OnConnectedToMaster() {
        btn_play.interactable = true;
        btn_play.GetComponentInChildren<Text>().text = "Play";

        Debug.Log("OnConnectedToMaster() was called by PUN | Ping: " + PhotonNetwork.GetPing());

    }

    public override void OnDisconnectedFromPhoton() {


        Debug.LogWarning("OnDisconnectedFromPhoton() was called by PUN");
    }
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
        Debug.Log("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers ="+MaxPlayersPerRoom+"}, null);");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        CreateRoom();
    }
    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg) {
        JoinRoom();
    }
    public override void OnJoinedRoom() {
        Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

        Debug.Log("We load the 'Room for 1' ");


        // #Critical
        // Load the Room Level. 
        PhotonNetwork.LoadLevel("CharacterSelect");

    }
    public override void OnCreatedRoom() {
        //Debug.Log("OnCreatedRoom");
        PhotonNetwork.LoadLevel("CharacterSelect");
    }
    #endregion
}
