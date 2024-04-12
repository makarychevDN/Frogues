using UnityEngine;
using UnityEngine.Localization;

namespace FroguesFramework
{
    public class UnitDescription : MonoBehaviour
    {
        [SerializeField] private LocalizedString unitName;
        [SerializeField] private LocalizedString description;

        public string UnitName => unitName.GetLocalizedString();
        public string Description => description.GetLocalizedString();
    }
}