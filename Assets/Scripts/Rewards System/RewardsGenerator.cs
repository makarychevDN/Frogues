using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class RewardsGenerator : MonoBehaviour
    {
        [SerializeField] private List<RewardPanelSetup> rewards;
        [SerializeField] private RewardsMenu rewardsMenu;

        public void Init()
        {
            EntryPoint.Instance.OnScoreIncreased.AddListener(TryToGiveReward);
            InitRewardsSetups();
            TryToGiveReward();
        }

        private void InitRewardsSetups()
        {
            rewards.Clear();

            foreach(RewardPanelSetup reward in EntryPoint.Instance.AscensionSetup.Rewards)
            {
                rewards.Add(new RewardPanelSetup(reward));
            }
        }

        private void TryToGiveReward()
        {
            List<RewardPanelSetup> currentRewards = rewards.Where(rewardSetup => !rewardSetup.isGivenAlready && rewardSetup.scoreRequirement <= EntryPoint.Instance.Score).ToList();

            if (currentRewards == null || currentRewards.Count == 0)
                return;

            foreach (var reward in currentRewards)
            {
                reward.isGivenAlready = true;
                rewardsMenu.GenerateSetOfRewards(reward.rewardType, reward.countOfPossibleRewards);
            }
        }

        public RewardPanelSetup GetNextRewardSetup()
        {
            return rewards.FirstOrDefault(rewardSetup => !rewardSetup.isGivenAlready);
        }

        public RewardPanelSetup GetLastRewardSetup()
        {
            return rewards.LastOrDefault(rewardSetup => rewardSetup.isGivenAlready);
        }

        [Serializable]
        public class RewardPanelSetup
        {
            [field: SerializeField] public bool isGivenAlready { get; set; }
            [field: SerializeField] public int scoreRequirement { get; set; }
            [field: SerializeField] public RewardType rewardType { get; set; }
            [field: SerializeField] public int countOfPossibleRewards { get; set; }

            public RewardPanelSetup(bool isGivenAlready, int scoreRequirement, RewardType rewardType, int countOfPossibleRewards)
            {
                this.isGivenAlready = isGivenAlready;
                this.scoreRequirement = scoreRequirement;
                this.rewardType = rewardType;
                this.countOfPossibleRewards = countOfPossibleRewards;
            }

            public RewardPanelSetup(RewardPanelSetup rewardPanel) : this(rewardPanel.isGivenAlready, rewardPanel.scoreRequirement, rewardPanel.rewardType, rewardPanel.countOfPossibleRewards) { }
        } 

        public enum RewardType
        {
            activeAbilities = 10, passiveAbilities = 20, stances = 30
        }
    }
}