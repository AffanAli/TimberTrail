using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [HideInInspector] public float bulletSpeed;
    [HideInInspector]  public float range;
    float distance;


    #region Unity CallBacks
    private void OnEnable()
    {
        distance = transform.position.z + range;
    }

    void Update()
    {
        transform.position += Vector3.forward * bulletSpeed * Time.deltaTime;
        CheckRange();
    }

    #endregion Unity Callbacks
    
    #region Functionalities
    private void CheckRange()
    {
        if (transform.position.z > distance)
        {
            PoolManager.Instance.BulletPool.Restore(gameObject);
        }
    }
    
    #endregion Functionalities
    
}
