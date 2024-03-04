using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FroguesFramework
{
    [RequireComponent(typeof(Button))]
    public class SceneLoaderButton : MonoBehaviour
    {
        [SerializeField] private string sceneName;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(LoadScene);
        }

        void LoadScene()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}