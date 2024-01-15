namespace FroguesFramework
{
    public interface IAbleToApplyAnyModificator
    {
        public int GetModificatorValue();
        public int GetDeltaValueForEachTurn();
        public int GetTimeToEndOfEffect();
        public bool GetEffectIsConstantly();
    }
}