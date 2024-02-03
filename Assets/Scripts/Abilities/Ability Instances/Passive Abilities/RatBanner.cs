namespace FroguesFramework
{
    public class RatBanner : PassiveAbility
    {
        public override void Init(Unit unit)
        {
            base.Init(unit);

            EntryPoint.Instance.CountOfRats++;
            _owner.AbleToDie.OnDeath.AddListener(DecreaseCountOfRats);
        }

        public override void UnInit()
        {
            DecreaseCountOfRats();
            _owner.AbleToDie.OnDeath.RemoveListener(DecreaseCountOfRats);

            base.UnInit();
        }

        private void DecreaseCountOfRats()
        {
            EntryPoint.Instance.CountOfRats--;
        }
    }
}