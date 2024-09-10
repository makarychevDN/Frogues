using UnityEngine;

namespace FroguesFramework
{
    public abstract class StatPointsUIController<T> : MonoBehaviour
    {
        [SerializeField] private GameObject parentOfVisualization;
        [SerializeField] private IntSpriteFontSegment intSpriteFontSegment;

        public abstract void Init(T dataSource);

        public void RedrawVisualization()
        {
            parentOfVisualization.SetActive(GetTargetValue() != 0);
            intSpriteFontSegment.SetValue(GetTargetValue());
        }

        protected abstract int GetTargetValue();

    }
}