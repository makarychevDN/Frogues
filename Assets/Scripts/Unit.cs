using System;
using UnityEngine;

namespace FroguesFramework
{
    //now unit is entry point for any unit functionality;
    public class Unit : MonoBehaviour
    {
        public MapLayer unitType;
        
        [Header("Movable Setup")]
        public Movable movable;
        [SerializeField] private Transform spriteParent;
        [SerializeField] private Transform shadow;
        [SerializeField] private SpriteRotator spriteRotator;
        
        [Space]
        public Cell currentCell;
        public BaseInput input;
        public Pusher pusher;
        public Pushable pushable;
        public Damagable health;
        public UnitsUI UI;
        public StringContainer description;
        public FloatContainer pathfinderWeightModificator;
        public ActionPoints actionPoints;

        [Header("For Small Units Only")] public bool small;
        public StepOnUnitTrigger stepOnUnitTrigger;

        public Vector2Int Coordinates => currentCell.coordinates;

        private void Awake()
        {
            if (spriteRotator != null)
            {
                spriteRotator.Sprite = spriteParent;
            }
            
            if (movable != null)
            {
                movable.Unit = this;
                movable.ActionPoints = actionPoints;
                movable.SpriteParent = spriteParent;
                movable.Shadow = shadow;
                movable.SpriteRotator = spriteRotator;
            }
        }
    }
}