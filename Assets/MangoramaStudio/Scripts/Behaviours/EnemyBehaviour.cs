using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Animator _enemyAnimator;
    private bool _isStartedToMove;
    private float _lerpValue;
    private Vector3 _spawnPos;

    private void Update()
    {
        if (_isStartedToMove == true)
        {
            _lerpValue += Time.deltaTime * 2f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, _spawnPos, _lerpValue);
        }
    }

    public void MoveCharactersToTerritory(Vector3 spawnPos)
    {
        _spawnPos = spawnPos;
        _isStartedToMove = true;
    }

    internal void SetFightingState()
    {
        _enemyAnimator.SetBool("isAtCombat", true);
    }
}
