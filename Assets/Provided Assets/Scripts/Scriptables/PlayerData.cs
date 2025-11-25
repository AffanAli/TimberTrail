using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public int numberOfWoodInFactory;
    public int score;
    public int multiplier;

    public void ResetData()
    {
        numberOfWoodInFactory = 0;
        score = 0;
        multiplier = 2;
    }
    
}
