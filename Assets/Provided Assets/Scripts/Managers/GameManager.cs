using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] EnvironmentGenerator environmentGenerator;
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject gameOver;
    [SerializeField] TextMeshProUGUI gameScore;
    [SerializeField] BulletGenerator bulletAttributes;

    public static GameManager Instance;
    public int currentLevel;

    private void Awake()
    {
        Instance = this;
        currentLevel = 1;
    }

    private void Start()
    {
        startButton.SetActive(true);
        SelectLevel(0);
    }

    private void ResetLevel()
    {
        ReferenceManager.Instance.player.ResetPlayer();
        playerData.ResetData();
        environmentGenerator.ResetEnvironment();
        PoolManager.Instance.BulletPool.RestoreAll();
        bulletAttributes.ResetRangeRate();
    }

    public void SelectLevel(int levelNumber)
    {
        currentLevel = levelNumber + 1;
        playerData.ResetData();
        environmentGenerator.RemoveEnvironment();
        environmentGenerator.GenerateEnvironment(levelNumber);
    }

    public void OnClickStartButton()
    {
        ReferenceManager.Instance.player.isShooting = true;
        ReferenceManager.Instance.player.GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePosition;
        startButton.SetActive(false);
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        int totalScore = (playerData.score * playerData.multiplier);
        gameScore.text = "" + totalScore;
    }
    public void OnClickHomeButton()
    {
        ReferenceManager.Instance.player.isShooting = false;
        ReferenceManager.Instance.player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        ResetLevel();
        SelectLevel(0);
        startButton.SetActive(true);
        gameOver.SetActive(false);
    }
    public void OnClickReplay()
    {
        ReferenceManager.Instance.player.isShooting = false;
        ReferenceManager.Instance.player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        ResetLevel();
        SelectLevel(currentLevel - 1);
        startButton.SetActive(true);
        gameOver.SetActive(false);
    }

    public void OnClickBackButton()
    {
        OnClickHomeButton();
    }
}
