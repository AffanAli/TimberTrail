using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateController : MonoBehaviour {
    
    [Header("References")]
    [SerializeField] TextMeshPro mul;
    [SerializeField] TextMeshPro heading;

    [Header("Gate Attributes")]
    [SerializeField] Vector2 mulRange;

    BulletGenerator generator;
    int multiplier;
    Enums.GateState currentState;
    
    #region UnityCallBacks

    private void Start() {
        generator = ReferenceManager.Instance.gun;
        multiplier = (int)Random.Range(mulRange.x, mulRange.y);
    }
    
    private void OnEnable() {
        mul.fontSharedMaterial = Instantiate(mul.fontSharedMaterial);
        heading.fontSharedMaterial = Instantiate(heading.fontSharedMaterial);
        multiplier = (int)Random.Range(mulRange.x, mulRange.y);
        mul.text = "+" + multiplier;
        transform.eulerAngles = Vector3.zero;
    }
    
    private void Update() {
      //  mul.text = "+" + multiplier;
    }

    #endregion UnityCallBacks

    #region Functionalities
    public void changeState(Enums.GateState state)
    {
        currentState = state;
        switch (currentState)
        {
            case Enums.GateState.FireRange:
                heading.text = "Fire\nRange";
                break;
            case Enums.GateState.FireRate:
                heading.text = "Fire\nRate";
                break;
        }
        heading.ForceMeshUpdate(true, true);
    }
  
    private void updateBulletAttributes()
    {
        switch (currentState)
        {
            case Enums.GateState.FireRange:
                generator.IncreaseRange(multiplier);
                break;
            case Enums.GateState.FireRate:
                generator.IncreaseRate(multiplier);
                break;
        }
    }
    #endregion Functionalities

    #region Trigger/Collision Functions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WoodBullet"))
        {
            multiplier++;
            mul.text = "+" + multiplier;
            mul.ForceMeshUpdate(true, true);
            PoolManager.Instance.BulletPool.Restore(other.gameObject);
            Vector3 newSize = new(1.08f, 1.08f, 1);
            LeanTween.scale(gameObject, newSize, 0.2f).setOnComplete(() =>
            {
               LeanTween.scale(gameObject, Vector3.one, 0.2f);
          });
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            updateBulletAttributes();
            LeanTween.rotateX(gameObject, 90, 0.2f).setOnComplete(() =>
            {
            PoolManager.Instance.GatePool.Restore(gameObject);
            });
        }
    }
    #endregion Trigger/Collision Functions
}
