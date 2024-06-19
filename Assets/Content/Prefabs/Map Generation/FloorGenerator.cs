using UnityEngine;

namespace FroguesFramework
{
    public class FloorGenerator : MonoBehaviour
    {
        [SerializeField] private FloorGridGenerator floorGreedGenerator;

        public void GenerateFloor()
        {
            GenerateFloorGreed();
        }

        private void GenerateFloorGreed()
        {
            floorGreedGenerator.GenerateFloorGrid();
        }
    }
}