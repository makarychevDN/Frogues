using UnityEngine;

namespace FroguesFramework
{
    public class LocalizationLoader : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Extensions.SetLocale(PlayerPrefs.GetInt("LastSelectedLocale")));
        }
    }
}