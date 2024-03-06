
namespace FroguesFramework
{
    public class TrainingInThirdRoom : BaseTrainingModificator
    {
        public override void Init()
        {
            EntryPoint.Instance.MetaPlayer.MovementAbility.IncreaseActionPointsCost(1);
        }
    }
}