using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MangoramaStudio.Scripts.UI
{
    public class InGamePanel : UIPanel
    {
        [SerializeField] private SettingsPanel _settingsPanel;
        [SerializeField] private PlayerCountPanel _playerCountPanel;

        public override void Initialize(UIManager uiManager)
        {
            base.Initialize(uiManager);
        }

        public void ToggleSettingsPanel()
        {
            _settingsPanel.gameObject.SetActive(!_settingsPanel.gameObject.activeSelf);
            _settingsPanel.InitializeContainers();
        }

        private void OnDestroy()
        {
            
        }
    }
}