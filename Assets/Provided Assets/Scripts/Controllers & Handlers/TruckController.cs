using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject[] bulletPoints;
    [SerializeField] GameObject[] Doors;
    [SerializeField] PlayerData player;
    [SerializeField] AudioSource movingSound;
    [SerializeField] AudioSource engineSound;
    public GameObject woodList;

    [Header("Objects Speed")]
    [SerializeField] float doorMovementSpeed;
    [SerializeField] float truckSpeed;

    FactoryController Factory;
    Enums.TruckState currentState;
    Vector3 wayPoint;
    bool isMoving;
    bool isRotated;
    bool isReadyToRemove;
    bool processing;
    float Timer = 2f;
    int numberOfBullets;

    #region Unity Callbacks
    private void OnEnable()
    {
        isMoving = false;
        isRotated = false;
        isReadyToRemove = false;
        processing = false;
        numberOfBullets = 0;
        ChangeState(Enums.TruckState.collecting);
        wayPoint = new(-0.032f, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        CheckForMovement();
        CheckForRemovingTruck();
    }
    #endregion Unity Callbacks

    #region Functionalities
    private void CheckForRemovingTruck()
    {
        if (isReadyToRemove)
        {
            Timer -= Time.deltaTime;
            if (Timer < 0)
            {
                isReadyToRemove = false;
                numberOfBullets = 0;
                PoolManager.Instance.truckPool.Restore(gameObject);
            }
        }
    }
    void CheckForMovement()
    {
        if (currentState == Enums.TruckState.moving)
        {   
            Movement();
        }
    }
    public void ChangeState(Enums.TruckState state)
    { 
        switch (state)
        {
            case Enums.TruckState.moving:
                TruckMovement();
                OpenDoors(false);
                break;

            case Enums.TruckState.collecting:
                SoundManager.Instance.Stop(movingSound);
                currentState = state;
                OpenDoors(true);
                break;

            case Enums.TruckState.idle:
                SoundManager.Instance.Stop(movingSound);
                currentState = state;
                OpenDoors(true);
                break;
        }
        
    }
    void TruckMovement()
    {
        SoundManager.Instance.Play(engineSound);
        StartCoroutine(PlayAfterEngine(0.2f));
    }
    IEnumerator PlayAfterEngine(float startTime)
    {
        yield return new WaitForSeconds(startTime);

        currentState = Enums.TruckState.moving;
        SoundManager.Instance.Play(movingSound);
    }
    private void OpenDoors(bool open)
    {
        float scaleY = open ? 0f : 1f;
        for (int i = 0; i < Doors.Length; i++)
        {
            LeanTween.scaleY(Doors[i], scaleY, doorMovementSpeed);
        }
    }

    private void Movement()
    {
        if (Vector3.Distance(transform.position, wayPoint) < .232f && !isRotated)
        {
            LeanTween.rotateY(gameObject, 180, 0.2f).setOnComplete(() =>
            {
                isRotated = true;
            });
        }
        if (isRotated)
        {
            transform.position += Vector3.forward * Time.deltaTime * truckSpeed;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPoint, Time.deltaTime * truckSpeed);
        }
    }

    public void StoreBullet(Collider other)
    {
        if (other.gameObject.CompareTag("WoodBullet") && other.gameObject.transform.parent == null)
        {
            if (!processing && (numberOfBullets < bulletPoints.Length || currentState == Enums.TruckState.moving))
            {
            processing = true;
                other.gameObject.GetComponent<BulletController>().enabled = false;
                other.transform.SetParent(woodList.transform);
                other.transform.position = bulletPoints[numberOfBullets].transform.position;
                numberOfBullets++;
                player.score++;
                SoundManager.Instance.Play(Enums.SoundEffects.LoadWood);
                if (numberOfBullets >= bulletPoints.Length)
                {
                    ChangeState(Enums.TruckState.moving);
                }
                processing = false;
            }
            else
            {
                PoolManager.Instance.BulletPool.Restore(other.gameObject);
            }
        }
    }

    public void StopTruck(Collision collision)
    {
        if (collision.gameObject.CompareTag("TruckStop"))
        {
            ChangeState(Enums.TruckState.idle);
            isReadyToRemove = true;
        }
    }


    #endregion Functionalities

    #region Collision/Trigger Functions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isMoving)
        {
            isMoving = true;
            ChangeState(Enums.TruckState.moving);
        }
        if (other.gameObject.CompareTag("Factory"))
        {
            Factory = other.gameObject.GetComponent<FactoryController>();
            GameObject incline = Factory.transform.parent.gameObject;
            gameObject.transform.SetParent(incline.transform);
        }
    }
    #endregion Collision/Trigger Functions
}
