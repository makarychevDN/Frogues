namespace FroguesFramework
{
    public interface IAbleToHaveCooldown
    {
        public void DecreaseCooldown(int value = 1);
        public void SetCooldownAsAfterStart();
        public void SetCooldownAsAfterUse();
        public bool IsCooldowned();
        public int GetCooldownCounter();
    }
}