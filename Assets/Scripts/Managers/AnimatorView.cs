using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorView : Photon.MonoBehaviour
{
    Animator animator;
    int state;

	void Start ()
    {
        animator = GetComponent<Animator>();
        state = 0;
    }
	
	void Update ()
    {
        if (!photonView.isMine)
        {
            animator.SetInteger("State", state);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(animator.GetInteger("State")); //Set animation state
        }
        else
        {
            state = (int)stream.ReceiveNext(); //Get Animation state
        }
    }
}