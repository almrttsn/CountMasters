using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] private PlayerParentBehaviour _playerParentBehaviour;
    private int _activeCharacterAmount;

    private void Start()
    {
        _playerParentBehaviour.IsCombatBegan += CombatHasBegan;
    }
    private void OnDestroy()
    {
        _playerParentBehaviour.IsCombatBegan -= CombatHasBegan;
    }

    private void CombatHasBegan(PlayerParentBehaviour playerParentBehaviour, EnemyParentBehaviour enemyParentBehaviour)
    {
        Debug.Log("Event arrived");
        playerParentBehaviour.MovementRestricted = true;
        var enemyListCount = enemyParentBehaviour.EnemyList.Count;

        ActiveCharactersAmountOnPlayerList();

        Debug.Log("Active character amount on player list is " + _activeCharacterAmount);

        if (_activeCharacterAmount <= enemyListCount)
        {
            for (int i = 0; i < _activeCharacterAmount; i++)
            {
                playerParentBehaviour.PlayerList[i].gameObject.SetActive(false);
                Destroy(enemyParentBehaviour.EnemyList[i].gameObject);
            }
            Debug.Log("Level Failed");
        }
        else
        {
            for (int i = 0; i < enemyListCount; i++)
            {
                playerParentBehaviour.PlayerList[i].gameObject.SetActive(false);
                Destroy(enemyParentBehaviour.EnemyList[i].gameObject);
                Debug.Log("Killing enemy");

            }
            playerParentBehaviour.MovementRestricted = false;
            playerParentBehaviour.EncounterHappened = false;
            Debug.Log("Player won the combat");
        }
        _activeCharacterAmount = 0;
        ActiveCharactersAmountOnPlayerList();
        _playerParentBehaviour.PlayerCharacterAmount = _activeCharacterAmount;
        Debug.Log("Player Count is " + _playerParentBehaviour.PlayerCharacterAmount);
    }

    private int ActiveCharactersAmountOnPlayerList()
    {
        for (int i = 0; i < _playerParentBehaviour.PlayerList.Count; i++)
        {
            if (_playerParentBehaviour.PlayerList[i].isActiveAndEnabled == true)
            {
                _activeCharacterAmount++;
            }
        }
        return _activeCharacterAmount;
    }
}
