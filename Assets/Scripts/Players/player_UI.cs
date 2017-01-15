using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_UI : MonoBehaviour {

    public Text playerNameText;
    public Slider playerHealthSlider;
    public Vector3 ScreenOffset = new Vector3(0, 30f, 0);

    PlayerController _target;
    float _characterControllerHeight = 0;
    Transform _targetTransform;
    Vector3 _targetPosition;

    void Awake() {
        this.GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>());

    }
    private void Start() {
        if(PhotonNetwork.playerList.Length == 1) {
            this.GetComponent<RectTransform>().position = transform.parent.position;
        } else {
            this.GetComponent<RectTransform>().position = new Vector3(transform.parent.position.x+(100f*(PhotonNetwork.playerList.Length-1)), transform.parent.position.y);

        }
    }
    void Update () {
        if(playerHealthSlider != null) {
            playerHealthSlider.value = _target.health;
        }
        if(_target == null) {
            Destroy(this.gameObject);
            return;
        }
	}


    public void SetTarget(PlayerController target) {
        if(target == null) {
            Debug.LogError("Missing playerController target!!");
        }
        //Cache reference for efficiency
        _target = target;
        if(playerNameText != null) {
            playerNameText.text = _target.photonView.owner.name;
        }

        
    }
}
