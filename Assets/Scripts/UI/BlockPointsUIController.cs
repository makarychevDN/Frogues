namespace FroguesFramework
{
    public class BlockPointsUIController : ResourcesPointsUIController<Health>
    {
        public override void Init(Health dataSource)
        {
            dataSource.OnDamageBlocked.AddListener(() => RedrawIcons(dataSource));
            dataSource.OnBlockChargesCountUpdated.AddListener(() => RedrawIcons(dataSource));
            dataSource.OnPretakenDamageOnBlockChanged.AddListener(() => RedrawIcons(dataSource));

            RedrawIcons(dataSource);
        }

        private void RedrawIcons(Health dataSource)
        {
            RedrawIcons(dataSource.Block, dataSource.Block, dataSource.BlockWithPreTakenDamage);
        }
    }
}