using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableChecker : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("Enabled " + gameObject.name);
    }
    private void OnDisable()
    {
        Debug.Log("Disabled " + gameObject.name);
    }
}
