using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Operations
{
    Plus,
    Minus,
    Divided,
    Multiply
}

public class DoorBehaviour : MonoBehaviour
{
    public Operations _operations;
    public event Action<int> IsPlayerPassAGate; 
    [SerializeField] private int _operationFactor;
    private PlayerBehaviour _playerBehaviour;
    private bool _playerPassedDoor;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _playerBehaviour = other.GetComponent<PlayerBehaviour>();

            switch (_operations)
            {
                case Operations.Plus:
                    IsPlayerPassAGate?.Invoke(_playerBehaviour.PlayerCount + _operationFactor);
                    break;
                case Operations.Minus:
                    IsPlayerPassAGate?.Invoke(_playerBehaviour.PlayerCount - _operationFactor);
                    break;
                case Operations.Divided:
                    IsPlayerPassAGate?.Invoke(_playerBehaviour.PlayerCount / _operationFactor);
                    break;
                case Operations.Multiply:
                    Debug.Log(_playerBehaviour.PlayerCount * _operationFactor);
                    IsPlayerPassAGate?.Invoke(_playerBehaviour.PlayerCount * _operationFactor);
                    break;
            }
        }
    }
}
