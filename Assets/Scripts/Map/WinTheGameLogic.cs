using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FroguesFramework
{
    public class WinTheGameLogic : MonoBehaviour
    {
        [SerializeField] private TMP_Text textOfExit;
        [SerializeField] private Cell exitCell;
        [SerializeField] private List<GameObject> warningAboutWinningObjects;
        [SerializeField] private MaxAvailableAscensionSaveManager maxAvailableAscensionSaveManager;

        private void Awake()
        {
            EntryPoint.Instance.OnFinalPartStarted.AddListener(warnAboutFinalPart);
            EntryPoint.Instance.OnWin.AddListener(warnAboutWinning);
            EntryPoint.Instance.TryToCountCampfireAfterFinalPartStarted();
        }

        private void warnAboutFinalPart()
        {
            textOfExit.text = "последний рывок";
        }

        private void warnAboutWinning()
        {
            textOfExit.text = "в меню";
            warningAboutWinningObjects.ForEach(go => go.SetActive(true));
            exitCell.OnBecameFull.RemoveAllListeners();
            exitCell.OnBecameFull.AddListener(() => SceneManager.LoadScene("main menu"));
            maxAvailableAscensionSaveManager.TryToSaveInfo();
        }
    }
}