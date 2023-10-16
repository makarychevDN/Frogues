using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class ChainLightningAbility : DefaultUnitTargetAbility
    {
        [SerializeField] private int ricochetsCount;
        [SerializeField] private int rangeOfRicochet;
        [SerializeField] private LineRenderer lightingVisualizatuion;
        [SerializeField] protected float lightingApearTime;
        private List<Unit> _targets = new();

        protected override IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);

            _targets.Clear();
            _targets.Add(target);
            
            for(int i = 0; i < ricochetsCount; i++)
            {
                var theNextTarget = _targets[i].GetRandomClosestUnitToUnitInRadius(rangeOfRicochet, _targets);

                if (theNextTarget == null)
                    break;

                _targets.Add(theNextTarget);
            }

            _targets.ForEach(unit => unit.Health.TakeDamage(CalculateDamage, ignoreArmor, _owner));
            ShowLighting(_targets);

            OnEffectApplied.Invoke();
            CurrentlyActiveObjects.Add(this);
            Invoke(nameof(HideLighting), lightingApearTime);
        }

        private void ShowLighting(List<Unit> targets)
        {
            lightingVisualizatuion.gameObject.SetActive(true);
            lightingVisualizatuion.positionCount = targets.Count + 1;

            lineFromOwnerToTarget.SetPosition(0, _owner.SpriteParent.position + _owner.transform.position);
            for (int i = 0; i < targets.Count; i++)
            {
                lightingVisualizatuion.SetPosition(i + 1, targets[i].SpriteParent.position - _owner.transform.position);
            }
        }

        private void HideLighting()
        {
            lightingVisualizatuion.gameObject.SetActive(false);
            CurrentlyActiveObjects.Remove(this);
        }
    }
}