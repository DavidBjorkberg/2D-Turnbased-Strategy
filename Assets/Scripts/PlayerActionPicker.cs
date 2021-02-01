using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionPicker : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;
    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
    }
    public IEnumerator RequestAction()
    {
        IEnumerator action = null;
        while (action == null)
        {
            action = movement.RequestAction();
            yield return new WaitForEndOfFrame();
        }

        yield return StartCoroutine(GameManager.Instance.roundManager.BeforePlayerActionEvent());
        yield return StartCoroutine(action);

        GameManager.Instance.roundManager.EndPlayerTurn();
        yield return null;
    }
}
