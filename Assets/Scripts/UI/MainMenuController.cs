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
        [SerializeField] private List<AscensionSetup> ascensionsForExpirensedToadModeascensionsForExpirensedToadMode;
        [SerializeField] private AscensionSetupContainer runWithAscenstionContainer;

        public void ShowDescriptionOfAscentionByAscensionContainer(AscensionSetupContainer ascensionSetupContainer)
        {
            description.text = ascensionSetupContainer.AscensionSetup.Description;
        }

        public void IncreaseCurrentAscentionIndex(int value)
        {
            currentAscensionIndex += value;
            currentAscensionIndex = Mathf.Clamp(currentAscensionIndex, 0, ascensionsForExpirensedToadModeascensionsForExpirensedToadMode.Count - 1);
            ascensionCountLabel.text = currentAscensionIndex.ToString();
            runWithAscenstionContainer.AscensionSetup = ascensionsForExpirensedToadModeascensionsForExpirensedToadMode[currentAscensionIndex];
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