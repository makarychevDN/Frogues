using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FroguesFramework
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private TMP_Text descriptionOfExpiriencedToadButton;
        [SerializeField] private TMP_Text ascensionCountLabel;
        [SerializeField] private int currentAscensionIndex;
        [SerializeField] private List<AscensionSetup> availableAscensionsForExpirensedToadMode;
        [SerializeField] private List<AscensionSetup> ascensionsForExpirensedToadMode;
        [SerializeField] private AscensionSetupContainer runWithAscenstionContainer;
        [SerializeField] private MaxAvailableAscensionSaveManager maxAvailableAscensionSaveManager;
        [SerializeField] private GameObject globalMask;
        [SerializeField] private GameObject columnOfMainMenu;
        [SerializeField] private GameObject pressAnyKeyLabel;
        [SerializeField] private GameObject ascensionSelectionPanel;
        private bool _anyKeyPressed;

        private void Awake()
        {
            columnOfMainMenu.SetActive(false);
            pressAnyKeyLabel.SetActive(true);
            globalMask.SetActive(true);

            maxAvailableAscensionSaveManager.TryToLoadInfo();
            ascensionSelectionPanel.SetActive(MaxAvailavleAscension.indexOfMaxAbailableAscension > 0);

            for (int i = 0; i < MaxAvailavleAscension.indexOfMaxAbailableAscension + 1; i++)
            {
                availableAscensionsForExpirensedToadMode.Add(ascensionsForExpirensedToadMode[i]);
            }
        }

        public void UpdateDescriptionOfAscentionByAscensionContainer(AscensionSetupContainer ascensionSetupContainer)
        {
            descriptionOfExpiriencedToadButton.text = ascensionSetupContainer.AscensionSetup.Description.GetLocalizedString();
        }

        public void IncreaseCurrentAscentionIndex(int value)
        {
            currentAscensionIndex += value;
            currentAscensionIndex = Mathf.Clamp(currentAscensionIndex, 0, availableAscensionsForExpirensedToadMode.Count - 1);
            ascensionCountLabel.text = $"{currentAscensionIndex} / 7";
            runWithAscenstionContainer.AscensionSetup = availableAscensionsForExpirensedToadMode[currentAscensionIndex];
            UpdateDescriptionOfAscentionByAscensionContainer(runWithAscenstionContainer);
        }

        public void SetCurrentAscension(AscensionSetupContainer ascensionSetupContainer)
        {
            CurrentAscention.ascensionSetup = ascensionSetupContainer.AscensionSetup;
        }

        public void StartGame(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        private void Update()
        {
            if (_anyKeyPressed)
                return;

            if (Input.anyKeyDown)
            {
                _anyKeyPressed = true;
                pressAnyKeyLabel.SetActive(false);
                columnOfMainMenu.SetActive(true);
            }
        }
    }
}