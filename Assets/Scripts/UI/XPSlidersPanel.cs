using UnityEngine;
using UnityEngine.UI;
using static FroguesFramework.RewardsGenerator;

namespace FroguesFramework
{
    public class XPSlidersPanel : MonoBehaviour
    {
        [SerializeField] Slider endRunScrollbar;
        [SerializeField] Slider campfireScrollbar;
        [SerializeField] Slider lvlUpScrollbar;
        [SerializeField] GameObject iconOfLvlUp;
        [SerializeField] GameObject iconOfMaxLvl;

        private void Start()
        {
            EntryPoint.Instance.OnScoreIncreased.AddListener(SetValuesToEndRunSider);
            EntryPoint.Instance.OnScoreIncreased.AddListener(SetValuesToLvlUpSider);
            EntryPoint.Instance.OnScoreIncreased.AddListener(SetValuesToCampfireSlider);

            SetValuesToEndRunSider();
            SetValuesToLvlUpSider();
            SetValuesToCampfireSlider();
        }

        private void SetValuesToEndRunSider() => endRunScrollbar.value = EntryPoint.Instance.Score;

        private void SetValuesToLvlUpSider()
        {
            RewardPanelSetup nextReward = EntryPoint.Instance.RewardsGenerator.GetNextRewardSetup();
            RewardPanelSetup lastReward = EntryPoint.Instance.RewardsGenerator.GetLastRewardSetup();

            if(nextReward == null)
            {
                iconOfLvlUp.SetActive(false);
                iconOfMaxLvl.SetActive(true);
                lvlUpScrollbar.value = lvlUpScrollbar.maxValue;
                return;
            }

            iconOfLvlUp.SetActive(true);
            iconOfMaxLvl.SetActive(false);

            lvlUpScrollbar.maxValue = nextReward.scoreRequirement;
            lvlUpScrollbar.minValue = lastReward == null ? 0 : lastReward.scoreRequirement;
            lvlUpScrollbar.value = EntryPoint.Instance.Score;
        }

        private void SetValuesToCampfireSlider()
        {
            campfireScrollbar.maxValue = EntryPoint.Instance.AscensionSetup.RequaredDeltaOfScoreToOpenExitToCampfire;
            campfireScrollbar.value = EntryPoint.Instance.ScoreDeltaCounterForBonfire;
        }
    }
}