namespace FroguesFramework
{
    public abstract class BattleStanceAbility : NonTargetAbility
    {
        protected bool stanceActiveNow;

        public override void Use()
        {
            if (!IsResoursePointsEnough())
                return;

            SpendResourcePoints();
            stanceActiveNow = !stanceActiveNow;
            ApplyEffect(stanceActiveNow);
        }

        public virtual void ApplyEffect(bool isActive)
        {

        }
    }
}