using UnityEngine;

namespace FroguesFramework
{
    public class InspectUnitManager : MonoBehaviour
    {
        public static InspectUnitManager Instance;
        [SerializeField] private UnitDescriptionPanel unitDescriptionPanel;

        public UnitDescriptionPanel UnitDescriptionPanel => unitDescriptionPanel;

        private void Awake()
        {
            Instance = this;
        }
    }
}