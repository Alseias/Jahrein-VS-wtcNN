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
<<<<<<< HEAD
    bool canDoubleJump = false;
=======
    bool canDoubleJump=false;
>>>>>>> fb127976aef4b5725d42990bf5ac3d8a2cfb17bb
    string[] skillKeyMaps = { "SkillQ", "SkillW", "SkillE", "SkillR" };
    float[] skillCoolDowns = { 4, 4, 7, 25 };

    void Start()
    {
        pc = GetComponent<PlayerController>();
        pv = GetComponent<PhotonView>();
        
        if(photonView.isMine)
            setSkills();

        if (photonView.isMine)
            setSkills();

    }
    AbilityCoolDown[] skillCoolDownCheck = new AbilityCoolDown[4];
    void setSkills()
    {
        GameObject skillCanvas = GameObject.Find("SkillSet");
        GameObject skillUI;
        for (int i = 0; i < 4; i++)
        {
            skillUI = Instantiate(skillUiPref, new Vector3(100 + (100 * i), 50, 0), skillCanvas.transform.rotation, skillCanvas.transform) as GameObject;
            skillUI.GetComponentInChildren<Image>().sprite = skillSprites[i];

            skillCoolDownCheck[i] = skillUI.GetComponent<AbilityCoolDown>();
            skillCoolDownCheck[i].abilityButtonAxisName = skillKeyMaps[i];
            skillCoolDownCheck[i].coolDownDuration = skillCoolDowns[i];
        }
    }
<<<<<<< HEAD
    void Update()
=======
    void Update ()
>>>>>>> fb127976aef4b5725d42990bf5ac3d8a2cfb17bb
    {
        if (pv.isMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pv.RPC("BasicAttack", PhotonTargets.All);
            }
<<<<<<< HEAD
            if (Input.GetKeyDown(KeyCode.UpArrow))
=======
            if(Input.GetKeyDown(KeyCode.UpArrow))
>>>>>>> fb127976aef4b5725d42990bf5ac3d8a2cfb17bb
            {
                if (pc.canJump)
                {
                    Debug.Log("zıpladık");
                    pc.canJump = false;
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
                    canDoubleJump = true;
                }
                else
                {
                    if (canDoubleJump)
                    {
                        Debug.Log("zıpladık2x");
                        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
                        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
                        canDoubleJump = false;
                    }
                }
            }
        }
    }

    [PunRPC]
    void BasicAttack()
    {
        Instantiate(shuriken, shurikenSpawnPoint.transform.position, Quaternion.identity);
    }
}
