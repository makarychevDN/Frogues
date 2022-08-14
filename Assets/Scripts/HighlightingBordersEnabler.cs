using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class HighlightingBordersEnabler : MonoBehaviour
    {
        [SerializeField] private HexagonCellNeighbours hexagonCellNeighbours;
        [SerializeField] private List<GameObjectWithHexDir> bordersAndDirs;
        [SerializeField] private Cell cell;

        public void EnableBorders(List<Cell> highlightedCells)
        {
            foreach (var borderWithDir in bordersAndDirs)
            {
                borderWithDir.border.SetActive(!highlightedCells.Contains(hexagonCellNeighbours.Neighbours[borderWithDir.hexDir]));
            }
        }

        public void SetActiveBorders(bool on) =>
            bordersAndDirs.ForEach(borderAndDir => borderAndDir.border.SetActive(on));
        
        [Serializable]
        private struct GameObjectWithHexDir
        {
            public HexDir hexDir;
            public GameObject border;
        }
    }
}