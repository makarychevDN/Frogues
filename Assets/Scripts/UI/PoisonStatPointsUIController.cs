using UnityEngine;

namespace FroguesFramework
{
    public class PoisonStatPointsUIController : StatPointsUIController<Health>
    {
        Health _health;

        public override void Init(Health dataSource)
        {
            _health = dataSource;
            dataSource.OnPoisonValueUpdated.AddListener(RedrawVisualization);
            RedrawVisualization();
        }

        protected override int GetTargetValue() => _health.Poison;
    }
}