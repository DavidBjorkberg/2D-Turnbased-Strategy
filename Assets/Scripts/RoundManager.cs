using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private void Start()
    {
        StartPlayerTurn();
    }
    public void StartPlayerTurn()
    {
        Shader.SetGlobalFloat("playerTakingWalkInput", 1);
         StartCoroutine(GameManager.Instance.player.GetComponent<PlayerActionPicker>().RequestAction());
    }
    public IEnumerator BeforePlayerActionEvent()
    {
        Shader.SetGlobalFloat("playerTakingWalkInput", 0);
        yield return null;
    }
    public IEnumerator BeforeEnemyActionEvent()
    {
        yield return null;
    }
    public void StartEnemyTurn()
    {
        StartCoroutine(GameManager.Instance.enemyManager.ProcessEnemies());
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
        GameManager.Instance.glyphManager.ProcessGlyphs();
        StartPlayerTurn();
    }
}
