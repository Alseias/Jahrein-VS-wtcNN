using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBarNetworkSycn : Photon.PunBehaviour {
    public Sprite[] sprites;
    [PunRPC]
    void changeSprite(int spr) {
        transform.FindChild("healthSprite").GetComponent<SpriteRenderer>().sprite = sprites[spr];
    }
}
