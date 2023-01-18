using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private float _radiusForFirstQueue;
    [SerializeField] private float _radiusForSecondQueue;
    [SerializeField] private CapsuleCollider _playerParentCapsuleCollider;
    

    private List<CharacterBehaviour> _playerList = new List<CharacterBehaviour>();
    private float _playerBarFactor;
    private Vector3 _firstPosition;
    private Vector3 _secondPosition;
    private Vector3 _populatePosition;
    private EnemyParentBehaviour _encounteredEnemy;

    private void Initialize()
    {
        PlayerCharacterAmount = 1;
        _playerList.Add(_firstCharacter);
        Debug.Log("Player Count is " + PlayerCharacterAmount);
        CreateCharacterPool(_initializePopulateAmount - PlayerCharacterAmount);
        for (int i = 1; i < _playerList.Count; i++)
        {
            _playerList[i].gameObject.SetActive(false);
        }
        CharacterSpeed = 5f;
        InputController.OnDrag += Dragged;
        
    }
    #region PassingGate
    public void PlayerIsPassAGate(int targetPlayerCount)
    {
        float clampedPlayerCircleFactor = Mathf.Clamp((float)(targetPlayerCount / 10f),0.5f,2f);
        _playerParentCapsuleCollider.radius = 0.5f + clampedPlayerCircleFactor;
        if (targetPlayerCount > PlayerCharacterAmount)
        {
            Debug.Log("Player Count is " + targetPlayerCount + ("FromPlayerPassAGate"));
            if (targetPlayerCount >= _playerList.Count)
            {
                ActivateCharacters(_playerList.Count);
                PlayerCharacterAmount = _playerList.Count;
                Debug.Log("Player character amount is " + PlayerCharacterAmount + ("FromActivatePlayersEqualiseMaxPlayer"));
            }
            else
            {
                ActivateCharacters(targetPlayerCount);
                PlayerCharacterAmount = targetPlayerCount;
                Debug.Log("Player character amount is " + PlayerCharacterAmount + ("FromActivatePlayersNormalAmount"));
            }
            ReArrangeCharacterPositions();
        }
        else if (targetPlayerCount < PlayerCharacterAmount)
        {
            Debug.Log("Player Count is " + targetPlayerCount + ("FromNoChangePlayerAmount"));
            DeactivatePlayers(PlayerCharacterAmount - targetPlayerCount);
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

    internal void SetAllCharactersToRunningState()
    {
        foreach (var character in _playerList)
        {
            character.SetRunningState();
        }
    }

    internal void SetAllCharactersToFightingState()
    {
        foreach (var character in _playerList)
        {
            character.SetFightingState();
        }
    }

    private void SetCharacterPositions(List<CharacterBehaviour> characterBehaviours, float radius)
    {
        for (int i = 0; i < characterBehaviours.Count; i++)
        {
            var radians = 2 * Mathf.PI / characterBehaviours.Count * i;
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);
            var spawnDir = new Vector3(horizontal, 0, vertical);
            var spawnPos = (_populatePosition + spawnDir) * radius;
            characterBehaviours[i].MoveCharactersToTerritory(spawnPos,_firstCharacter.transform);
        }
    }
    #endregion

    #region CharactersAddRemoveProcess
    public void CreateCharacterPool(float num)
    {
        for (int i = 0; i < num; i++)
        {
            
            var character = Instantiate(_characterBehaviourPrefab) as CharacterBehaviour;
            character.transform.parent = gameObject.transform;
            character.transform.position = transform.position;
            _playerList.Add(character);
        }
    }

    private void DeactivatePlayers(int depopulateAmount)
    {
        for (int i = PlayerCharacterAmount - 1; i >= PlayerCharacterAmount - depopulateAmount; i--)
        {
            _playerList[i].gameObject.SetActive(false);
        }
        PlayerCharacterAmount = PlayerCharacterAmount - depopulateAmount;
    }
    #endregion

    private void ActivateCharacters(int targetPlayerCount)
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
        else if(other.tag == "LevelEnd")
        {
            //level complete
        }
    }

    private void Update()
    {
        transform.position += new Vector3(0, 0, CharacterSpeed) * Time.deltaTime;        
        _playerBarFactor = PlayerCharacterAmount / 100f;
    }

    private void Dragged(Vector2 dragVector)
    {
        transform.position += new Vector3(dragVector.x * 100f, 0, 0) * Time.deltaTime;
    }
}
