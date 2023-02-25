using System;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    //now unit is entry point for any unit functionality;
    public class Unit : MonoBehaviour
    {
        [field : Header("Map Setup")]
        [field : SerializeField] public  MapLayer unitType { get; private set; }
        [field : SerializeField] public Cell CurrentCell { get; set; }
        [field : SerializeField] public bool Small { get; private set; }
        
        [field : Header("Input Setup")]
        [field : SerializeField] public bool Enemy { get; private set; }
        [field : SerializeField] public AbilitiesManager AbilitiesManager { get; private set; }
        [field : SerializeField] public ActionPoints ActionPoints { get; private set; }
        
        [field : SerializeField] public AbleToSkipTurn AbleToSkipTurn { get; private set; }
        [field : SerializeField] public Movable Movable { get; private set; }
        [field : SerializeField] public MovementAbility MovementAbility { get; private set; }
        [field : SerializeField] public IAbleToAct ActionsInput { get; private set; }
        
        [field : Header("Sprite Setup")]
        [field : SerializeField] public Animator Animator { get; private set; }
        [field : SerializeField] public Transform SpriteParent { get; private set; }
        [field : SerializeField] public Transform Shadow { get; private set; }
        [field : SerializeField] public SpriteRotator SpriteRotator { get; private set; }

        [field : Header("Health Setup")]
        [field : SerializeField] public Health Health { get; private set; }
        [field : SerializeField] public AbleToDie AbleToDie { get; private set; }
        [field : SerializeField] public EffectsVisualiser EffectsVisualiser { get; private set; }

        public UnityEvent OnStepOnThisUnit = new UnityEvent();
        public UnityEvent<Unit> OnStepOnThisUnitByTheUnit = new UnityEvent<Unit>();
        
        public Vector2Int Coordinates => CurrentCell.coordinates;
        public Grid Grid => FindObjectOfType<Grid>();
        private bool _initAlready;

        public void Init()
        {
            if(_initAlready)
                return;
            
            ActionPoints?.Init(this);
            AbleToDie?.Init(this);
            Health?.Init(this);
            SpriteRotator?.Init(this);
            Movable?.Init(this);
            MovementAbility?.Init(this);
            
            ActionsInput = GetComponentInChildren<IAbleToAct>();
            ActionsInput?.Init();
            
            AbleToSkipTurn?.Init(this);
            AbilitiesManager?.Init(this);
            EffectsVisualiser?.Init(this);

            _initAlready = true;

            if (CurrentCell != null)
                transform.position = CurrentCell.transform.position;
        }
    }
}