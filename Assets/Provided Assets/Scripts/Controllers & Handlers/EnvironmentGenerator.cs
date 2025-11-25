using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{

    PoolController RoadsPool => PoolManager.Instance.SimpleRoadPool;
    PoolController FactoryRoadPool => PoolManager.Instance.FactoryRoadPool;
    PoolController TruckPool => PoolManager.Instance.truckPool;
    PoolController Gate => PoolManager.Instance.GatePool;

    [Header("References")]
    public GameObject EndingWall;
    public GameObject Player;


    [Header("Level Data")]
    public LevelPlacementData[] Level;

    public List<Transform> roads = new();
    public int currentLevel = 0;

    #region Unity Callbacks
    private void Start()
    {
        roads.Clear();
        //GenerateEnvironment();
    }

    void Update()
    {
    }
    #endregion Unity CallBacks


    #region Functionalities
    public void GenerateEnvironment(int levelNumber = 0)
    {
        currentLevel = levelNumber;
        Vector3 position = new(0, 0, 0);

        for (int i = 0; i < Level[levelNumber].placements.Length; i++)
        {
            switch (Level[levelNumber].placements[i].type)
            {
                case Enums.RoadType.simple:
                    position = PlaceSimpleRoad(position, i, levelNumber);
                    break;
                case Enums.RoadType.factory:
                    position = PlaceFactoryRoad(position);
                    break;
            }
        }

        // Activate Ending Wall
        EndingWall.transform.position = position;
        EndingWall.SetActive(true);
    }

    private Vector3 PlaceFactoryRoad(Vector3 position)
    {
        GameObject factoryRoad = FactoryRoadPool.Get();
        FactoryRoad factoryRoadComp = factoryRoad.GetComponent<FactoryRoad>();
        factoryRoad.transform.position = position;
        position = factoryRoadComp.end.position;
        factoryRoad.SetActive(true);
        roads.Add(factoryRoad.transform);
        return position;
    }

    private Vector3 PlaceSimpleRoad(Vector3 position, int i, int levelNumber)
    {
        GameObject road = RoadsPool.Get();
        road.transform.position = position;
        road.SetActive(true);
        position = road.GetComponent<SimpleRoad>().end.position;

        if (Level[levelNumber].placements[i].truckPlacement)
        {
            PlaceTruck(i, levelNumber, road);
        }
        if (Level[levelNumber].placements[i].fireRange)
        {
            GameObject gate = Gate.Get();
            gate.GetComponent<GateController>().changeState(Enums.GateState.FireRange);
            float gatePos = Level[levelNumber].placements[i].rangePos;
            PlaceGate(road, gate, gatePos);
        }
        if (Level[levelNumber].placements[i].fireRate)
        {
            GameObject gate = Gate.Get();
            gate.GetComponent<GateController>().changeState(Enums.GateState.FireRate);
            float gatePos = Level[levelNumber].placements[i].ratePos;
            PlaceGate(road, gate, gatePos);
        }


        roads.Add(road.transform);
        return position;
    }

    private void PlaceGate(GameObject road, GameObject gate, float gatePos)
    {
        Vector3 roadPos = road.transform.position;
        gate.transform.SetParent(road.transform);
        gate.transform.position = new Vector3(gatePos, roadPos.y, roadPos.z);
        gate.SetActive(true);
    }

    private void PlaceTruck(int i, int levelNumber, GameObject road)
    {
        GameObject truck = TruckPool.Get();
        Vector3 roadPos = road.transform.position;
        float truckPosX = Level[levelNumber].placements[i].truckpos;
        truck.transform.eulerAngles = new(0, 90, 0);
        truck.transform.SetParent(road.transform);
        truck.transform.position = new Vector3(truckPosX, roadPos.y, roadPos.z);
        truck.SetActive(true);
    }

    public void ResetEnvironment()
    {
        RemoveEnvironment();
        GenerateEnvironment(currentLevel);
    }

    public void RemoveEnvironment()
    {
        RoadsPool.RestoreAll();
        FactoryRoadPool.RestoreAll();
        TruckPool.RestoreAll();
        Gate.RestoreAll();
        roads.Clear();
    }
    #endregion Functionalities
}


