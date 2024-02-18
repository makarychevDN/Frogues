using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class SingleRewardPanel : MonoBehaviour
    {
        [SerializeField] private Button selectThisRewardButton;
        [SerializeField] private AbilityButtonSlot abilitySlot;
        [SerializeField] private AbilityButton abilityButtonPrefab;
        public UnityEvent OnRewardPicked;

        public void Init(BaseAbility ability, Unit unit)
        {
            ability.SetOwner(unit);
            var abilityButton = Instantiate(abilityButtonPrefab);;
            abilityButton.Init(ability, abilitySlot, false, new Vector2(0.5f, 1), Vector2.down * 40 * 2);
            selectThisRewardButton.onClick.AddListener(() => PickReward(ability, unit));
        }

        private void PickReward(BaseAbility ability, Unit unit)
        {
            ability.Init(unit);
            OnRewardPicked.Invoke();
        }
    }
}