using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : Photon.PunBehaviour
{
    public bool canMove = true;
    public bool canUseSkill = true;
	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float moveSpeed = 6;
    bool getFacingRight;
    public bool isfacingRight;
    GameObject target, player;

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
    private void Awake() {
        animator = GetComponent<Animator>();

    }

    //bool turned = true;

    void Start()
    {
        player = this.gameObject;

		controller = GetComponent<Controller2D> ();

        animator = GetComponent<Animator>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
        target = GameObject.FindGameObjectWithTag("Player");
        if (PlayerPrefs.GetInt("player") == 1 && 
            ((gameObject.name == "wtcn" && transform.localScale.x == -1) ||
            (gameObject.name == "jahRay" && transform.localScale.x == 1)))
        {
            isfacingRight = true;
        }
        else
        {
            isfacingRight = false;
            player.transform.localScale = new Vector2(transform.localScale.x * -1, 1);
        }
	}

	void Update()
    {
        
        
       

        if(!photonView.isMine)
        {
            animator.SetInteger("State", state);

            timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(positionAtLastPacket, realPosition, (float)(currentTime / timeToReachGoal));
            isfacingRight = getFacingRight;
            this.gameObject.layer = 10;

        }else {
            if(canMove) {
                CalculateVelocity();
            }
            LookAtTarget();
            HandleWallSliding();
            controller.Move(velocity * Time.deltaTime, directionalInput);

        }


        if(controller.collisions.above || controller.collisions.below)
        {
			if (controller.collisions.slidingDownMaxSlope)
            {
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			}
            else
            {
				velocity.y = 0;
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

    void LookAtTarget() //This function changes players direction toward enemy
    {
        if(target != null)
        {
            float xDirection = target.transform.position.x - this.transform.position.x;
            if (xDirection < 0 && !isfacingRight || xDirection > 0 && isfacingRight)
            {
                Debug.Log("if true");
                ChangeDirection();
            }
        }
    }

    void ChangeDirection()
    {
        isfacingRight = !isfacingRight;
        target.transform.localScale = new Vector2(transform.localScale.x * -1, 1);
        player.transform.localScale = new Vector2(transform.localScale.x * -1, 1);
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
            stream.SendNext(this.transform.position);
            //stream.SendNext(health);
            stream.SendNext(animator.GetInteger("State")); //Set animation state
            stream.SendNext(isfacingRight);
        }
        else
        {
            //correctPosition = (Vector3)stream.ReceiveNext();

            currentTime = 0.0;
            positionAtLastPacket = transform.position;
            realPosition = (Vector3)stream.ReceiveNext();
            //this.health = (float)stream.ReceiveNext();
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.timestamp;
            state = (int)stream.ReceiveNext(); //Get Animation state
            getFacingRight = (bool)stream.ReceiveNext();
        }
    }
}
