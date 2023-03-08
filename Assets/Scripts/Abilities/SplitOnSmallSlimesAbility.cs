using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class SplitOnSmallSlimesAbility : MonoBehaviour, IAbility
    {
        [SerializeField] private int radius;
        [SerializeField] private Unit prefabToSpawn;
        [SerializeField] private int expectedSpawnInstancesQuantity;
        private Unit _unit;
        private List<Cell> _spawnArea;

        public void VisualizePreUse()
        {
            throw new System.NotImplementedException();
        }

        public void Use()
        {
            _spawnArea = CellsTaker.TakeCellsAreaByRange(_unit.CurrentCell, radius);

            var emptyCells = _spawnArea.Where(cell => cell.IsEmpty).ToList(); 
            
            for (int i = 0; i < expectedSpawnInstancesQuantity; i++)
            {
                if(emptyCells.Count == 0)
                    break;
                
                var cellToSpawn = emptyCells.GetRandomElement();
                emptyCells.Remove(cellToSpawn);
                SpawnAndMoveToCell(cellToSpawn);
            }

            _unit.AbleToDie.Die();
        }
        
        public void SpawnAndMoveToCell(Cell targetCell)
        {
            var spawnedObject = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            spawnedObject.Init();
            spawnedObject.CurrentCell = _unit.CurrentCell;
            spawnedObject.Movable.FreeCostMove(targetCell);
            EntryPoint.Instance.UnitsQueue.AddObjectInQueueAfterTarget(_unit, spawnedObject);
        }

        public void Init(Unit unit)
        {
            _unit = unit;
        }

        public int GetCost() => 0;

        public bool IsPartOfWeapon() => false;
    }
}