namespace FroguesFramework
{
    public interface IAbleToApplyIntelligenceModificator
    {
        public int GetIntelligenceModificatorValue();
        public int GetDeltaOfIntelligenceValueForEachTurn();
        public int GetTimeToEndOfIntelligenceEffect();
        public bool GetIntelligenceEffectIsConstantly();
    }
}