using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class charStats : MonoBehaviour
{
    public Text dmg, spd, hp;

    PlayerController _pc;

    void Update ()
    {
        if(_pc != null)
        {
            dmg.text = "DMG: "+_pc.damage;
            spd.text = "SPD: "+_pc.speed;
            hp.text = "HP: "+_pc.health;
        }

        else
        {
            this.gameObject.SetActive(false);
        }
	}
	
	public void SetTarget (PlayerController pc)
    {
        _pc = pc;
	}
}
