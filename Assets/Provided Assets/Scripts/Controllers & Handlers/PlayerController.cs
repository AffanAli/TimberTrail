using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject climbingHand;
    [SerializeField] GameObject gunHand;
    [SerializeField] GameObject rightHand;
    [SerializeField] GameObject leftHand;
    [SerializeField] PlayerData player;
    [SerializeField] CameraFollow playerCamera;

    [Header("Player Attributes")]
    [SerializeField] float keyboardMoveSpeed = 5f;
    [SerializeField] float forwardSpeed;
    [SerializeField] Vector3 swipeRange;

    [Header("Climbing Hands Attributes")]
    [SerializeField] Vector2 rightHandOffset;
    [SerializeField] Vector2 leftHandOffset;
    [SerializeField] float climbHandSpeed;
    [SerializeField] float climbDelay;

    FinalWall wall;
    Rigidbody rb;
    Transform teleportPos;
    Animator leftAnim;
    Animator rightAnim;
    
    Vector3 nextRightPos;
    Vector3 nextLeftPos;
    Vector3 startingPos;
    Vector3 deathPos;
    Vector2 swipingRange;

    bool isRight;
    bool isClimbing;
    bool isDeath;
    bool isFinalWall;
    bool isMoving;
    int climbCount;

    public bool isShooting;

    int ladderCount;
    int currentIndex;
    int remainingLadder;


    Enums.HandPosition currentPosition;

    #region Unity Callbacks
    void Start()
    {
        Debug.Log("Player connected.");
        swipingRange = new(swipeRange.x, swipeRange.y);
        rb = GetComponent<Rigidbody>();
        leftAnim = leftHand.GetComponent<Animator>();
        rightAnim = rightHand.GetComponent<Animator>();
        startingPos = transform.position;
        isShooting = false;
        isRight = true;
        isClimbing = false;
        isDeath = false;
        climbCount = 0;
        isFinalWall = false;
        isMoving = true;
        currentIndex = 0;
        ladderCount = 0;
        remainingLadder = 0;
        ChangeState(Enums.HandPosition.gunInHand);
    }


    void Update()
    {
        if (!isDeath && CheckForTweens())
        {
            Movement();
        }
    }

    #endregion Unity Callbacks

    #region Functionalities

    bool CheckForTweens()
    {
        return !(LeanTween.isTweening(rightHand) || LeanTween.isTweening(leftHand));
    }

    void Movement()
    {
        if (isMoving)
        {
            rb.linearVelocity = new(rb.linearVelocity.x, rb.linearVelocity.y, forwardSpeed);
            KeyboardControls();
        }
        else if(isClimbing)
        {
            ClimbingState();
        }
    }

    private void ClimbingState()
    {
            if (isFinalWall)
            {
                ClimbFinalWall();
            }
    }

    private void KeyboardControls()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Arrow keys
        if (Mathf.Abs(horizontal) > 0.01f)
        {
            float newX = transform.position.x + horizontal * keyboardMoveSpeed * Time.deltaTime;
            newX = Mathf.Clamp(newX, swipingRange.x, swipingRange.y);
            rb.MovePosition(new Vector3(newX, rb.position.y, rb.position.z));
        }
    }
    void ClimbFinalWall()
    {
        if (LeanTween.isTweening(rightHand) || LeanTween.isTweening(leftHand)) return;
        int c = wall.Ladder.transform.childCount;
        climbCount++;

            if (climbCount < c && player.numberOfWoodInFactory > 0)
        {
            float offset = 0.05f;
            if (rightHand.transform.position.y != leftHand.transform.position.y)
            {
                offset *= 2;
            }
            if (isRight)
            {
                nextRightPos = new(rightHand.transform.position.x, rightHand.transform.position.y + offset, rightHand.transform.position.z);
                rightAnim.SetTrigger("Release");
                LeanTween.move(rightHand, nextRightPos, climbHandSpeed).setDelay(climbDelay).setOnComplete(() =>
                {
                    rightAnim.SetTrigger("Grab");
                    SoundManager.Instance.Play(Enums.SoundEffects.LadderGrab);
                });
                isRight = false;

            }
            else
            {
                nextLeftPos = new(leftHand.transform.position.x, leftHand.transform.position.y + offset, leftHand.transform.position.z);
                leftAnim.SetTrigger("Release");
                LeanTween.move(leftHand, nextLeftPos, climbHandSpeed).setDelay(climbDelay).setOnComplete(() =>
                {
                    leftAnim.SetTrigger("Grab");
                    SoundManager.Instance.Play(Enums.SoundEffects.LadderGrab);
                });
                isRight = true;
            }
            player.numberOfWoodInFactory--;
        }
        else if (remainingLadder > 0)
        {
            Death();
        }
        else
        {
            isFinalWall = false;
            GameManager.Instance.GameOver();
        }
    }
    void ResetClimbingHands()
    {
        rightHand.transform.localEulerAngles = new(0, 90, -40);
        leftHand.transform.localEulerAngles = new(0, 90, -40);
        isRight = true;
    }

    public void ResetPlayer()
    {
        isDeath = false;
        isClimbing = false;
        isFinalWall = false;
        climbingHand.SetActive(false);
        climbCount = 0;
        gunHand.SetActive(true);
        transform.position = startingPos;
        climbingHand.transform.localScale = new(1, 1, 1);
        ResetClimbingHands();
        playerCamera.target = transform;
        isMoving = true;
        swipingRange = new(swipeRange.x, swipeRange.y);
    }
    private void UpdateState()
    {
        if (ladderCount < 0)
        {
            if (!isDeath)
            {
                Death();
            }
        }
        else if (!isClimbing)
        {
            ChangeState(Enums.HandPosition.climbing);
        }
    }

    void Death()
    {
        isDeath = true;
        isClimbing = false;

        climbingHand.SetActive(true);
        gunHand.SetActive(false);

        climbingHand.transform.position = gunHand.transform.position;
        Vector3 rightPos = new(0.03f, 0, 0);
        SoundManager.Instance.Play(Enums.SoundEffects.LevelFailed);
        LeanTween.moveLocal(leftHand, Vector3.zero, 1f).setEaseInSine();
        LeanTween.rotateZ(leftHand, -230f, 1f).setEaseInSine();
        LeanTween.moveLocal(rightHand, rightPos, 1f).setEaseInSine();
        LeanTween.rotateZ(rightHand, -230f, 1f).setEaseInSine().setOnComplete(() =>
        {
            Invoke(nameof(GameOver), 0.2f);
        });
    }
    void GameOver()
    {
        GameManager.Instance.GameOver();
    }

    void ResetCamera()
    {
        playerCamera.smoothTime = 0f;
        transform.position = teleportPos.position;
        ChangeState(Enums.HandPosition.gunInHand);
    }

    void ChangeState(Enums.HandPosition position)
    {
        currentPosition = position;
        switch (currentPosition)
        {
            case Enums.HandPosition.climbing:
                ClimbingPosition();
                break;
            case Enums.HandPosition.gunInHand:
                GunInHandPosition();
                break;
        }
    }

    private void GunInHandPosition()
    {
        isClimbing = false;
        isMoving = true;
        climbingHand.SetActive(false);
        gunHand.SetActive(true);
        playerCamera.target = transform;
    }

    private void ClimbingPosition()
    {
        isMoving = false;
        isClimbing = true;
        player.numberOfWoodInFactory--;
        currentIndex++;
        ladderCount--;
        rightHand.transform.position = nextRightPos;
        leftHand.transform.position = nextLeftPos;
        gunHand.SetActive(false);
        climbingHand.SetActive(true);
        playerCamera.target = leftHand.transform;
    }



    #endregion Functionalities

    #region Collision/Trigger Functions

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FinalWall"))
        {
            remainingLadder = wall.Ladder.transform.childCount - player.numberOfWoodInFactory;
            if (player.numberOfWoodInFactory == 0)
            {
                ClimbFinalWall();
            }
            else
            {
                nextLeftPos = wall.point.position;
                nextRightPos = wall.point.position;
                nextRightPos = new(nextRightPos.x - rightHandOffset.x, nextRightPos.y, nextRightPos.z - rightHandOffset.y);
                nextLeftPos = new(nextLeftPos.x - leftHandOffset.x, nextLeftPos.y, nextLeftPos.z - leftHandOffset.y);
                ChangeState(Enums.HandPosition.climbing);
                isFinalWall = true;
            }
        }
    }


    private void OnCollisionExit(Collision collision)
    {
  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ExitPoint")
        {
            isShooting = true;
        }
        if (other.gameObject.CompareTag("FinalWall"))
        {
            wall = other.gameObject.GetComponent<FinalWall>();
            for (int i = 0; i < wall.Ladder.transform.childCount; i++)
            {
                wall.Ladder.transform.GetChild(i).gameObject.SetActive(i < player.numberOfWoodInFactory);
            }

            for(int i =0; i < wall.woodCount.Length; i++)
            {
                int count = wall.woodCount[i];
                if(count > player.numberOfWoodInFactory)
                {
                    if (i > 0)
                        player.multiplier = wall.multipliers[i - 1];
                    else
                        player.multiplier = wall.multipliers[0];
                    break;
                }
            }
        }
    }

    #endregion Collision/Trigger Functions
}
