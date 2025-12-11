using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FactoryController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject Door;
    [SerializeField] TextMeshPro woodCount;
    [SerializeField] FactoryRoad FactoryRoad;

    [Header("Factory Attributes")]
    [SerializeField] PlayerData player;
    [SerializeField] float doorMovementSpeed;

    // Private Fields
    Vector3 doorOpenScale = new(1, 1, 0);
    
    bool isDoorOpen;


    #region Unity CallBacks
    private void OnEnable()
    {
        isDoorOpen = false;
        player.ResetData();
    }

    private void Start()
    {
      
    }

    private void Update()
    {
        // Wood Count Text
        woodCount.text = "" + player.numberOfWoodInFactory;

        UpdateDoor();

    }
    #endregion Unity CallBacks

    #region Functionalities


    private void UpdateDoor()
    {
        if (isDoorOpen)
        {
            OpenDoor(true);
        }
        else
        {
            OpenDoor(false);
        }
    }
    private void OpenDoor(bool open)
    {
        Vector3 newScale = open ? doorOpenScale : Vector3.one;
        if (Vector3.Distance(Door.transform.localScale, newScale) > 0)
        {
            Door.transform.localScale = Vector3.MoveTowards(Door.transform.localScale, newScale, Time.deltaTime * doorMovementSpeed);
        }
    }

    public void TakeWoodsFromTruck(Collision truck)
    {
        if (truck.gameObject.CompareTag("Truck"))
        {
            isDoorOpen = false;

            GameObject temp = truck.gameObject.GetComponentInParent<TruckController>().woodList;

            while (temp.transform.childCount > 0)
            {
                Transform wood = temp.transform.GetChild(0);
                player.numberOfWoodInFactory++;
                PoolManager.Instance.TimberBulletPool.Restore(wood.gameObject);
            }
        }
    }

    public void ResetFactory()
    {
        isDoorOpen = false;
    }

    #endregion Functionalities

    #region Collision/Trigger Functions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Truck"))
        {
            isDoorOpen = true;
        }
    }

    #endregion Collision/Trigger Functions
}
