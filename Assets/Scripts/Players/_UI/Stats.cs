using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : Photon.PunBehaviour
{
    public const float maxHealth = 100;
    public GameObject hud;
    public float currentHealth;

    GameObject healthbar;

    void Start ()
    {
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
                Debug.Log("YOU ARE DIE !!");
            }
            float _health = currentHealth / maxHealth;//Healthbar's x axis size is 1 by default. This gives us a number between 0-1.
            OnChangeHealth(_health);
        }
    }

    [PunRPC]
    void OnChangeHealth (float health)
    {
        healthbar.transform.localScale = new Vector3(health, healthbar.transform.localScale.y, healthbar.transform.localScale.z);
    }

    public void JahInstantiateHud()
    {
        PhotonNetwork.Instantiate(hud.name, new Vector3(-8, 4, 0), Quaternion.identity, 0);
    }

    public void WtcnInstantiateHud()
    {
        PhotonNetwork.Instantiate(hud.name, new Vector3(8, 4, 0), Quaternion.identity, 0);
    }
}
