using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class UIOfUnit : MonoBehaviour
    {
        [Header("Healthbar Setup")]
        [SerializeField] private HealthPointsUIController healthPointsUIController;
        [SerializeField] private ActionPointsUIController actionPointsUIController;
        [SerializeField] private BlockPointsUIController blockPointsUIController;
        [SerializeField] private RectTransform commonResizableParent;

        [Header("Status Effects Setup")]
        [SerializeField] private PoisonStatPointsUIController poisonStatPointsUIController;
        [SerializeField] private BoostStatPointsUIController boostStatPointsUIController;

        public void Init(Unit unit)
        {
            healthPointsUIController.OnIconsRedrawed.AddListener(UpdateCommonResizableParent);
            actionPointsUIController.OnIconsRedrawed.AddListener(UpdateCommonResizableParent);
            blockPointsUIController.OnIconsRedrawed.AddListener(UpdateCommonResizableParent);

            healthPointsUIController.Init(unit.Health);
            actionPointsUIController.Init(unit.ActionPoints);
            blockPointsUIController.Init(unit.Health);
            poisonStatPointsUIController.Init(unit.Health);
            boostStatPointsUIController.Init(unit.Stats);
        }

        private void UpdateCommonResizableParent()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(commonResizableParent);
        }
    }
}