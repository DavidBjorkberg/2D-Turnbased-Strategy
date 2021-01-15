using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public EnemyActionPicker enemyTest;
    private void Start()
    {
        StartPlayerTurn();
    }
    public void StartPlayerTurn()
    {
         StartCoroutine(GameManager.Instance.player.GetComponent<PlayerActionPicker>().RequestAction());
    }
    public IEnumerator BeforePlayerActionEvent()
    {
        yield return null;
    }
    public IEnumerator BeforeEnemyActionEvent()
    {
        yield return null;
    }
    public void StartEnemyTurn()
    {
        StartCoroutine(enemyTest.RequestAction());
    }
    public void PerformPlayerTurn()
    {

    }
    public void PerformEnemyTurn()
    {

    }
    public void EndPlayerTurn()
    {
        StartEnemyTurn();
    }
    public void EndEnemyTurn()
    {
        StartPlayerTurn();
    }
}
