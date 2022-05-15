using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class SurfaceTrigger : MonoBehaviour
    {
        public UnityEvent OnCellInUnitsLayerBecameFull;
        public UnityEvent OnCellInUnitsLayerBecameEmpty;

        public void CellBecameFull()
        {
            OnCellInUnitsLayerBecameFull.Invoke();
        }

        public void CellBecameEmpty()
        {
            OnCellInUnitsLayerBecameEmpty.Invoke();
        }
    }
}