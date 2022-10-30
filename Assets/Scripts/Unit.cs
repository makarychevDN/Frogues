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
        [SerializeField] private MovementAbility movementAbility;
        
        [Header("Health Setup")]
        public Damagable health;
        public AbleToDie ableToDie;
        
        [Space]
        public Cell currentCell;
        public BaseInput input;
        public Pusher pusher;
        public Pushable pushable;
        public UnitsUI UI;
        public StringContainer description;
        public FloatContainer pathfinderWeightModificator;
        
        [Header("Action Points")]
        public ActionPoints actionPoints;
        [SerializeField] private AbleToSkipTurn ableToSkipTurn;

        [Header("For Small Units Only")] public bool small;
        public StepOnUnitTrigger stepOnUnitTrigger;

        public Vector2Int Coordinates => currentCell.coordinates;

        private void Awake()
        {
            if (ableToDie != null)
            {
                ableToDie.Unit = this;
            }
            
            if (health != null)
            {
                health.AbleToDie = ableToDie;
            }
            
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

            if (movementAbility != null)
            {
                movementAbility.Unit = this;
                movementAbility.Movable = movable;
                movementAbility.ActionPoints = actionPoints;
                movementAbility.Grid = Map.Instance.tilemap.layoutGrid;
            }
        }
    }
}