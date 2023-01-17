using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] private PlayerParentBehaviour _playerParentBehaviour;

    private int _activeCharacterAmount;
    private bool _combatFinished;

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
        StartCoroutine(CombatTimeCo());
        var enemyListCount = enemyParentBehaviour.EnemyList.Count;

        ActiveCharactersAmountOnPlayerList();

        Debug.Log("Active character amount on player list is " + _activeCharacterAmount + ("FromAfterFirstAmountCalculation"));

        if (_activeCharacterAmount <= enemyListCount)
        {
            EnemyBeatPlayer(playerParentBehaviour, enemyParentBehaviour);
        }
        else
        {
            PlayerBeatEnemy(playerParentBehaviour, enemyParentBehaviour, enemyListCount);
        }
        ActiveCharactersAmountOnPlayerList();
        _playerParentBehaviour.PlayerCharacterAmount = _activeCharacterAmount;
        Debug.Log("Player Count is " + _playerParentBehaviour.PlayerCharacterAmount + ("FromAfterLastCombat"));
    }

    #region CombatConsequences
    private void PlayerBeatEnemy(PlayerParentBehaviour playerParentBehaviour, EnemyParentBehaviour enemyParentBehaviour, int enemyListCount)
    {
        for (int i = _activeCharacterAmount - 1; i >= _activeCharacterAmount - enemyListCount; i--)
        {
            playerParentBehaviour.PlayerList[i].gameObject.SetActive(false);
            
            var destroyEnemy = enemyParentBehaviour.EnemyList[0];
            enemyParentBehaviour.EnemyList.RemoveAt(0);
            Destroy(destroyEnemy.gameObject);
            
            Debug.Log("Killing enemy");
        }
        playerParentBehaviour.MovementRestricted = false;
        playerParentBehaviour.EncounterHappened = false;
        Debug.Log("Player won the combat");
    }

    private void EnemyBeatPlayer(PlayerParentBehaviour playerParentBehaviour, EnemyParentBehaviour enemyParentBehaviour)
    {
        for (int i = 0; i < _activeCharacterAmount; i++)
        {
            playerParentBehaviour.PlayerList[i].gameObject.SetActive(false);
            Destroy(enemyParentBehaviour.EnemyList[i].gameObject);
        }
        Debug.Log("Level Failed");
    }
    #endregion

    private void ActiveCharactersAmountOnPlayerList()
    {
        _activeCharacterAmount = 0;
        for (int i = 0; i < _playerParentBehaviour.PlayerList.Count; i++)
        {
            if (_playerParentBehaviour.PlayerList[i].isActiveAndEnabled == true)
            {
                _activeCharacterAmount++;
            }
        }
    }

    private IEnumerator CombatTimeCo()
    {
        _playerParentBehaviour.CharacterSpeed = 0;
        yield return new WaitForSeconds(1f);
        _combatFinished = true;
        _playerParentBehaviour.CharacterSpeed = 5f;

    }
}
