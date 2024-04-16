using UnityEngine;
using UnityEngine.Localization;
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

        [SerializeField] LocalizedString finalTestName;
        [SerializeField] LocalizedString finalTestDescription;

        [SerializeField] LocalizedString campName;
        [SerializeField] LocalizedString campDescription;

        [SerializeField] LocalizedString levelName;
        [SerializeField] LocalizedString levelDescription;
        [SerializeField] LocalizedString levelDescriptionOnMaxLvl;

        [SerializeField] private Vector2 pivot;
        [SerializeField] private Vector2 offset;

        private int _hashedPointsToGoToCamp;

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

            if (nextReward == null)
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
            _hashedPointsToGoToCamp = EntryPoint.Instance.AscensionSetup.RequaredDeltaOfScoreToOpenExitToCampfire - EntryPoint.Instance.ScoreDeltaCounterForBonfire;
        }

        public void ShowHintAboutCampfire()
        {
            string campDescriptionWithValues = campDescription.GetLocalizedString();
            campDescriptionWithValues = campDescriptionWithValues.Replace("{value}", EntryPoint.Instance.AscensionSetup.RequaredDeltaOfScoreToOpenExitToCampfire.ToString());
            campDescriptionWithValues = campDescriptionWithValues.Replace("{value_2}", _hashedPointsToGoToCamp.ToString());
            EntryPoint.Instance.CommonSmallHint.Init(campName.GetLocalizedString(), campDescriptionWithValues, transform, pivot, offset);
            EntryPoint.Instance.CommonSmallHint.EnableContent(true);
        }

        public void ShowHintAboutLvlUp()
        {
            string levelDescriptionWithValues;

            var nextReward = EntryPoint.Instance.RewardsGenerator.GetNextRewardSetup();
            if (nextReward == null)
            {
                levelDescriptionWithValues = levelDescriptionOnMaxLvl.GetLocalizedString();
            }
            else
            {
                levelDescriptionWithValues = levelDescription.GetLocalizedString();
                levelDescriptionWithValues = levelDescriptionWithValues.Replace("{value}", nextReward.scoreRequirement.ToString());
            }

            EntryPoint.Instance.CommonSmallHint.Init(levelName.GetLocalizedString(), levelDescriptionWithValues, transform, pivot, offset);
            EntryPoint.Instance.CommonSmallHint.EnableContent(true);
        }

        public void ShowHintAboutFinalTest()
        {
            EntryPoint.Instance.CommonSmallHint.Init(finalTestName.GetLocalizedString(), finalTestDescription.GetLocalizedString(), transform, pivot, offset);
            EntryPoint.Instance.CommonSmallHint.EnableContent(true);
        }

        public void HideHint()
        {
            EntryPoint.Instance.CommonSmallHint.EnableContent(false);
        }
    }
}