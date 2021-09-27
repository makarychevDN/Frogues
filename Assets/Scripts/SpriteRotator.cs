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
}
