using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{

    internal bool isPlayerPickingAction;
    private void Start()
    {
        StartPlayerTurn();
    }
    public void StartPlayerTurn()
    {
         StartCoroutine(GameManager.Instance.player.GetComponent<PlayerActionPicker>().RequestAction());
    }
    public void StartEnemyTurn()
    {

    }
    public void PerformPlayerTurn()
    {

    }
    public void PerformEnemyTurn()
    {

    }
    public void EndPlayerTurn()
    {

    }
    public void EndEnemyTurn()
    {

    }
}
