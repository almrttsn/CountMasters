using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCountPanel : MonoBehaviour
{
    [SerializeField] private Text _playerCountText;
    [SerializeField] private PlayerParentBehaviour _playerParentBehaviour;

    private void Update()
    {
        _playerCountText.text = (" Player Count is: " + _playerParentBehaviour.PlayerCharacterAmount);
    }
}
