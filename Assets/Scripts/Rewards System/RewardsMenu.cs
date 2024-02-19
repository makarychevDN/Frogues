using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static FroguesFramework.RewardsGenerator;

namespace FroguesFramework
{
    public class RewardsMenu : MonoBehaviour
    {
        [SerializeField] private Transform parentForSetsOfRewards;
        [SerializeField] private GameObject background;
        [SerializeField] private Button chooseRewardsButton;
        [SerializeField] private SetOfRewardsPanel setOfRewardsPanelPrefab;
        [SerializeField] private List<SetOfRewardsPanel> setOfRewardsPanels;
        public UnityEvent<SetOfRewardsPanel> OnRewardChosen;

        public void Awake()
        {
            chooseRewardsButton.onClick.AddListener(DisplayRewardsMenu);
        }

        public void GenerateSetOfRewards(RewardType rewardType, int countOfPossibleRewards)
        {
            var newSetOfRewardsPanel = Instantiate(setOfRewardsPanelPrefab, parentForSetsOfRewards);
            newSetOfRewardsPanel.Init(rewardType, countOfPossibleRewards);
            setOfRewardsPanels.Add(newSetOfRewardsPanel);
            chooseRewardsButton.gameObject.SetActive(true);
            newSetOfRewardsPanel.OnRewardFromSetPicked.AddListener(() => RemoveSetOfRewardsFromPanel(newSetOfRewardsPanel));
        }

        public void RemoveSetOfRewardsFromPanel(SetOfRewardsPanel setOfRewards)
        {
            setOfRewardsPanels.Remove(setOfRewards);
            Destroy(setOfRewards.gameObject);
            parentForSetsOfRewards.gameObject.SetActive(setOfRewardsPanels.Count > 0);
            background.SetActive(setOfRewardsPanels.Count > 0);
        }

        private void DisplayRewardsMenu()
        {
            parentForSetsOfRewards.gameObject.SetActive(true);
            background.SetActive(true);
            chooseRewardsButton.gameObject.SetActive(false);
        }
    }
}