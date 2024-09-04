namespace FroguesFramework
{
    public class ActionPointsUIController : ResourcesPointsUIController<AbilityResourcePoints>
    {
        public override void Init(AbilityResourcePoints dataSource)
        {
            dataSource.OnPointsSpended.AddListener(() => RedrawIcons(dataSource));
            dataSource.OnDefaultPointsIncreased.AddListener(() => RedrawIcons(dataSource));
            dataSource.OnPreSpendPointsChanged.AddListener(() => RedrawIcons(dataSource));
            dataSource.OnMaxPointsChanged.AddListener(() => RedrawIcons(dataSource));

            RedrawIcons(dataSource);
        }

        private void RedrawIcons(AbilityResourcePoints dataSource)
        {
            RedrawIcons(dataSource.CurrentPoints, dataSource.MaxPointsCount, dataSource.PreTakenCurrentPoints);
        }
    }
}