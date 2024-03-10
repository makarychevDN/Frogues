using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    [RequireComponent(typeof(Button))]
    public class AscensionSetupContainer : MonoBehaviour
    {
        [field: SerializeField] public AscensionSetup AscensionSetup { get; set; }
    }
}