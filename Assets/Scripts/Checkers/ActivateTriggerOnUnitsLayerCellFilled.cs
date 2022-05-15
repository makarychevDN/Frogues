using UnityEngine;

namespace FroguesFramework
{
    public class ActivateTriggerOnUnitsLayerCellFilled : MonoBehaviour
    {
        [SerializeField] private Cell cell;

        public void TriggerOnBecameFull()
        {
            if (cell.IsEmpty)
                return;

            var temp = cell.Content.GetComponentInChildren<SurfaceTrigger>();
            if (temp != null)
                temp.CellBecameFull();
        }

        public void TriggerOnBecameEmpty()
        {
            if (cell.IsEmpty)
                return;

            var temp = cell.Content.GetComponentInChildren<SurfaceTrigger>();
            if (temp != null)
                temp.CellBecameEmpty();
        }
    }
}