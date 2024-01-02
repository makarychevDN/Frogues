using System.Collections;
using FroguesFramework;
using UnityEngine;

public class SpikeBushesAbility : NonTargetAbility
{
    [Space, Header("Ability Settings")]
    [SerializeField] private int spikesValue;
    [SerializeField] private int temporaryBlockValue;
    [SerializeField] private int permanentBlockValue;
    [SerializeField] private int timeToEndEffect = 1;
        
    public override void Use()
    {
        if (!PossibleToUse())
            return;

        SpendResourcePoints();
        SetCooldownAsAfterUse();

        CurrentlyActiveObjects.Add(this);
        _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());
        StartCoroutine(ApplyEffect(timeBeforeImpact));
        Invoke(nameof(RemoveCurrentlyActive), fullAnimationTime);
    }

    protected virtual IEnumerator ApplyEffect(float time)
    {
        yield return new WaitForSeconds(time);
        _owner.Stats.AddStatEffect(new StatEffect(StatEffectTypes.spikes, spikesValue, timeToEndEffect));
        _owner.Health.IncreaseTemporaryBlock(temporaryBlockValue);
        _owner.Health.IncreasePermanentBlock(permanentBlockValue);
    }

    private void RemoveCurrentlyActive() => CurrentlyActiveObjects.Remove(this);
}
