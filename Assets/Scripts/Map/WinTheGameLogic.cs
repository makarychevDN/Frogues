using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FroguesFramework
{
    public class WinTheGameLogic : MonoBehaviour
    {
        [SerializeField] private Vector2Int exitCellCoordinates;
        [SerializeField] private GameObject textOfExit;
        [SerializeField] private GameObject textOfFinalTest;
        [SerializeField] private GameObject textOfGoToMainMenu;
        [SerializeField] private List<GameObject> warningAboutWinningObjects;
        [SerializeField] private MaxAvailableAscensionSaveManager maxAvailableAscensionSaveManager;
        [SerializeField] private Cell _exitCell;

        private void Start()
        {
            _exitCell = EntryPoint.Instance.Map.GetCell(exitCellCoordinates);
            EntryPoint.Instance.OnFinalPartStarted.AddListener(warnAboutFinalPart);
            EntryPoint.Instance.OnWin.AddListener(warnAboutWinning);
            EntryPoint.Instance.TryToCountCampfireAfterFinalPartStarted();
        }

        private void warnAboutFinalPart()
        {
            textOfExit.gameObject.SetActive(false);
            textOfFinalTest.gameObject.SetActive(true);
        }

        private void warnAboutWinning()
        {
            textOfExit.gameObject.SetActive(false);
            textOfGoToMainMenu.gameObject.SetActive(true);
            warningAboutWinningObjects.ForEach(go => go.SetActive(true));
            _exitCell.OnBecameFull.RemoveAllListeners();
            _exitCell.OnBecameFull.AddListener(() => SceneManager.LoadScene("main menu"));
            maxAvailableAscensionSaveManager.TryToSaveInfo();
        }
    }
}