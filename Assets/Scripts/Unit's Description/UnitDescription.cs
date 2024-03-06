using UnityEngine;

namespace FroguesFramework
{
    public class UnitDescription : MonoBehaviour
    {
        [SerializeField] private string unitName;
        [SerializeField, TextArea] private string description;

        public string UnitName => unitName;
        public string Description => description;
    }
}