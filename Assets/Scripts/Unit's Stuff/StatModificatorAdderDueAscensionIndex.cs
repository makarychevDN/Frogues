using FroguesFramework;
using UnityEngine;

namespace FroguesFramework
{
    public class StatModificatorAdderDueAscensionIndex : MonoBehaviour
    {
        [SerializeField] private int expectedAscension = -1;
        [SerializeField] private Stats stats;
        [SerializeField] private StatEffect statEffect;

        void Start()
        {
            if (EntryPoint.Instance.AscensionSetup.index == expectedAscension)
            {
                stats.AddStatEffect(statEffect);
            }
        }
    }
}