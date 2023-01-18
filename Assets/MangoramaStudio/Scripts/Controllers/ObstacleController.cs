using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private List<EnemyParentBehaviour> _enemyParentBehaviourList;
    private List<DoorBehaviour> _doorBehaviourList;

    private void Initialize()
    {
        _enemyParentBehaviourList = FindObjectsOfType<EnemyParentBehaviour>().ToList();
        _doorBehaviourList = FindObjectsOfType<DoorBehaviour>().ToList();
        for (int i = 0; i < _enemyParentBehaviourList.Count; i++)
        {
            _enemyParentBehaviourList[i].Initialize();
        }
        for (int i = 0; i < _doorBehaviourList.Count; i++)
        {
            _doorBehaviourList[i].Initialize();
        }
    }
}
