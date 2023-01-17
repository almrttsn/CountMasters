using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParentBehaviour : MonoBehaviour
{
    public List<EnemyBehaviour> EnemyList => _enemyList;

    [SerializeField] private int _enemyCount;
    [SerializeField] private EnemyBehaviour _enemyCharacter;

    private List<EnemyBehaviour> _enemyList = new List<EnemyBehaviour>();    
    private Vector3 _populatePosition;

    private void Start()
    {
        AddNewPlayers(_enemyCount -1, _populatePosition, 1.5f);
        _enemyList.Add(_enemyCharacter);
    }

    public void AddNewPlayers(float num, Vector3 point, float radius)
    {
        for (int i = 0; i < num; i++)
        {
            var radians = 2 * Mathf.PI / num * i;
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);
            var spawnDir = new Vector3(horizontal, 0, vertical);
            var spawnPos = point + spawnDir * radius;
            var character = Instantiate(_enemyCharacter) as EnemyBehaviour;
            character.transform.parent = gameObject.transform;
            character.transform.position = transform.position;
            character.MoveCharactersToTerritory(spawnPos);
            _enemyList.Add(character);
        }
    }

    internal void SetAllCharactersToFightingState()
    {
        foreach (var enemy in _enemyList)
        {
            enemy.SetFightingState();
        }
    }
}
