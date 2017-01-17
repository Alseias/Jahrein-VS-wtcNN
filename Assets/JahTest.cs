using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class JahTest : MonoBehaviour
{
    Animator anim;
	void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetInteger("state", 2);
        }
	}
}
