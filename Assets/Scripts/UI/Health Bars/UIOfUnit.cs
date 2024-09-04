using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class UIOfUnit : MonoBehaviour
    {
        [SerializeField] private HealthPointsUIController healthPointsUIController;
        [SerializeField] private ActionPointsUIController actionPointsUIController;
        [SerializeField] private BlockPointsUIController blockPointsUIController;
        [SerializeField] private RectTransform commonResizableParent;

        public void Init(Unit unit)
        {
            healthPointsUIController.OnIconsRedrawed.AddListener(UpdateCommonResizableParent);
            actionPointsUIController.OnIconsRedrawed.AddListener(UpdateCommonResizableParent);
            blockPointsUIController.OnIconsRedrawed.AddListener(UpdateCommonResizableParent);

            healthPointsUIController.Init(unit.Health);
            actionPointsUIController.Init(unit.ActionPoints);
            blockPointsUIController.Init(unit.Health);
        }

        private void UpdateCommonResizableParent()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(commonResizableParent);
        }
    }
}