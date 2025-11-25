using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    public static ReferenceManager Instance;
    private void Start()
    {
        Instance = this;
    }

    public BulletGenerator gun;
    public PlayerController player;
    public GameObject claimText;
}
