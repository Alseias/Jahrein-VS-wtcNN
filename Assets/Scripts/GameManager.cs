using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Photon.PunBehaviour {



    //call functions from other scripts
    static public GameManager Instance;
    public GameObject[] playerPrefabs;
    public GameObject healthOne, healthTwo;
    int selectedChrID;
    private void Start() {
        //call from other scripts
        Instance = this;
        selectedChrID = PlayerPrefs.GetInt("chrID");
        if(playerPrefabs == null) {
            Debug.LogError("Missing PlayerPrefab.Set up GameObject for GameManager script");
        }else {
            /* {
                healthOne.SetActive(true);
            }else {
                healthTwo.SetActive(true);
                healthOne.SetActive(true);

            }*/
            PhotonNetwork.Instantiate(this.playerPrefabs[selectedChrID].name, new Vector3(Random.Range(-8, 8), 0, 0), Quaternion.identity, 0);


        }

    }
    private void Update() {
       

    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena()
    {
        if(PhotonNetwork.room.PlayerCount == 2) {
            
        }
    }
    #region Photon Messages


    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        LoadArena();
        Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting
        
        if(PhotonNetwork.isMasterClient) {
                Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected


                
            }
        
    }


    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects


        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected


            //LoadArena();
        }
    }


    #endregion
}
