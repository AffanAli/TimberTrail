using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("Pools")]
    public PoolController truckPool;
    public PoolController SimpleRoadPool;
    public PoolController FactoryRoadPool;
    public PoolController BulletPool;
    public PoolController GatePool;
    public static PoolManager Instance;

    private void Awake()
    {
        Instance = this;
    }
}
