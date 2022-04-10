using UnityEngine;

namespace FroguesFramework
{
    public class SetVectorContainerValueByTargetUnitPos : MonoBehaviour
    {
        [SerializeField] private Unit thisUnit;
        [SerializeField] private UnitContainer targetUnit;
        [SerializeField] private Vector2IntContainer vectorContainer;

        // Update is called once per frame
        void Update()
        {
            if (targetUnit.Content == null)
                return;

            Vector3 unitPos = thisUnit.transform.position;
            Vector3 targetUnitPos = targetUnit.Content.transform.position;

            if (targetUnitPos.x < unitPos.x && targetUnitPos.y < unitPos.y)
            {
                vectorContainer.Content = Vector2Int.left;
            }

            if (targetUnitPos.x > unitPos.x && targetUnitPos.y < unitPos.y)
            {
                vectorContainer.Content = Vector2Int.down;
            }

            if (targetUnitPos.x < unitPos.x && targetUnitPos.y > unitPos.y)
            {
                vectorContainer.Content = Vector2Int.up;
            }

            if (targetUnitPos.x > unitPos.x && targetUnitPos.y > unitPos.y)
            {
                vectorContainer.Content = Vector2Int.right;
            }
        }
    }
}
