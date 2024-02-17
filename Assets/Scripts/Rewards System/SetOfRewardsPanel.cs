using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class SetOfRewardsPanel : MonoBehaviour
    {
        [SerializeField] private List<BaseAbility> ableToBeChosenAbilities;
        [SerializeField] private SingleRewardPanel singleRewardPanelPrefab;
        [SerializeField] private Transform parentForSignleRewardPanels;

        public void Init(bool isPassiveAbilitiesPool, int numberOfAbilitesAbleToChoose)
        {
            var abilityList = isPassiveAbilitiesPool ? EntryPoint.Instance.PoolOfPassiveAbilitiesPerRun : EntryPoint.Instance.PoolOfActiveAbilitiesPerRun;

            for(int i = 0; i < numberOfAbilitesAbleToChoose; i++)
            {
                var abilty = abilityList.GetRandomElement();
                ableToBeChosenAbilities.Add(abilty);
                abilityList.Remove(abilty);

                var singleRewardPanel = Instantiate(singleRewardPanelPrefab);
                singleRewardPanel.transform.parent = parentForSignleRewardPanels;
                singleRewardPanel.transform.SetSiblingIndex(1);
                singleRewardPanel.Init(abilty, EntryPoint.Instance.MetaPlayer);
            }
        }
    }
}