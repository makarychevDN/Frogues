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
            RewardPanelSetup reward = rewards.FirstOrDefault(rewardSetup => !rewardSetup.isGivenAlready && rewardSetup.scoreRequirement <= EntryPoint.Instance.Score);

            if (reward == null)
                return;

            reward.isGivenAlready = true;
            var newRewardPanel = Instantiate(singleRewardPanelPrefab);
            newRewardPanel.Init(reward.isPassiveAbilitiesPool, reward.countOfPossibleRewards);
            rewardsPanel.AddSingleReward(newRewardPanel);
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