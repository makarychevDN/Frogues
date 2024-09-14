namespace FroguesFramework
{
    public class BoostStatPointsUIController : StatPointsUIController<Stats>
    {
        Stats _health;

        public override void Init(Stats dataSource)
        {
            _health = dataSource;
            dataSource.OnBoostUpdated.AddListener(RedrawVisualization);
            RedrawVisualization();
        }

        protected override int GetTargetValue() => _health.Boost;
    }
}