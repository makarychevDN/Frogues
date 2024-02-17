using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class SingleRewardPanel : MonoBehaviour
    {
        [SerializeField] private List<BaseAbility> ableToBeChosenAbilities;

        public void Init(bool isPassiveAbilitiesPool, int numberOfAbilitesAbleToChoose)
        {
            var abilityList = isPassiveAbilitiesPool ? EntryPoint.Instance.PoolOfPassiveAbilitiesPerRun : EntryPoint.Instance.PoolOfActiveAbilitiesPerRun;

            for(int i = 0; i < numberOfAbilitesAbleToChoose; i++)
            {
                var abilty = abilityList.GetRandomElement();
                ableToBeChosenAbilities.Add(abilty);
                abilityList.Remove(abilty);
            }
        }
    }
}