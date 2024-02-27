namespace FroguesFramework
{
    public interface IAbleToHaveCooldown
    {
        public void DecreaseCooldown(int value = 1);
        public void SetCooldownAsAfterStart();
        public void SetCooldownAsAfterUse();
        public int GetCooldownCounter();
        public int GetCooldownAfterStart();
        public int GetCooldownAfterUse();
        public bool GetCooldownAfterStartIsDone();
        public int GetCurrentCooldown();
        public bool IsEnoughCharges();
        public int GetCurrentCharges();
        public int GetMaxCharges();
        public void SpendCharges();
    }
}