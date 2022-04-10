using UnityEngine;

namespace FroguesFramework
{
    public class Vector2IntDirectionToTarget : Vector2IntContainer
    {
        [SerializeField] private Unit user;
        [SerializeField] private UnitContainer targetContainer;
        [SerializeField] private bool debugDir;

        public override Vector2Int Content => CalculateDirection();

        private Vector2Int CalculateDirection()
        {
            Vector2Int result = new(
                Mathf.Clamp(targetContainer.Content.Coordinates.x - user.Coordinates.x, -1, 1),
                Mathf.Clamp(targetContainer.Content.Coordinates.y - user.Coordinates.y, -1, 1)
            );

            Content = result;
            return result;
        }
    }
}