using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Photon.PunBehaviour
{
    //call functions from other scripts
    static public GameManager Instance;
    public GameObject[] playerPrefabs;
    //public GameObject healthOne, healthTwo;
    public GameObject playerOne, playerTwo;
    int selectedChrID;
    bool gameStart = false;

    private void Start()
    {
        //call from other scripts
        Instance = this;
        selectedChrID = PlayerPrefs.GetInt("chrID");
        int playerID = PlayerPrefs.GetInt("player");

        if(playerPrefabs == null)
        {
            Debug.LogError("Missing PlayerPrefab.Set up GameObject for GameManager script");
        }
        else
        {
            /* {
                healthOne.SetActive(true);
            }else {
                healthTwo.SetActive(true);
                healthOne.SetActive(true);

            }*/
            if(playerID==1)
            {
                playerOne= PhotonNetwork.Instantiate(this.playerPrefabs[selectedChrID].name, playerOne.transform.position, Quaternion.identity, 0);
                this.playerPrefabs[selectedChrID].GetComponent<Stats>().JahInstantiateHud();
                //playerOne.GetComponent<Player>().canMove = false;
            }
            else
            {
                playerTwo = PhotonNetwork.Instantiate(this.playerPrefabs[selectedChrID].name, playerTwo.transform.position, Quaternion.identity, 0);
                this.playerPrefabs[selectedChrID].GetComponent<Stats>().WtcnInstantiateHud();
                //playerTwo.GetComponent<Player>().canMove = false;

            }
        }



    }
    private void Update()
    {


    }
    IEnumerator debug() {
        yield return new WaitForSeconds(3);
        Debug.LogError(Time.deltaTime);
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
        Debug.Log(PhotonNetwork.room.PlayerCount);
        if(PhotonNetwork.room.PlayerCount == 2) {
            Debug.Log("2  kişi var oda da");
        }
    }
    #region Photon Messages


    public override void OnPhotonPlayerConnected(PhotonPlayer other) {
        LoadArena();
        Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting

        if(PhotonNetwork.isMasterClient) {
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected



        }

    }


    public override void OnPhotonPlayerDisconnected(PhotonPlayer other) {
        Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects


        if(PhotonNetwork.isMasterClient) {
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected


            //LoadArena();
        }
    }


    #endregion

}
