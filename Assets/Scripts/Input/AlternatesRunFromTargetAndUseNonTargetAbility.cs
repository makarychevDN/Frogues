using UnityEngine;

namespace FroguesFramework
{
    public class AlternatesRunFromTargetAndUseNonTargetAbility : AlternatesRunFromTargetAndDoSomethingAI
    {
        [SerializeField] private NonTargetAbility nonTargetAbility;

        protected override void TryToDoSomething()
        {
            if (nonTargetAbility == null || !nonTargetAbility.PossibleToUse())
            {
                EndTurn();
                return;
            }

            nonTargetAbility.Use();
        }
    }
}