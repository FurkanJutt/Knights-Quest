using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    public Player player;

    public GameObject EnemyGroup1;
    public GameObject EnemyGroup2;
    public int enemyGroup1Count = 4;
    public int enemyGroup2Count = 4;

    public Transform GameWin;
    public Transform GameOver;

    // Start is called before the first frame update
    void Start()
    {
        EnemyGroup2.SetActive(false);
        GameWin.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (enemyGroup1Count == 0)
        {
            EnemyGroup1.SetActive(false);
            EnemyGroup2.SetActive(true);
            enemyGroup1Count = -1;
        }
        if(enemyGroup2Count == 0)
        {
            EnemyGroup1.SetActive(false);
            EnemyGroup2.SetActive(false);
            enemyGroup2Count = -1;
            GameWon();
        }
    }

    public void GameWon()
    {
        player.enabled = false;
        GameWin.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        GameWin.DOScale(0.83207f, .2f).SetEase(Ease.Flash);
    }

    public void GameLost()
    {
        player.enabled = false;
        GameOver.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        GameOver.DOScale(0.83207f, .2f).SetEase(Ease.Flash);
    }
}
