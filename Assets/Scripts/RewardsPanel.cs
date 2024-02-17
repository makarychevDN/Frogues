using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class RewardsPanel : MonoBehaviour
    {
        [SerializeField] private Transform parentForRewards;
        [SerializeField] private GameObject background;
        [SerializeField] private List<SingleRewardPanel> singleRewardPanels;
        public UnityEvent<SingleRewardPanel> OnRewardChoosed;

        private void AddSingleReward(SingleRewardPanel singleReward)
        {
            singleRewardPanels.Add(singleReward);
            singleReward.transform.parent = parentForRewards;
            OnRewardChoosed.AddListener(RemoveRewardFromPanel);
        }

        private void RemoveRewardFromPanel(SingleRewardPanel singleReward)
        {
            singleRewardPanels.Remove(singleReward);
        }
    }
}