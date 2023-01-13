using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private int _operationFactor;
    [SerializeField] private TextMesh _doorText;
    private PlayerParentBehaviour _playerBehaviour;

    private void Start()
    {
        _doorText.text = this.gameObject.name.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Player")
        {
            _playerBehaviour = other.GetComponent<PlayerParentBehaviour>();

            switch (_operations)
            {
                case Operations.Plus:
                    _playerBehaviour.PlayerIsPassAGate(_playerBehaviour.PlayerCharacterAmount + _operationFactor);
                    break;
                case Operations.Minus:
                    _playerBehaviour.PlayerIsPassAGate(_playerBehaviour.PlayerCharacterAmount - _operationFactor);
                    break;
                case Operations.Divided:                   
                    _playerBehaviour.PlayerIsPassAGate(_playerBehaviour.PlayerCharacterAmount / _operationFactor);
                    break;
                case Operations.Multiply:                    
                    _playerBehaviour.PlayerIsPassAGate(_playerBehaviour.PlayerCharacterAmount * _operationFactor);
                    break;
            }
        }
    }
}
