using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    [SerializeField] private Transform sprite;

    public void TurnLeft()
    {
        sprite.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void TurnRight()
    {
        sprite.rotation = Quaternion.Euler(0, 0, 0);
    }
    
    
    /// <summary>
    /// rotates the sprite to the left if the value is less than zero and to the right if it is greater than zero
    /// </summary>
    /// <param name="value"></param>
    public void Turn(float value)
    {
        if(value < 0)
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

    public void TurnByCoordinatesRelativeToSprite(Vector3 coordinates)
    {
        if((coordinates - sprite.transform.position).x > 0)
            TurnRight();
        
        if((coordinates - sprite.transform.position).x < 0)
            TurnLeft();
    }
}
