using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class CellHighlighter : MonoBehaviour
    {
        [SerializeField] private HexagonCellNeighbours hexagonCellNeighbours;
        [SerializeField] private List<BorderWithHexDir> bordersAndDirs;
        [SerializeField] private GameObject highlight;

        public void EnableBordersAndHighlight(List<Cell> highlightedCells)
        {
            highlight.SetActive(true);
            
            foreach (var borderWithDir in bordersAndDirs)
            {
                borderWithDir.border.SetActive(!highlightedCells.Contains(hexagonCellNeighbours.GetNeighborByHexDir(borderWithDir.hexDir)));
            }
        }

        public void SetActive(bool on)
        {
            highlight.SetActive(on);

            foreach(var borderWithDir in bordersAndDirs)
            {
                borderWithDir.border.SetActive(on);
            }
        }
        
        public void SetActiveHighlight(bool on)
        {
            highlight.SetActive(on);
        }
        
        public void SetActiveBorders(bool on)
        {
            bordersAndDirs.ForEach(borderAndDir => borderAndDir.border.SetActive(on));
        }

        [Serializable]
        private struct BorderWithHexDir
        {
            public HexDir hexDir;
            public GameObject border;
        }
    }
}