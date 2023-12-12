namespace FroguesFramework
{
    public interface IAbleToHaveCooldown
    {
        public void DecreaseCooldown(int value = 1);
        public void SetCooldownAsAfterStart();
        public void SetCooldownAsAfterUse();
        public bool IsCooldowned();
        public int GetCooldownCounter();
        public int GetCooldownAfterStart();
        public int GetCooldownAfterUse();
        public bool GetCooldownAfterStartIsDone();
        public int GetCurrentCooldown();
    }
}