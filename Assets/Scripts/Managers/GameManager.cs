using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : Photon.PunBehaviour{
    //call functions from other scripts
    static public GameManager Instance;
    public GameObject[] playerPrefabs;
    //public GameObject healthOne, healthTwo;
    public GameObject playerOne, playerTwo;
    public Text txtStart;
    public bool p_gameStart = false;
    public AudioClip[] audioClips;

    private AudioSource audio;
    private const int TIME_TO_START_MATCH = 3;
    private float countDown = 0;
    private bool rpcStart=false;

    int selectedChrID;
    bool toStart;
    int playerID;
    private void Start()
    {
         
        //call from other scripts
        Instance = this;
        audio = GetComponent<AudioSource>();
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
                PhotonNetwork.Instantiate("p1HealthUI", new Vector3(-5, 4, 0), Quaternion.identity, 0);
                PhotonNetwork.isMessageQueueRunning = true;
                
                //this.playerPrefabs[selectedChrID].GetComponent<Stats>().playerOneHud();
                //playerOne.GetComponent<Player>().canMove = false;
            } else {
                playerTwo = (GameObject)PhotonNetwork.Instantiate(this.playerPrefabs[selectedChrID].name, playerTwo.transform.position, Quaternion.identity, 0);
                PhotonNetwork.Instantiate("p2HealthUI", new Vector3(5, 4, 0), Quaternion.identity, 0);

                //this.playerPrefabs[selectedChrID].GetComponent<Stats>().playerTwoHud();
                //playerTwo.GetComponent<Player>().isfacingRight = true;

                //playerTwo.GetComponent<Player>().canMove = false;

            }
        }



    }
    private void Update()
    {
        if(toStart) {
            countDown += Time.deltaTime;

            txtStart.text = string.Format("Match starts in {0}...", TIME_TO_START_MATCH - Mathf.FloorToInt(countDown));

            if(countDown >= TIME_TO_START_MATCH) {
                playAudios(0);
                p_gameStart = true;
                txtStart.gameObject.SetActive(false);

                toStart = false;
            }
        }

    }
    
    //master client okay with start
    public void startMatchMaster() {
        if(!rpcStart) {
            rpcStart = true;
            photonView.RPC("StartMatch", PhotonTargets.AllViaServer);
        }
    }

    [PunRPC]
    void StartMatch() {
        Debug.Log("Start match");
        toStart = true;
        txtStart.gameObject.SetActive(true);
        playAudios(1);

    }
    void playAudios(int number) {
        audio.clip = audioClips[number];
        audio.Play();

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
