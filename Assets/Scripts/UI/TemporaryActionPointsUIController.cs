using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class TemporaryActionPointsUIController : ResourcesPointsUIController<AbilityResourcePoints>
    {
        private AbilityResourcePoints dataSource;

        public override void Init(AbilityResourcePoints dataSource)
        {
            this.dataSource = dataSource;
            dataSource.OnPointsSpent.AddListener(RedrawIcons);
            dataSource.OnTemporaryPointsIncreased.AddListener(RedrawIcons);
            dataSource.OnPreSpendPointsChanged.AddListener(RedrawIcons);
            dataSource.OnTemporaryPointsReseted.AddListener(RedrawIcons);

            RedrawIcons();
        }

        private void RedrawIcons()
        {
            RedrawIcons(dataSource.TemporaryPoints, dataSource.TemporaryPoints, dataSource.PreTakenTemporaryPoints);
        }

        public override void UnInit()
        {
            dataSource.OnPointsSpent.RemoveListener(RedrawIcons);
            dataSource.OnTemporaryPointsIncreased.RemoveListener(RedrawIcons);
            dataSource.OnPreSpendPointsChanged.RemoveListener(RedrawIcons);
            dataSource.OnTemporaryPointsReseted.RemoveListener(RedrawIcons);
        }

        private void OnDestroy()
        {
            UnInit();
        }
    }
}