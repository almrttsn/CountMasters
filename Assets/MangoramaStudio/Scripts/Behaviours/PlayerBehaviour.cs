using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public int PlayerCount => _playerCount;

    [SerializeField] private float _speed;

    private int _playerCount = 1;
    private float _playerBarFactor;
    private Vector3 _firstPosition;
    private Vector3 _secondPosition;
    private DoorBehaviour _doorBehaviour;
    private GameManager _gameManager;
    private bool _playerCountChanged;

    private void Start()
    {
        _doorBehaviour.IsPlayerPassAGate += PlayerIsPassAGate;
    }

    private void OnDestroy()
    {
        _doorBehaviour.IsPlayerPassAGate -= PlayerIsPassAGate;
    }

    private void PlayerIsPassAGate(int playerCount)
    {
        _playerCount = playerCount;
        Debug.Log("playerCount is: " + _playerCount);
        _playerCountChanged = true;
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

        if (_playerCountChanged == true)
        {
            //Instantiate
            _playerCountChanged = false;
        }

    }

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
