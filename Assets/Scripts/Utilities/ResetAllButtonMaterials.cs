using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class ResetAllButtonMaterials : MonoBehaviour
    {
        private void OnApplicationQuit()
        {
            FindObjectsOfType<AbilityButton>().ToList().ForEach(button => button.ResetButtonMaterial());
        }
    }
}