namespace FroguesFramework
{
    public interface IAbleToApplySpikesModificator
    {
        public int GetSpikesModificatorValue();
        public int GetdeltaOfSpikesValueForEachTurn();
        public int GetTimeToEndOfSpikesEffect();
        public bool GetSpikesEffectIsConstantly();
    }
}