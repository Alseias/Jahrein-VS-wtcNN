using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : Photon.PunBehaviour
{
    public bool canMove = false;
    public bool canUseSkill = true;
    public bool canDoubleJump;
	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float moveSpeed = 6;
    public float health = 100;
    public float damage;
    bool getFacingRight;
    public bool isfacingRight;
    public GameObject target, player;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;
    

    public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	public Vector3 velocity;
	float velocityXSmoothing;

    Animator animator;
    bool gamestart;
	public Controller2D controller;
    int state=0;
	Vector2 directionalInput;
	bool wallSliding;
	int wallDirX;

    Vector3 realPosition = Vector3.zero;
    Vector3 positionAtLastPacket = Vector3.zero;
    double currentTime = 0.0;
    double currentPacketTime = 0.0;
    double lastPacketTime = 0.0;
    double timeToReachGoal = 0.0;
    bool turned = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if(!photonView.isMine)
        {
            this.gameObject.tag = "enemy";
        }
        target = GameObject.FindGameObjectWithTag("enemy");
        /*if(PhotonNetwork.isMasterClient) {
            GetComponent<Stats>().playerOneHud();
        }else {
            GetComponent<Stats>().playerTwoHud();
        }*/
    }

    void Start()
    {
        isfacingRight = true;
        player = this.gameObject;
        gamestart = GameObject.Find("GameManager").GetComponent<GameManager>().gameStart;
		controller = GetComponent<Controller2D> ();
        animator = GetComponent<Animator>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
    }

    void Update()
    {

        
        if (!photonView.isMine)
        {


            timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(positionAtLastPacket, realPosition, (float)(currentTime / timeToReachGoal));
            this.gameObject.layer = 10;
        }
        else
        {
            if(canMove) {
                CalculateVelocity();
            }

            HandleWallSliding();

            controller.Move(velocity * Time.deltaTime, directionalInput);
            if(target == null)
            {
                target = GameObject.FindGameObjectWithTag("enemy");
            }
            if (canUseSkill)
            {
                LookAtTarget();
            }
            if(controller.collisions.above || controller.collisions.below) {
                if(controller.collisions.slidingDownMaxSlope) {
                    velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
                } else {
                    velocity.y = 0;
                }
            }
        }

       
	}
    


	public void SetDirectionalInput (Vector2 input)
    {
        if(canMove)
        {
            directionalInput = input;
        }
    }

	public void OnJumpInputDown()
    {
		if (wallSliding)
        {
			if (wallDirX == directionalInput.x)
            {
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			else if (directionalInput.x == 0)
            {
				velocity.x = -wallDirX * wallJumpOff.x;
				velocity.y = wallJumpOff.y;
			}
			else
            {
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
		}
		if (controller.collisions.below && canMove)
        {
            canDoubleJump = true;
			if (controller.collisions.slidingDownMaxSlope)
            {
				if (directionalInput.x != -Mathf.Sign (controller.collisions.slopeNormal.x))
                { // not jumping against max slope
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			}
            else
            {
				velocity.y = maxJumpVelocity;
			}
		}else if(canDoubleJump&&(gameObject.name!= "jahRay(Clone)")) {
            canDoubleJump = false;
            velocity.y = maxJumpVelocity;
        }
	}

	public void OnJumpInputUp()
    {
		if (velocity.y > minJumpVelocity && canMove)
        {
			velocity.y = minJumpVelocity;
		}
	}
		

	void HandleWallSliding()
    {
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;
			if (velocity.y < -wallSlideSpeedMax)
            {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0)
            {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (directionalInput.x != wallDirX && directionalInput.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				}
				else
                {
					timeToWallUnstick = wallStickTime;
				}
			}
			else
            {
				timeToWallUnstick = wallStickTime;
			}

		}

	}

    void LookAtTarget() //This function changes players direction towards enemy
    {
        if(target != null)
        {
            float xDirection = Mathf.Sign(target.transform.position.x - this.transform.position.x);
            if (xDirection < 0 && isfacingRight || xDirection > 0 && !isfacingRight)
            {
                ChangeDirection();
            }
        }
    }

    [PunRPC]
    void ChangeDirection()
    {
        isfacingRight = !isfacingRight;
        this.transform.localScale = new Vector2(transform.localScale.x * -1, 1);
    }

	void CalculateVelocity()
    {
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}

    //Vector2 correctPosition;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(animator.GetInteger("State")); //Set animation state
            stream.SendNext(this.transform.position);
            stream.SendNext(health);
            stream.SendNext(this.transform.localScale);
            stream.SendNext(isfacingRight);
            stream.SendNext(damage);
        }
        else
        {
            //correctPosition = (Vector3)stream.ReceiveNext();
            animator.SetInteger("State",(int)stream.ReceiveNext());//Get Animation state
            currentTime = 0.0;
            positionAtLastPacket = transform.position;
            realPosition = (Vector3)stream.ReceiveNext();
            health = (float)stream.ReceiveNext();
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.timestamp;
            target.transform.localScale = (Vector3)stream.ReceiveNext();
            isfacingRight = (bool)stream.ReceiveNext();
            damage = (float)stream.ReceiveNext();
        }
    }
}
