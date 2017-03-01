using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : Photon.PunBehaviour
{
    public const float maxHealth = 100;
    public GameObject hud;
    public float currentHealth;
    public bool isAlive;

    GameObject healthbar;

    void Start ()
    {
        isAlive = true;
        currentHealth = maxHealth;
        healthbar = GameObject.Find("healthbar").gameObject;
    }

    public void TakeDamage (int damage)
    {
        if (photonView.isMine)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                isAlive = false;
                Debug.Log("YOU ARE DIE !!");
                if (this.gameObject.name == "wtcn")
                    this.gameObject.GetComponent<wtcnnSkills>().playSound(5);
                else
                    this.gameObject.GetComponent<JahreinSkills>().playSound(5);
                this.gameObject.GetComponent<Player>().canMove = false;
                this.gameObject.GetComponent<Player>().canUseSkill = false;
                this.gameObject.GetComponent<Animator>().Play("dying");
                this.gameObject.GetComponent<Animator>().SetInteger("State", 8);

            }
            float _health = currentHealth / maxHealth;
            OnChangeHealth(_health);
        }
        else
        {
            if(currentHealth <= 0)
            {
                Debug.Log("YOU ARE DIE !!");

            }
        }
    }

    void Dead()
    {
        this.gameObject.GetComponent<Animator>().SetInteger("State", 8);
        this.gameObject.GetComponent<Animator>().Play("dead");
    }

    [PunRPC]
    void OnChangeHealth (float health)
    {
        healthbar.transform.localScale = new Vector3(health, healthbar.transform.localScale.y, healthbar.transform.localScale.z);
    }

    public void JahInstantiateHud()
    {
        PhotonNetwork.Instantiate(hud.name, new Vector3(-5, 4, 0), Quaternion.identity, 0);
    }

    public void WtcnInstantiateHud()
    {
        PhotonNetwork.Instantiate(hud.name, new Vector3(5, 4, 0), Quaternion.identity, 0);
    }
}
