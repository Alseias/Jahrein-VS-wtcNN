using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vidanjor : MonoBehaviour {
    private void Update() {
        GetComponent<Rigidbody2D>().AddForce(transform.right * 50f);
    }
    private void OnTriggerExit2D(Collider2D collision) {
        
        if(collision.tag == "Player") {
            collision.GetComponent<PlayerController>().takeHit(.25f);
        }
    }
    public void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
