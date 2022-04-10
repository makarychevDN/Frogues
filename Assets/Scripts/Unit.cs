using UnityEngine;

namespace FroguesFramework
{
    public class Unit : MonoBehaviour
    {
        public MapLayer unitType;
        public Movable movable;
        public Cell currentCell;
        public BaseInput input;
        public Pusher pusher;
        public Pushable pushable;
        public Damagable health;
        public UnitsUI UI;
        public StringContainer description;
        public FloatContainer pathfinderWeightModificator;

        [Header("For Small Units Only")] public bool small;
        public StepOnUnitTrigger stepOnUnitTrigger;

        public Vector2Int Coordinates => currentCell.coordinates;
    }
}