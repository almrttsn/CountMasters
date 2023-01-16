using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterBehaviour : MonoBehaviour
{

    [SerializeField] private NavMeshAgent _navMeshAgent;
    private Vector3 _spawnPos;
    private bool _isStartedToMove;
    private Transform _playerParentTransform;

    private void Update()
    {
        if (_isStartedToMove == true)
        {
            //_lerpValue += Time.deltaTime * 2f;
            //transform.localPosition = Vector3.Lerp(transform.localPosition, _spawnPos, _lerpValue);
            _navMeshAgent.SetDestination(_playerParentTransform.position + _spawnPos);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, _playerParentTransform.position + _spawnPos);
    }

    public void MoveCharactersToTerritory(Vector3 spawnPos, Transform playerParentTransform)
    {
        _playerParentTransform = playerParentTransform;
        _spawnPos = spawnPos;
        _isStartedToMove = true;
    }
}
