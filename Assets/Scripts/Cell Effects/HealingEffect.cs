using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class HealingEffect : CellsEffectWithPreVisualization
    {
        [SerializeField] private BaseCellsTaker cellsTaker;
        [SerializeField] private IntContainer healingValue;

        public override void ApplyEffect() => ApplyEffect(cellsTaker.Take());

        public override void ApplyEffect(List<Cell> cells)
        {
            if (cells == null)
                return;

            TakeCellsAbleToTakeDamage(cells).ForEach(cell => cell.Content.health.TakeHealing(healingValue.Content));
        }

        public override void PreVisualizeEffect() => PreVisualizeEffect(cellsTaker.Take());

        public override void PreVisualizeEffect(List<Cell> cells)
        {
            //if (cells == null)
            //return;

            //TakeCellsAbleToTakeDamage(cells).ForEach(cell => cell.Content.health.PretakeDamage(damage.Content, damageType.Content, ignoreArmor));
        }

        private List<Cell> TakeCellsAbleToTakeDamage(List<Cell> cells)
        {
            return CellsListToCulumnsList(cells).Where(cell => !cell.IsEmpty && cell.Content.health != null).ToList();
        }
    }
}
