using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionPicker : MonoBehaviour
{
    private EnemyMovement movement;
    private EnemyAttack attack;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<EnemyAttack>();
    }

    public IEnumerator RequestAction()
    {
        IEnumerator action = null;
        while (action == null)
        {
            action = attack.RequestAction();
            if (action == null)
            {
                action = movement.RequestAction();
            }
            yield return new WaitForEndOfFrame();
        }

        yield return StartCoroutine(GameManager.Instance.roundManager.BeforeEnemyActionEvent());
        yield return StartCoroutine(action);

        GameManager.Instance.roundManager.EndEnemyTurn();
        yield return null;
    }



}
