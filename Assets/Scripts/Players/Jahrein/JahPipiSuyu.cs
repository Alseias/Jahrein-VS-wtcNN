using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JahPipiSuyu : Photon.PunBehaviour
{
    public float fDir;
    public int pvID;
    void Start ()
    {
        Destroy(gameObject, 2);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(15 * fDir, 5) * 50f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.name == "wtcn(Clone)") {
            if(photonView.isMine)
            collision.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, 10f);
            Destroy(gameObject);

        }
    }
}
