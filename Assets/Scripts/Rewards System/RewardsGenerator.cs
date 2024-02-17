using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class RewardsGenerator : MonoBehaviour
    {
        [SerializeField] private SingleRewardPanel singleRewardPanelPrefab; 
        [SerializeField] private List<RewardPanelSetup> rewards;
        [SerializeField] private RewardsPanel rewardsPanel;

        private void Start()
        {
            EntryPoint.Instance.OnScoreIncreased.AddListener(TryToGiveReward);
        }

        private void TryToGiveReward()
        {
            List<RewardPanelSetup> currentRewards = rewards.Where(rewardSetup => !rewardSetup.isGivenAlready && rewardSetup.scoreRequirement <= EntryPoint.Instance.Score).ToList();

            if (currentRewards == null || currentRewards.Count == 0)
                return;

            foreach (var reward in currentRewards)
            {
                reward.isGivenAlready = true;
                var newRewardPanel = Instantiate(singleRewardPanelPrefab);
                newRewardPanel.Init(reward.isPassiveAbilitiesPool, reward.countOfPossibleRewards);
                rewardsPanel.AddSingleReward(newRewardPanel);
            }
        }

        [Serializable]
        public class RewardPanelSetup
        {
            [field: SerializeField] public bool isGivenAlready { get; set; }
            [field: SerializeField] public bool isPassiveAbilitiesPool { get; set; }
            [field: SerializeField] public int scoreRequirement { get; set; }
            [field: SerializeField] public int countOfPossibleRewards { get; set; }
        } 
    }
}