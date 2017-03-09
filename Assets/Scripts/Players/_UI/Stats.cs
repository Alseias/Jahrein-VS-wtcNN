using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : Photon.PunBehaviour
{
    public const float maxHealth = 100;
    GameObject hud;
    public float currentHealth;
    public bool isAlive, isDamageTaken;
    public Sprite Sprite;
    Player _player;
    GameObject healthbar;

    void Start ()
    {
        _player = GetComponent<Player>();
        isAlive = true;
        currentHealth = maxHealth;
        healthbar = GameObject.Find("healthbar").gameObject;
        //Debug.Log(hud.transform.FindChild("healthSprite").GetComponent<SpriteRenderer>().sprite.name);
        isDamageTaken = false;
    }

    [PunRPC]
    public void TakeDamage (int damage)
    {
        if (!isDamageTaken)
        {
            if (photonView.isMine)
            {
                Debug.Log("Damage taken");
                currentHealth -= damage;
                _player.health -= damage;
                if (currentHealth <= 0)
                {
                    isAlive = false;
                    if (this.gameObject.name == "wtcn")
                        this.gameObject.GetComponent<wtcnnSkills>().playSound(5);
                    else
                        this.gameObject.GetComponent<JahreinSkills>().playSound(5);

                    this.gameObject.GetComponent<Player>().enabled = false;
                    this.gameObject.GetComponent<JahreinSkills>().enabled = false;
                    this.gameObject.GetComponent<PhotonView>().RPC("DyingAnimTrigger", PhotonTargets.All);
                }
                float _health = currentHealth / maxHealth;
                OnChangeHealth(_health);
                isDamageTaken = true;
           }
        }
    }

    void Dead()
    {
        this.gameObject.GetComponent<PhotonView>().RPC("DeadAnimTrigger", PhotonTargets.All);
    }


    void OnChangeHealth (float health)
    {
        healthbar.transform.localScale = new Vector3(health, healthbar.transform.localScale.y, healthbar.transform.localScale.z);
    }

    public void playerOneHud()
    {

       hud = PhotonNetwork.Instantiate("p1HealthUI", new Vector3(-5, 4, 0), Quaternion.identity, 0);
       hud.transform.FindChild("healthSprite").GetComponent<SpriteRenderer>().sprite = Sprite;

    }

    public void playerTwoHud()
    {
        hud = PhotonNetwork.Instantiate("p2HealthUI", new Vector3(5, 4, 0), Quaternion.identity, 0);
        hud.transform.FindChild("healthSprite").GetComponent<SpriteRenderer>().sprite = Sprite;

    }

    #region Animation RPC

    [PunRPC]
    void DyingAnimTrigger()
    {
        this.gameObject.GetComponent<Animator>().Play("dying");
    }

    [PunRPC]
    void DeadAnimTrigger()
    {
        this.gameObject.GetComponent<Animator>().Play("dead");
    }
    #endregion
}
