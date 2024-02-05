using UnityEngine;

namespace FroguesFramework
{
    public class UnitHealthBarRedrawerDueActionPointsCount : MonoBehaviour
    {
        [SerializeField] private AbilityResourcePoints actionPoints;
        [SerializeField] private BaseHealthBar healthBar;
        private int _hashedActionPoints;

        private void Update()
        {
            if(_hashedActionPoints != actionPoints.CalculateHashFunctionOfPrevisualisation())
            {
                healthBar.Redraw();
            }

            _hashedActionPoints = actionPoints.CalculateHashFunctionOfPrevisualisation();
        }
    }
}