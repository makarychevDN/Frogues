namespace FroguesFramework
{
    public class BlockPointsUIController : ResourcesPointsUIController<Health>
    {
        private Health dataSource;

        public override void Init(Health dataSource)
        {
            this.dataSource = dataSource;
            dataSource.OnDamageBlocked.AddListener(RedrawIcons);
            dataSource.OnBlockChargesCountUpdated.AddListener(RedrawIcons);
            dataSource.OnPretakenDamageOnBlockChanged.AddListener(RedrawIcons);

            RedrawIcons();
        }

        private void RedrawIcons()
        {
            RedrawIcons(dataSource.Block, dataSource.Block, dataSource.BlockWithPreTakenDamage);
        }

        public override void UnInit()
        {
            dataSource.OnDamageBlocked.RemoveListener(RedrawIcons);
            dataSource.OnBlockChargesCountUpdated.RemoveListener(RedrawIcons);
            dataSource.OnPretakenDamageOnBlockChanged.RemoveListener(RedrawIcons);
        }
    }
}