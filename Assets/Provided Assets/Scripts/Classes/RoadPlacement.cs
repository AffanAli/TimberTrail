using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadPlacement
{
    public Enums.RoadType type;
    public bool truckPlacement;
    [Range(0.2127f, 0.74f)] public float truckpos = 0.35f;
    public bool fireRange;
    [Range(0.272f, 0.61f)] public float rangePos = 0.35f;
    public bool fireRate;
    [Range(0.272f, 0.61f)] public float ratePos = 0.35f;
}

