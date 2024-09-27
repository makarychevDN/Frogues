namespace FroguesFramework
{
    public class ActionPointsUIController : ResourcesPointsUIController<AbilityResourcePoints>
    {
        private AbilityResourcePoints dataSource; 

        public override void Init(AbilityResourcePoints dataSource)
        {
            this.dataSource = dataSource;
            dataSource.OnPointsSpent.AddListener(RedrawIcons);
            dataSource.OnDefaultPointsIncreased.AddListener(RedrawIcons);
            dataSource.OnPreSpendPointsChanged.AddListener(RedrawIcons);
            dataSource.OnMaxPointsChanged.AddListener(RedrawIcons);

            RedrawIcons();
        }

        private void RedrawIcons()
        {
            RedrawIcons(dataSource.CurrentPoints, dataSource.MaxPointsCount, dataSource.PreTakenCurrentPoints);
        }

        public override void UnInit()
        {
            dataSource.OnPointsSpent.RemoveListener(RedrawIcons);
            dataSource.OnDefaultPointsIncreased.RemoveListener(RedrawIcons);
            dataSource.OnPreSpendPointsChanged.RemoveListener(RedrawIcons);
            dataSource.OnMaxPointsChanged.RemoveListener(RedrawIcons);
        }

        private void OnDestroy()
        {
            UnInit();
        }

    }
}