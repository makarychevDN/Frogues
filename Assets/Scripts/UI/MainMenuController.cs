using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FroguesFramework
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text ascensionCountLabel;
        [SerializeField] private int currentAscensionIndex;
        [SerializeField] private List<AscensionSetup> availableAscensionsForExpirensedToadMode;
        [SerializeField] private List<AscensionSetup> ascensionsForExpirensedToadMode;
        [SerializeField] private AscensionSetupContainer runWithAscenstionContainer;
        [SerializeField] private MaxAvailableAscensionSaveManager maxAvailableAscensionSaveManager;

        private void Awake()
        {
            maxAvailableAscensionSaveManager.TryToLoadInfo();

            for(int i = 0; i < MaxAvailavleAscension.indexOfMaxAbailableAscension + 1; i++)
            {
                availableAscensionsForExpirensedToadMode.Add(ascensionsForExpirensedToadMode[i]);
            }
        }

        public void ShowDescriptionOfAscentionByAscensionContainer(AscensionSetupContainer ascensionSetupContainer)
        {
            description.text = ascensionSetupContainer.AscensionSetup.Description;
        }

        public void IncreaseCurrentAscentionIndex(int value)
        {
            currentAscensionIndex += value;
            currentAscensionIndex = Mathf.Clamp(currentAscensionIndex, 0, availableAscensionsForExpirensedToadMode.Count - 1);
            ascensionCountLabel.text = currentAscensionIndex.ToString() + " / 7";
            runWithAscenstionContainer.AscensionSetup = availableAscensionsForExpirensedToadMode[currentAscensionIndex];
            ShowDescriptionOfAscentionByAscensionContainer(runWithAscenstionContainer);
        }

        public void SetCurrentAscension(AscensionSetupContainer ascensionSetupContainer)
        {
            CurrentAscention.ascensionSetup = ascensionSetupContainer.AscensionSetup;
        }

        public void StartGame(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}