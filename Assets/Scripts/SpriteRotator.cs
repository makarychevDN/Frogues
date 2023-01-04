using UnityEngine;

namespace FroguesFramework
{
    public class SpriteRotator : MonoBehaviour
    {
        private Transform _sprite;
        
        public void Init(Unit unit)
        {
            _sprite = unit.SpriteParent;
        }
        
        public void TurnAroundByTarget(Vector3 targetPosition)
        {
            if (_sprite.position.PositionRelativeToMainCamera().x < targetPosition.PositionRelativeToMainCamera().x)
                TurnRight();
            else
                TurnLeft();
        }
        
        private void TurnLeft() => _sprite.localRotation = Quaternion.Euler(0, 180, 0);

        private void TurnRight() => _sprite.localRotation = Quaternion.Euler(0, 0, 0);
    }
}