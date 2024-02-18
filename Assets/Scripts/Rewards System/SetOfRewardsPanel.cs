using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class SetOfRewardsPanel : MonoBehaviour
    {
        [SerializeField] private SingleRewardPanel singleRewardPanelPrefab;
        [SerializeField] private Transform parentForSignleRewardPanels;
        [SerializeField] private Button cancelButton;
        [SerializeField] private List<BaseAbility> ableToBeChosenAbilities;
        public UnityEvent OnRewardFromSetPicked;

        public void Init(bool isPassiveAbilitiesPool, int numberOfAbilitesAbleToChoose)
        {
            cancelButton.onClick.AddListener(InvokeRewardPickedEvent);

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
                singleRewardPanel.OnRewardPicked.AddListener(InvokeRewardPickedEvent);
            }
        }

        private void InvokeRewardPickedEvent() => OnRewardFromSetPicked.Invoke();
    }
}