using UnityEngine;

namespace FroguesFramework
{
    public class LocalizationSelector : MonoBehaviour
    {
        public void SelectLocalization(int id)
        {
            StartCoroutine(Extensions.SetLocale(id));
        }
    }
}