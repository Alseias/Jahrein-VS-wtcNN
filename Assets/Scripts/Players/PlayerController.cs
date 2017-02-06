using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Photon.PunBehaviour,IPunObservable {
    public float health = 100f,damage=1f,speed=1f;
    public Vector3 realPosition = Vector3.zero;
    public Vector3 positionAtLastPacket = Vector3.zero;

    //skill settings-->
    public Sprite[] skillSprites;
    public float[] skillCoolDowns;
    public float[] skillDurations;

    string[] skillKeyMaps = {"SkillQ", "SkillW", "SkillE", "SkillR" };
    
    //end of skill settings---|

    public bool dontUseJump;
    [HideInInspector]
    public bool canJump, canTakeHit,canMove,canUseSkill;


    double currentTime = 0.0;
    double currentPacketTime = 0.0;
    double lastPacketTime = 0.0;
    double timeToReachGoal = 0.0;

    
    //GameObject _uiGo;
    void Awake () {

        //setHealthBar();

        canMove = true;
        canJump = false;

        
        if(photonView.isMine) {

            setSkills();
        }


    }
    /*
    private void setHealthBar() {

        if(PhotonNetwork.room.PlayerCount == 1) {
            Debug.LogWarning("find and set ui");
            _uiGo = GameObject.FindGameObjectWithTag("uiOne");

        } else if(PhotonNetwork.room.PlayerCount == 2) {
            _uiGo = GameObject.FindGameObjectWithTag("uiTwo");
        }
        _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }*/
    [HideInInspector]
    public AbilityCoolDown[] skillCoolDownCheck;
    void setSkills() {
        skillCoolDownCheck = new AbilityCoolDown[6];
        GameObject skillCanvas = GameObject.Find("SkillSet");
        GameObject skillUI;
        for(int i = 0; i < 5; i++) {
            skillUI = Instantiate(Resources.Load("ui/SkillUI"), Vector3.zero, skillCanvas.transform.rotation, skillCanvas.transform) as GameObject;
            skillUI.transform.FindChild("skillSprite").GetComponent<Image>().sprite = skillSprites[i];
            
            skillCoolDownCheck[i] = skillUI.GetComponent<AbilityCoolDown>();
            skillCoolDownCheck[i].setTarget(this);
            skillCoolDownCheck[i].coolDownDuration = skillCoolDowns[i];
            if(i < 4) {
                skillCoolDownCheck[i].abilityButtonAxisName = skillKeyMaps[i];
                skillCoolDownCheck[i].durationTime = skillDurations[i];
            }else {
                skillCoolDownCheck[i].isPassive = true;
                skillUI.GetComponent<RectTransform>().localScale = new Vector3(.85f, .85f, 1);
            }
            
        }
        skillUI = Instantiate(Resources.Load("ui/chrStats"), Vector3.zero, skillCanvas.transform.rotation, skillCanvas.transform) as GameObject;
        skillUI.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        
    }


    void Update () {
        playerInputs();
        if(!photonView.isMine) {
            timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(positionAtLastPacket, realPosition, (float)(currentTime / timeToReachGoal));
        }

        if(health <= 0) {
            health = 100;
        }
		
	}



    private void playerInputs() {
        if(canMove) {
            if(Input.GetKey(KeyCode.LeftArrow)) {
                transform.Translate(Vector2.left * Time.deltaTime * 3f);
            }
            if(Input.GetKey(KeyCode.RightArrow)) {
                transform.Translate(Vector2.right * Time.deltaTime * 3f);
            }
            if(Input.GetKey(KeyCode.UpArrow)&&!dontUseJump) {

                if(canJump) {
                    canJump = false;
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 2f), ForceMode2D.Impulse);
                    
                } 
                
            }

        }

    }

    private void OnCollisionStay2D(Collision2D collision) {
        if(collision.transform.tag == "Ground"&&photonView.isMine) {
            canJump = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if(collision.transform.tag == "Ground" && photonView.isMine) {
            canJump = false;
        }
    }
    [PunRPC]
    public bool takeHit(float dmg) {
        Debug.Log("alınan hasar: " + dmg);
        this.health -= dmg;
        return true;
    }

    Vector2 correctPosition;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting) {
            stream.SendNext(this.transform.position);
            stream.SendNext(health);
        }else {
            //correctPosition = (Vector3)stream.ReceiveNext();

            currentTime = 0.0;
            positionAtLastPacket = transform.position;
            realPosition = (Vector3)stream.ReceiveNext();
            this.health = (float)stream.ReceiveNext();
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.timestamp;
        }
    }
}
