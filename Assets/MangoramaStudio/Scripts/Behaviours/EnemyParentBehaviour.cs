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
    private bool _isStartedToMove;
    private Vector3 _spawnPos;
    private Vector3 _populatePosition;


    private void Start()
    {
        AddNewPlayers(_enemyCount, _populatePosition, 1.5f);
        _enemyList.Add(_enemyCharacter);
    }

    public void AddNewPlayers(float num, Vector3 point, float radius)
    {

        for (int i = 0; i < num; i++)
        {
            /* Distance around the circle */
            var radians = 2 * Mathf.PI / num * i;
            /* Get the vector direction */
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);
            var spawnDir = new Vector3(horizontal, 0, vertical);
            /* Get the spawn position */
            var spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point
            /* Now spawn */
            var character = Instantiate(_enemyCharacter) as EnemyBehaviour;
            character.transform.parent = gameObject.transform;
            character.transform.position = transform.position;
            character.MoveCharactersToTerritory(spawnPos);
            _enemyList.Add(character);
            /* Rotate the enemy to face towards player */
            //enemy.transform.LookAt(point);
            /* Adjust height */
            //player.transform.Translate(new Vector3(0, player.transform.localScale.y / 2, 0));
        }
        //_playerCount = _playerList.Count;
    }

    
}
