using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class wtcnnSkills : Photon.PunBehaviour
{
    public GameObject shurikenSpawnPoint;
    public GameObject shuriken;
    public Sprite[] skillSprites;
    public GameObject skillUiPref;

    bool isFacingRight;
    Transform trans;
    PlayerController pc;
    PhotonView pv;
    bool isGrounded, canDoubleJump;
	void Start ()
    {
        pc = GetComponent<PlayerController>();
        pv = GetComponent<PhotonView>();
        isGrounded = pc.canJump;
        canDoubleJump = true;
        if(photonView.isMine)
            setSkills();

    }
    void setSkills() {
        GameObject skillCanvas = GameObject.Find("SkillSet");
        GameObject skillUI;
        for(int i = 0; i < 4; i++) {
            skillUI = Instantiate(skillUiPref, new Vector3(100 + (100 * i), 50, 0), skillCanvas.transform.rotation, skillCanvas.transform);
            skillUI.GetComponent<Image>().sprite = skillSprites[i];
        }
    }
    void Update ()
    {
        if(pv.isMine) {
            isGrounded = pc.canJump;
            if(Input.GetKeyDown(KeyCode.Space)) {
                pv.RPC("BasicAttack", PhotonTargets.All);
            }
            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                if(canDoubleJump) {
                    DoubleJump();
                }
            }
            if(!isGrounded)
                canDoubleJump = false;
        }
    }

    [PunRPC]
    void BasicAttack()
    {
        Instantiate(shuriken, shurikenSpawnPoint.transform.position, Quaternion.identity);
    }

    void DoubleJump()
    {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
        canDoubleJump = true;
    }
}
