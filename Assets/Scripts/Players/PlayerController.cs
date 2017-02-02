using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Photon.PunBehaviour, IPunObservable
{
    public float health = 100f;
    public GameObject playerUiPrefab;
    public Vector3 realPosition = Vector3.zero;
    public Vector3 positionAtLastPacket = Vector3.zero;
    public double currentTime = 0.0;
    public double currentPacketTime = 0.0;
    public double lastPacketTime = 0.0;
    public double timeToReachGoal = 0.0;
    public float damage = 1f, speed = 1f;
    public bool canMove;
    public bool canJump, dontUseJump;

    void Awake()
    {
        canMove = true;
        if (playerUiPrefab == null)
        {
            Debug.LogError("Missing playerUiPrefab!!");
        }
        else
        {
            GameObject _uiGo;
            if (photonView.ownerId == 1)
                _uiGo = GameObject.FindGameObjectWithTag("uiOne");
            else
                _uiGo = GameObject.FindGameObjectWithTag("uiTwo");
            //_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
    }

    void Update()
    {
        playerInputs();
        if (!photonView.isMine)
        {
            timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(positionAtLastPacket, realPosition, (float)(currentTime / timeToReachGoal));
        }

        if (health <= 0)
        {
            health = 100;
        }
    }

    private void playerInputs()
    {
        if (canMove)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector2.left * Time.deltaTime * 3f);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector2.right * Time.deltaTime * 3f);
            }
            if (Input.GetKey(KeyCode.UpArrow) && !dontUseJump)
            {
                if (canJump)
                {
                    canJump = false;
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4f), ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground" && photonView.isMine)
        {
            canJump = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground" && photonView.isMine)
        {
            canJump = false;
        }
    }
    public void takeHit(float dmg)
    {
        this.health -= dmg;
    }

    Vector2 correctPosition;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(this.transform.position);
            stream.SendNext(health);
        }
        else
        {
            //correctPosition = (Vector3)stream.ReceiveNext();
            currentTime = 0.0;
            positionAtLastPacket = transform.position;
            realPosition = (Vector3)stream.ReceiveNext();
            this.health = (float)stream.ReceiveNext();
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.timestamp;
        }
    }
}