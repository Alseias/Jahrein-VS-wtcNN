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
    public bool gameStart = false;
    int playerID;
    private void Start()
    {

        //call from other scripts
        Instance = this;
        selectedChrID = PlayerPrefs.GetInt("chrID");
        playerID = PlayerPrefs.GetInt("player");
        if(playerPrefabs == null) {
            Debug.LogError("Missing PlayerPrefab.Set up GameObject for GameManager script");
        } else {
            /* {
                healthOne.SetActive(true);
            }else {
                healthTwo.SetActive(true);
                healthOne.SetActive(true);

            }*/
            if(playerID == 1) {
                playerOne = (GameObject)PhotonNetwork.Instantiate(this.playerPrefabs[selectedChrID].name, playerOne.transform.position, Quaternion.identity, 0);
                this.playerPrefabs[selectedChrID].GetComponent<Stats>().JahInstantiateHud();
                //playerOne.GetComponent<Player>().canMove = false;
            } else {
                playerTwo = (GameObject)PhotonNetwork.Instantiate(this.playerPrefabs[selectedChrID].name, playerTwo.transform.position, Quaternion.identity, 0);
                this.playerPrefabs[selectedChrID].GetComponent<Stats>().WtcnInstantiateHud();
                //playerTwo.GetComponent<Player>().isfacingRight = true;

                //playerTwo.GetComponent<Player>().canMove = false;

            }
        }



    }
    private void Update()
    {
        if(PhotonNetwork.room.PlayerCount == 2) {
            treeSecond();

        }

    }
    
    IEnumerator treeSecond() {
        yield return new WaitForSeconds(3f);
        gameStart = true;

    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


}
