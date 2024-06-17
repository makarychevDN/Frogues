using UnityEngine;

namespace FroguesFramework
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager Instance;
        [SerializeField] private GameObject pauseMenu;

        public bool PauseIsActive => pauseMenu.activeSelf;

        private void Awake()
        {
            Instance = this;
        }

        public void SwitchPauseActive()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }
}