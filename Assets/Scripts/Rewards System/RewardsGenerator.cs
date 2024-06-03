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