using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    public enum RoadType
    {
        simple,
        factory
    };

    public enum TruckState
    {
        idle,
        collecting,
        moving
    };

    public enum HandPosition { 
    gunInHand,
    climbing
    };

    public enum Collectibles
    {
        gun,
        gem,
        sword
    };

    public enum SoundEffects { 
    LadderGrab,
    LoadWood,
    LevelFailed,
    ItemCollect
    };

    public enum GateState
    {
        FireRate,
        FireRange
    };
}
