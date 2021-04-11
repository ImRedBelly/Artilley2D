using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float timeMove;
    public Text timer;
    public float timerText;

    List<EnemyMovement> enemys = new List<EnemyMovement>();
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        StartCoroutine(Queue());
    }

    void Update()
    {
        timerText -= Time.deltaTime;
        timer.text = "End of Turn: " + Math.Round(timerText);
    }

    IEnumerator Queue()
    {
        timerText = timeMove * 2;
        PlayerMovement.instance.SetImMain(true);
        yield return new WaitForSeconds(timeMove * 2);
        PlayerMovement.instance.SetImMain(false);

        for (int i = 0; i < enemys.Count; i++)
        {
            timerText = timeMove;
            enemys[i].SetImMain(true);
            yield return new WaitForSeconds(timeMove);
            enemys[i].SetImMain(false);
        }

        StartCoroutine(Queue());
    }
    public void AddEnemy(EnemyMovement enemy)
    {
        enemys.Add(enemy);
    }

    public void RemoveEnemy(EnemyMovement enemy)
    {
        enemys.Remove(enemy);
        StopAllCoroutines();
        StartCoroutine(Queue());
    }

}
