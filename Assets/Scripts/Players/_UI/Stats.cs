using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : Photon.PunBehaviour
{
    public const float maxHealth = 100;
    GameObject hud;
    public float currentHealth;
    public bool isAlive, isDamageTaken;
    public Sprite spr;
    Player _player;
    GameObject healthbar;
    int playerID;

    void Start ()
    {
        _player = GetComponent<Player>();
        isAlive = true;
        currentHealth = maxHealth;
        healthbar = GameObject.Find("healthbar").gameObject;
        playerID = PlayerPrefs.GetInt("chrID");

        if(hud == null && !PhotonNetwork.offlineMode)
        {
            if(PhotonNetwork.isMasterClient)
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
        if(PhotonNetwork.offlineMode)
        {
            hud = GameObject.Find("p1HealthUI(Clone)");
            hud.GetComponent<PhotonView>().RPC("changeSprite", PhotonTargets.All, playerID);
        }
        isDamageTaken = false;
    }

    private void Update()
    {
        Debug.Log(healthbar.name);
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        Debug.Log("Damage taken, "+this.gameObject.name);
        currentHealth -= damage;
        _player.health -= damage;
        if(currentHealth <= 0 && isAlive)
        {
            isAlive = false;
            if(this.gameObject.name == "wtcn")
                this.gameObject.GetComponent<wtcnnSkills>().playSound(5);
            else
                this.gameObject.GetComponent<JahreinSkills>().playSound(5);

            this.gameObject.GetComponent<Player>().canMove = false;
            //this.gameObject.GetComponent<JahreinSkills>().enabled = false;
            photonView.RPC("DyingAnimTrigger", PhotonTargets.All);
        }
        float _health = currentHealth / maxHealth;
        OnChangeHealth(_health);
        isDamageTaken = true;
    }

    void Dead()
    {
        photonView.RPC("DeadAnimTrigger", PhotonTargets.All);
    }

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
    }

    #endregion
}
