using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wtcnGasm : MonoBehaviour {

    public int wtcnGasmDamage = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered 1");
        if (other.gameObject.CompareTag("Player"))
        {
            //other.gameObject.GetComponent<Stats>().TakeDamage(wtcnGasmDamage);
            Debug.Log("Triggered 2");
        }
    }
}
