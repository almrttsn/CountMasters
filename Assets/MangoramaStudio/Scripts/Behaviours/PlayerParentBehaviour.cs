using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParentBehaviour : MonoBehaviour
{
    public int PlayerCount => _playerCount;
    public float PlayerBarFactor => _playerBarFactor;

    [SerializeField] private float _speed;
    [SerializeField] private CharacterBehaviour _characterBehaviourPrefab;
    [SerializeField] private CharacterBehaviour _firstCharacter;

    public List<CharacterBehaviour> _playerList = new List<CharacterBehaviour>();
    private int _playerCount = 1;
    private float _playerBarFactor;
    private float _populateRadius;
    private Vector3 _firstPosition;
    private Vector3 _secondPosition;
    private Vector3 _populateTransform;

    private void Start()
    {
        _playerList.Add(_firstCharacter);
        Debug.Log("Player Count is " + _playerCount);
    }

    public void PlayerIsPassAGate(int targetPlayerCount)
    {
        if (targetPlayerCount > _playerCount)
        {
            Debug.Log(targetPlayerCount);
            _populateRadius++;
            AddNewPlayers(targetPlayerCount - _playerCount, _populateTransform, _populateRadius);
        }
        else if(targetPlayerCount < _playerCount)
        {
            Debug.Log(targetPlayerCount);
            RemovePlayers(_playerCount - targetPlayerCount);
        }
        else
        {
            return;
        }
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
            var character = Instantiate(_characterBehaviourPrefab) as CharacterBehaviour;
            character.transform.parent = gameObject.transform;
            character.transform.position = transform.position;
            character.MoveChacartersToTerritory(spawnPos);
            _playerList.Add(character);
            /* Rotate the enemy to face towards player */
            //enemy.transform.LookAt(point);
            /* Adjust height */
            //player.transform.Translate(new Vector3(0, player.transform.localScale.y / 2, 0));
        }
        _playerCount = _playerList.Count;
    }

    private void RemovePlayers(int depopulateAmount)
    {
        for (int i = 0; i < depopulateAmount; i++)
        {
            Destroy(_playerList[_playerList.Count -1].gameObject);
            _playerList.RemoveAt(_playerList.Count - 1);
        }
        _playerCount = _playerList.Count;
    }


    private void Update()
    {
        transform.position += new Vector3(0, 0, _speed) * Time.deltaTime;

        _playerBarFactor = _playerCount / 100f;


        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Player clicked to mouse");
            _firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            //Debug.Log("Player is pushing to mouse button");
            _secondPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PlayerSwipeAtClampedAreaProcess();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-.01f, 0, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(.01f, 0, 0);
        }

        //if (_playerCountChanged == true)
        //{
        //    PopulatePlayer();
        //}

    }

    //private void PopulatePlayer()
    //{
    //    Vector3[] spawnPositions = new[] { new Vector3(0, 0, 1), new Vector3(1, 0, 1) };

    //    for (int i = 0; i < _playerCount; i++)
    //    {
    //        GameObject player = Instantiate(_playerPrefab, transform.position + spawnPositions[i], Quaternion.identity);
    //        player.transform.parent = gameObject.transform;
    //        _playerList.Add(player);
    //        //_playerCountChanged = false;
    //    }
    //}

    private void PlayerSwipeAtClampedAreaProcess()
    {
        if (Mathf.Abs(_firstPosition.x - _secondPosition.x) < 0.01f)
        {
            return;
        }
        if (_secondPosition.x > _firstPosition.x)
        {
            transform.position += new Vector3(Mathf.Abs(_firstPosition.x - _secondPosition.x) * 500f, 0, 0) * Time.deltaTime;
        }
        else if (_secondPosition.x < _firstPosition.x)
        {
            transform.position += new Vector3(-Mathf.Abs(_firstPosition.x - _secondPosition.x) * 500f, 0, 0) * Time.deltaTime;
        }
        _firstPosition = _secondPosition;
        var instantPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        float clampedPos = Mathf.Clamp(instantPosition.x, 1000f, 1000f);
        transform.position = new Vector3(clampedPos, transform.position.y, transform.position.z);
    }



}
