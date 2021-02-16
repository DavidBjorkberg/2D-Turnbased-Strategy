using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    enum RoundState
    {
        StartPlayerTurn, BeforePlayerActionEvent, BeforeEnemyActionEvent, StartEnemyTurn, EndPlayerTurn, EndEnemyTurn, None
    };
    RoundState roundState;
    struct StartEnemyTurnData
    {
        internal bool startedProcessingGlyphs;
    }
    StartEnemyTurnData startEnemyTurnData;
    struct EndEnemyTurnData
    {
        internal bool startedProcessingGlyphs;
    }
    EndEnemyTurnData endEnemyTurnData;
    Coroutine playerRequestAction;
    void Update()
    {
        switch (roundState)
        {
            case RoundState.StartPlayerTurn:
                break;
            case RoundState.BeforePlayerActionEvent:
                break;
            case RoundState.BeforeEnemyActionEvent:
                break;
            case RoundState.StartEnemyTurn:
                StartEnemyTurn();
                break;
            case RoundState.EndPlayerTurn:
                break;
            case RoundState.EndEnemyTurn:
                EndEnemyTurn();
                break;
            default:
                break;
        }
    }
    public void StartPlayerTurn()
    {
        roundState = RoundState.StartPlayerTurn;

        Shader.SetGlobalFloat("playerTakingWalkInput", 1);
        if(playerRequestAction != null)
        {
            StopCoroutine(playerRequestAction);
        }
        playerRequestAction = StartCoroutine(GameManager.Instance.player.GetComponent<PlayerActionPicker>().RequestAction());
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
        roundState = RoundState.StartEnemyTurn;
        if (!startEnemyTurnData.startedProcessingGlyphs)
        {
            GameManager.Instance.glyphManager.startProcessingGlyphs(false);
            startEnemyTurnData.startedProcessingGlyphs = true;
        }
        else if (!GameManager.Instance.glyphManager.processGlyphsData.isProcessing)
        {
            GameManager.Instance.enemyManager.ProcessDeadEnemies();
            StartCoroutine(GameManager.Instance.enemyManager.ProcessEnemies());
            roundState = RoundState.None;
            startEnemyTurnData.startedProcessingGlyphs = false;
        }
    }
    public void EndPlayerTurn()
    {
        StartEnemyTurn();
    }
    public void EndEnemyTurn()
    {
        roundState = RoundState.EndEnemyTurn;
        if (!endEnemyTurnData.startedProcessingGlyphs)
        {
            GameManager.Instance.glyphManager.startProcessingGlyphs(true);
            endEnemyTurnData.startedProcessingGlyphs = true;
        }
        else if (!GameManager.Instance.glyphManager.processGlyphsData.isProcessing)
        {
            GameManager.Instance.enemyManager.ProcessDeadEnemies();
            StartPlayerTurn();

            endEnemyTurnData.startedProcessingGlyphs = false;
        }
    }
    public void SwitchRoom()
    {
        StartPlayerTurn();
    }
}
