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
        action = attack.RequestAction();
        if (action == null)
        {
            action = movement.RequestAction();
        }

        yield return StartCoroutine(GameManager.Instance.roundManager.BeforeEnemyActionEvent());
        yield return StartCoroutine(action);

        yield return null;
    }



}
