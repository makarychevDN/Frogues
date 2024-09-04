using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class UIOfUnit : MonoBehaviour
    {
        [SerializeField] private HealthPointsUIController healthPointsUIController;
        [SerializeField] private ActionPointsUIController actionPointsUIController;
        [SerializeField] private RectTransform commonResizableParent;

        public void Init(Unit unit)
        {
            healthPointsUIController.OnIconsRedrawed.AddListener(UpdateCommonResizableParent);
            actionPointsUIController.OnIconsRedrawed.AddListener(UpdateCommonResizableParent);

            healthPointsUIController.Init(unit.Health);
            actionPointsUIController.Init(unit.ActionPoints);
        }

        private void UpdateCommonResizableParent()
        {
            print("sex");
            LayoutRebuilder.ForceRebuildLayoutImmediate(commonResizableParent);
        }
    }
}