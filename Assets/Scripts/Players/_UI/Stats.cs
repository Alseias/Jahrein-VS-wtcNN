using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : Photon.PunBehaviour
{
    GameObject hud;
    public bool isAlive, isDamageTaken;
    public Sprite spr;
    Player _player;
    GameObject healthbar;
    int playerID;

    void Start ()
    {
        _player = GetComponent<Player>();
        isAlive = true;
        playerID = PlayerPrefs.GetInt("chrID");

        if(hud == null)
        {
            if(PhotonNetwork.isMasterClient || PhotonNetwork.offlineMode)
            {
                hud = GameObject.Find("p1HealthUI(Clone)");
                hud.GetComponent<PhotonView>().RPC("changeSprite", PhotonTargets.AllBuffered, playerID);
            }
            else
            {
                hud = GameObject.Find("p2HealthUI(Clone)");
                hud.GetComponent<PhotonView>().RPC("changeSprite", PhotonTargets.All, playerID);
            }
        }
        healthbar = hud.transform.FindChild("healthbar").gameObject;

        /*if()
        {
            hud = GameObject.Find("p1HealthUI(Clone)");
            hud.GetComponent<PhotonView>().RPC("changeSprite", PhotonTargets.All, playerID);
        }*/
        isDamageTaken = false;
    }

    private void Update()
    {
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        Debug.Log("Damage taken, "+this.gameObject.name+" " +photonView.viewID);
        //currentHealth -= damage;
        _player.health -= damage;
        if(_player.health <= 0 && isAlive)
        {
            isAlive = false;
            if(this.gameObject.name == "wtcn(clone)")
                this.gameObject.GetComponent<wtcnnSkills>().playSound(5);
            else
                this.gameObject.GetComponent<JahreinSkills>().playSound(5);

            this.gameObject.GetComponent<Player>().canMove = false;
            //this.gameObject.GetComponent<JahreinSkills>().enabled = false;
            photonView.RPC("DyingAnimTrigger", PhotonTargets.All);
        }
        float _health = _player.health / 100;
        //OnChangeHealth(_health);
        if(photonView.isMine) {
            healthbar.transform.localScale = new Vector3(_health, healthbar.transform.localScale.y, healthbar.transform.localScale.z);

        }

        isDamageTaken = true;
    }

   /* void Dead()
    {
        photonView.RPC("DeadAnimTrigger", PhotonTargets.All);
    }*/

    void OnChangeHealth (float health)
    {
        healthbar.transform.localScale = new Vector3(health, healthbar.transform.localScale.y, healthbar.transform.localScale.z);
    }

    public void playerOneHud()
    {
        PhotonNetwork.Instantiate("p1HealthUI", new Vector3(-5, 4, 0), Quaternion.identity, 0);
    }

    public void playerTwoHud()
    {
        PhotonNetwork.Instantiate("p2HealthUI", new Vector3(5, 4, 0), Quaternion.identity, 0);
    }
    #region Animation RPC

    [PunRPC]
    void DyingAnimTrigger()
    {
        this.gameObject.GetComponent<Animator>().Play("dying");
        GameObject.Find("GameManager").GetComponent<GameManager>().restartGame();
    }

    #endregion
}
