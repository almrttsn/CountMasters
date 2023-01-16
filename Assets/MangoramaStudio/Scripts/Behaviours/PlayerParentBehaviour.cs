using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParentBehaviour : MonoBehaviour
{
    public event Action<PlayerParentBehaviour, EnemyParentBehaviour> IsCombatBegan;
    public List<CharacterBehaviour> PlayerList => _playerList;

    public float PlayerBarFactor => _playerBarFactor;
    public bool MovementRestricted { get; set; }
    public bool EncounterHappened { get; set; }
    public int PlayerCharacterAmount { get; set; }
    public float CharacterSpeed { get; set; }

    [SerializeField] private CharacterBehaviour _characterBehaviourPrefab;
    [SerializeField] private CharacterBehaviour _firstCharacter;
    [SerializeField] private int _initializePopulateAmount;
    [SerializeField] private float _populateRadiusForFirstInstantiate;
    [SerializeField] private float _radiusForFirstQueue;
    [SerializeField] private float _radiusForSecondQueue;
    [SerializeField] private Transform _charactersLookVector;

    private List<CharacterBehaviour> _playerList = new List<CharacterBehaviour>();
    private float _playerBarFactor;
    private Vector3 _firstPosition;
    private Vector3 _secondPosition;
    private Vector3 _populatePosition;
    private EnemyParentBehaviour _encounteredEnemy;

    private void Start()
    {
        PlayerCharacterAmount = 1;
        _playerList.Add(_firstCharacter);
        Debug.Log("Player Count is " + PlayerCharacterAmount);
        AddNewCharacters(_initializePopulateAmount - PlayerCharacterAmount, _populatePosition, _populateRadiusForFirstInstantiate);
        for (int i = 1; i < _playerList.Count; i++)
        {
            _playerList[i].gameObject.SetActive(false);
        }
        CharacterSpeed = 5f;
    }

    #region PassingGate
    public void PlayerIsPassAGate(int targetPlayerCount)
    {
        if (targetPlayerCount > PlayerCharacterAmount)
        {
            Debug.Log("Player Count is " + targetPlayerCount + ("FromPlayerPassAGate"));
            _populateRadiusForFirstInstantiate++;
            if (targetPlayerCount >= _playerList.Count)
            {
                ActivateDesiredAmountCharacters(_playerList.Count);
                PlayerCharacterAmount = _playerList.Count;
                Debug.Log("Player character amount is " + PlayerCharacterAmount + ("FromActivatePlayersEqualiseMaxPlayer"));
            }
            else
            {
                ActivateDesiredAmountCharacters(targetPlayerCount);
                PlayerCharacterAmount = targetPlayerCount;
                Debug.Log("Player character amount is " + PlayerCharacterAmount + ("FromActivatePlayersNormalAmount"));
            }
            ReArrangeCharacterPositions();
        }
        else if (targetPlayerCount < PlayerCharacterAmount)
        {
            Debug.Log("Player Count is " + targetPlayerCount + ("FromNoChangePlayerAmount"));
            RemovePlayers(PlayerCharacterAmount - targetPlayerCount);
            ReArrangeCharacterPositions();
        }
        else
        {
            return;
        }
    }
    #endregion

    #region SettlementProcessForCharacters
    [Button]
    private void ReArrangeCharacterPositions()
    {
        List<CharacterBehaviour> firstCircleCharacters = new List<CharacterBehaviour>();
        List<CharacterBehaviour> secondCircleCharacters = new List<CharacterBehaviour>();
        for (int i = 1; i < PlayerCharacterAmount; i++)
        {
            if (firstCircleCharacters.Count < 10)
            {
                firstCircleCharacters.Add(_playerList[i]);
            }
            else
            {
                secondCircleCharacters.Add(_playerList[i]);
            }
        }
        SetCharacterPositions(firstCircleCharacters, _radiusForFirstQueue);
        SetCharacterPositions(secondCircleCharacters, _radiusForSecondQueue);
    }

    private void SetCharacterPositions(List<CharacterBehaviour> characterBehaviours, float radius)
    {
        for (int i = 0; i < characterBehaviours.Count; i++)
        {
            var radians = 2 * Mathf.PI / characterBehaviours.Count * i;
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);
            var spawnDir = new Vector3(horizontal, 0, vertical);
            var spawnPos = _populatePosition + spawnDir * radius * 3f;
            characterBehaviours[i].MoveCharactersToTerritory(spawnPos,transform);
        }
    }
    #endregion

    #region CharactersAddRemoveProcess
    public void AddNewCharacters(float num, Vector3 point, float radius)
    {
        for (int i = 0; i < num; i++)
        {
            var radians = 2 * Mathf.PI / num * i;
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);
            var spawnDir = new Vector3(horizontal, 0, vertical);
            var spawnPos = point + spawnDir * radius * 3f;
            var character = Instantiate(_characterBehaviourPrefab) as CharacterBehaviour;
            character.transform.parent = gameObject.transform;
            character.transform.position = transform.position;
            character.MoveCharactersToTerritory(spawnPos,transform);
            _playerList.Add(character);
        }
    }

    private void RemovePlayers(int depopulateAmount)
    {
        for (int i = PlayerCharacterAmount - 1; i >= PlayerCharacterAmount - depopulateAmount; i--)
        {
            _playerList[i].gameObject.SetActive(false);
        }
        PlayerCharacterAmount = PlayerCharacterAmount - depopulateAmount;
    }
    #endregion

    private void ActivateDesiredAmountCharacters(int targetPlayerCount)
    {
        for (int i = 0; i < targetPlayerCount; i++)
        {
            _playerList[i].gameObject.SetActive(true);
        }
        PlayerCharacterAmount = targetPlayerCount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && EncounterHappened == false)
        {
            EncounterHappened = true;
            Debug.Log("Combat began");
            _encounteredEnemy = other.GetComponent<EnemyParentBehaviour>();
            IsCombatBegan?.Invoke(this, _encounteredEnemy);
        }
    }

    private void Update()
    {
        transform.position += new Vector3(0, 0, CharacterSpeed) * Time.deltaTime;
        //if (MovementRestricted == false)
        //{
        //}
        _playerBarFactor = PlayerCharacterAmount / 10f;

        if (Input.GetMouseButtonDown(0))
        {
            _firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            _secondPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PlayerSwipeAtClampedAreaProcess();
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
