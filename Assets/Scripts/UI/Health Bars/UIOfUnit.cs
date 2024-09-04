using UnityEngine;

namespace FroguesFramework
{
    public class UIOfUnit : MonoBehaviour
    {
        [SerializeField] private HealthPointsUIController healthPointsUIController;
        [SerializeField] private ActionPointsUIController actionPointsUIController;

        public void Init(Unit unit)
        {
            healthPointsUIController.Init(unit.Health);
            actionPointsUIController.Init(unit.ActionPoints);
        }
    }
}