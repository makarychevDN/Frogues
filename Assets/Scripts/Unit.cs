using UnityEngine;

namespace FroguesFramework
{
    //now unit is entry point for any unit functionality;
    public class Unit : MonoBehaviour
    {
        [SerializeField] private AbilitiesManager abilitiesAbilitiesManager;
        [SerializeField] private Animator animator;
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
        public IAbleToAct input;
        public UnitsUI UI;

        [Header("Action Points")]
        public ActionPoints actionPoints;
        [SerializeField] private AbleToSkipTurn ableToSkipTurn;

        [Header("For Small Units Only")] public bool small;
        public Vector2Int Coordinates => currentCell.coordinates;

        public MovementAbility MovementAbility => movementAbility;
        
        public Grid Grid => Map.Instance.tilemap.layoutGrid;

        public AbilitiesManager AbilitiesManager => abilitiesAbilitiesManager;

        public Animator Animator
        {
            get => animator;
            set => animator = value;
        }

        public AbleToSkipTurn AbleToSkipTurn => ableToSkipTurn;

        public void Init()
        {
            if (ableToDie != null)
            {
                ableToDie.Unit = this;
            }
            
            if (health != null)
            {
                health.Init(this);
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
                movementAbility.Init(this);
            }
            
            input = GetComponentInChildren<IAbleToAct>();
            if (input != null)
            {
                input.Init();
            }
            
            if (abilitiesAbilitiesManager != null)
            {
                abilitiesAbilitiesManager.AbleToHaveCurrentAbility = input as IAbleToHaveCurrentAbility;
            }
        }
    }
}