namespace FroguesFramework
{
    public class HealthPointsUIController : ResourcesPointsUIController<Health>
    {
        private Health dataSource;

        public override void Init(Health dataSource)
        {
            this.dataSource = dataSource;
            dataSource.OnDamageAppledByHealth.AddListener(RedrawIcons);
            dataSource.OnHpHealed.AddListener(RedrawIcons);
            dataSource.OnPretakenDamageOnHealthChanged.AddListener(RedrawIcons);
            dataSource.OnMaxHealthChanged.AddListener(RedrawIcons);

            RedrawIcons();
        }

        private void RedrawIcons()
        {
            RedrawIcons(dataSource.CurrentHp, dataSource.MaxHp, dataSource.HealthWithPreTakenDamage);
        }

        public override void UnInit()
        {
            dataSource.OnDamageAppledByHealth.RemoveListener(RedrawIcons);
            dataSource.OnHpHealed.RemoveListener(RedrawIcons);
            dataSource.OnPretakenDamageOnHealthChanged.RemoveListener(RedrawIcons);
            dataSource.OnMaxHealthChanged.RemoveListener(RedrawIcons);
        }
    }
}