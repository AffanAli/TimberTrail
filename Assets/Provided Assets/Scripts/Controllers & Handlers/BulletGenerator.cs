using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform bulletPoint;
    [SerializeField] PlayerController player;

    [Header("Bullet attribute")]
    [SerializeField] float initialSpawnTime;
    [SerializeField] float initialRange;
    [SerializeField] float initialBulletSpeed;

    float currentSpawnTime;
    float currentRange;
    float currentBulletSpeed;

    PoolController BulletPool => PoolManager.Instance.BulletPool;

    float Timer;

    #region Unity CallBacks
    private void Start() {
        Timer = initialSpawnTime;
        currentBulletSpeed = initialBulletSpeed;
        currentRange = initialRange;
        currentSpawnTime = initialSpawnTime;
    }

    void Update() {
        if (player.isShooting) {
            CheckForGeneration();
        }

    }
    #endregion Unity CallBacks

    #region Functionalities
    private void CheckForGeneration() {
        Timer -= Time.deltaTime;
        if (Timer < 0) {
            Generate();
            Timer = currentSpawnTime;
        }
    }

    void Generate() {
        GameObject bullet = BulletPool.Get();
        BulletController controller = bullet.GetComponent<BulletController>();
        controller.enabled = true;
        controller.range = currentRange;
        controller.bulletSpeed = currentBulletSpeed;
        bullet.transform.eulerAngles = Vector3.zero;
        bullet.transform.SetParent(transform);
        bullet.transform.position = bulletPoint.position;
        bullet.transform.SetParent(null);
        bullet.transform.localScale = new(1f, 0.99999f, 1.25f);
        
        bullet.SetActive(true);
        Recoil();
    }

    public void IncreaseRange(float mul) {
        currentRange += mul / 90f;
    }

    public void IncreaseRate(float mul) {
        currentBulletSpeed += mul / 90f;
        currentSpawnTime -= mul / 900f;
    }

    void Recoil() {
        GameObject gun = player.gameObject;
        LeanTween.rotateX(gun, -3, 0.15f).setOnComplete(() => {
            LeanTween.rotateX(gun, 0, 0.15f);
        });
    }
    
    public void ResetRangeRate() {
        currentBulletSpeed = initialBulletSpeed;
        currentRange = initialRange;
        currentSpawnTime = initialSpawnTime;
    }

    #endregion Functionalities

}
