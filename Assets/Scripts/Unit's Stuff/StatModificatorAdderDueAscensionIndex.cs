using FroguesFramework;
using UnityEngine;

public class StatModificatorAdderDueAscensionIndex : MonoBehaviour
{
    [SerializeField] private int expectedAscension = -1;
    [SerializeField] private Stats stats;
    [SerializeField] private StatEffect statEffect;

    void Start()
    {
        if(CurrentAscention.ascensionSetup.index == expectedAscension)
        {
            stats.AddStatEffect(statEffect);
        }
    }
}
