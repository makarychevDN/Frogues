using UnityEngine;

namespace FroguesFramework
{
    public class SpriteRotator : MonoBehaviour
    {
        private Transform _sprite;

        public Transform Sprite
        {
            set => _sprite = value;
        }

        public void TurnLeft()
        {
            _sprite.rotation = Quaternion.Euler(0, 180, 0);
        }

        public void TurnRight()
        {
            _sprite.rotation = Quaternion.Euler(0, 0, 0);
        }


        /// <summary>
        /// rotates the sprite to the left if the value is less than zero and to the right if it is greater than zero
        /// </summary>
        /// <param name="value"></param>
        public void Turn(float value)
        {
            if (value < 0)
                TurnLeft();
            if (value > 0)
                TurnRight();
        }

        public void TurnByMousePosition()
        {
            TurnByCoordinatesRelativeToSprite(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        public void TurnByTarget(Unit target)
        {
            TurnByCoordinatesRelativeToSprite(target.transform.position);
        }
        
        public void TurnByTarget(Cell target)
        {
            TurnByCoordinatesRelativeToSprite(target.transform.position);
        }

        public void TurnByCoordinatesRelativeToSprite(Vector3 coordinates)
        {
            if ((coordinates - _sprite.transform.position).x > 0)
                TurnRight();

            if ((coordinates - _sprite.transform.position).x < 0)
                TurnLeft();
        }
    }
}