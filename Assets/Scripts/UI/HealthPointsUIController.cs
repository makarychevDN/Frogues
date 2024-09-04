namespace FroguesFramework
{
    public class HealthPointsUIController : ResourcesPointsUIController<Health>
    {
        public override void Init(Health dataSource)
        {
            dataSource.OnDamageAppledByHealth.AddListener(() => RedrawIcons(dataSource));
            dataSource.OnHpHealed.AddListener(() => RedrawIcons(dataSource));
            dataSource.OnPretakenDamageOnHealthChanged.AddListener(() => RedrawIcons(dataSource));
            dataSource.OnMaxHealthChanged.AddListener(() => RedrawIcons(dataSource));

            RedrawIcons(dataSource);
        }

        private void RedrawIcons(Health dataSource)
        {
            RedrawIcons(dataSource.CurrentHp, dataSource.MaxHp, dataSource.HealthWithPreTakenDamage);
        }
    }
}