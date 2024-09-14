namespace FroguesFramework
{
    public class ThornsStatPointsUIController : StatPointsUIController<Health>
    {
        Health _health;

        public override void Init(Health dataSource)
        {
            _health = dataSource;
            dataSource.OnThornsValueUpdated.AddListener(RedrawVisualization);
            RedrawVisualization();
        }

        protected override int GetTargetValue() => _health.Thorns;
    }
}