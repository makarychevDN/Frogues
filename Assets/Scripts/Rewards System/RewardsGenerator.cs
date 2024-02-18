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
                rewardsMenu.GenerateSetOfRewards(reward.rewardType, reward.countOfPossibleRewards);
            }
        }

        [Serializable]
        public class RewardPanelSetup
        {
            [field: SerializeField] public bool isGivenAlready { get; set; }
            [field: SerializeField] public int scoreRequirement { get; set; }
            [field: SerializeField] public RewardType rewardType { get; set; }
            [field: SerializeField] public int countOfPossibleRewards { get; set; }
        } 

        public enum RewardType
        {
            activeAbilities = 10, passiveAbilities = 20, stances = 30
        }
    }
}