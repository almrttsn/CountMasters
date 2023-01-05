using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCountBar : MonoBehaviour
{
    [SerializeField] private Image _playercountBarImage;
    [SerializeField] private PlayerBehaviour _playerBehaviour;

    private void Update()
    {
        _playercountBarImage.fillAmount = Mathf.Lerp(_playercountBarImage.fillAmount, _playerBehaviour.PlayerBarFactor, 2f * Time.deltaTime);
    }
}
