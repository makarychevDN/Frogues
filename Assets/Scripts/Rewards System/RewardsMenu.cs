using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class RewardsMenu : MonoBehaviour
    {
        [SerializeField] private Transform parentForSetsOfRewards;
        [SerializeField] private GameObject background;
        [SerializeField] private SetOfRewardsPanel setOfRewardsPanelPrefab;
        [SerializeField] private List<SetOfRewardsPanel> setOfRewardsPanels;
        public UnityEvent<SetOfRewardsPanel> OnRewardChosen;

        public void GenerateSetOfRewards(bool isPassiveAbilitiesPool, int countOfPossibleRewards)
        {
            var newSetOfRewardsPanel = Instantiate(setOfRewardsPanelPrefab, parentForSetsOfRewards);
            newSetOfRewardsPanel.Init(isPassiveAbilitiesPool, countOfPossibleRewards);
            setOfRewardsPanels.Add(newSetOfRewardsPanel);
            background.SetActive(true);
            newSetOfRewardsPanel.OnRewardFromSetPicked.AddListener(() => RemoveSetOfRewardsFromPanel(newSetOfRewardsPanel));
        }

        public void RemoveSetOfRewardsFromPanel(SetOfRewardsPanel setOfRewards)
        {
            setOfRewardsPanels.Remove(setOfRewards);
            Destroy(setOfRewards.gameObject);
            background.SetActive(setOfRewardsPanels.Count > 0);
        }
    }
}