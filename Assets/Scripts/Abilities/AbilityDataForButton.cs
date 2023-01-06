using UnityEngine;

namespace FroguesFramework
{
    public class AbilityDataForButton : MonoBehaviour
    {
        [SerializeField] private Material material;
        public Material Material => material;
    }
}