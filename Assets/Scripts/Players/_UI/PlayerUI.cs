using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public Text playerNameText;
    public Slider playerHealthSlider;
    public Vector3 ScreenOffset = new Vector3(0, 30f, 0);

    Player _target;
    Transform _targetTransform;
    Vector3 _targetPosition;

    void Awake() {
        //this.GetComponent<Transform>().SetParent(GameObject.Find("Player").GetComponent<Transform>());

    }
    private void Start() {

    }
    void Update () {
        if(_target != null) {
            playerHealthSlider.value = _target.health;
        }
        else{
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
            return;
        }
	}


    public void SetTarget(Player target) {
        if(target == null) {
            Debug.LogError("Missing playerController target!!");
        }
        //Cache reference for efficiency
        _target = target;
        if(playerNameText != null) {
            playerNameText.text = _target.photonView.owner.NickName;
        }

        
    }
}
