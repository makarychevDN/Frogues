using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class HexagonCellNeighbours : MonoBehaviour
    {
        [SerializeField] private Cell myCell;
        
        private Dictionary<HexDir, Cell> _neighbours;
        private Dictionary<HexDir, Cell> _oppositeNeighbours;
        private Dictionary<Cell, HexDir> _hexDirsByCell;
        private Dictionary<Cell, HexDir> _oppositeDirsByCell;

        private List<Cell> _neighbors;
        private Cell _topLeftCell;
        private Cell _topRightCell;
        private Cell _downLeftCell;
        private Cell _downRightCell;
        private Cell _leftCell;
        private Cell _rightCell;

        public Cell GetNeighborByHexDir(HexDir hexDir) => _neighbours[hexDir];
        public Cell GetOppositeNeighborByHexDir(HexDir hexDir) => _oppositeNeighbours[hexDir];
        public HexDir GetHexDirByNeighbor(Cell neighbor) => _hexDirsByCell[neighbor];
        public List<Cell> GetAllNeighbors() => _neighbors;

        #region Init

        public void Init()
        {
            _oppositeDirsByCell = new Dictionary<Cell, HexDir>();

            InitNeighborCells();
            InitNeighbours();
            InitOppositeNeighbours();
            InitHexDirsByCell();
            InitOppositeHexDirsByCell();
        }
        
        private void InitNeighborCells()
        {
            int evenModificator = myCell.coordinates.y.Even().ToInt();
            int oddModificator = myCell.coordinates.y.Odd().ToInt();

            _topLeftCell =
                EntryPoint.Instance.Map.GetCell(new Vector2Int(myCell.coordinates.x - evenModificator, myCell.coordinates.y + 1),
                    myCell.mapLayer);
            
            _topRightCell =
                EntryPoint.Instance.Map.GetCell(new Vector2Int(myCell.coordinates.x + oddModificator, myCell.coordinates.y + 1),
                    myCell.mapLayer);
            
            _downLeftCell =
                EntryPoint.Instance.Map.GetCell(new Vector2Int(myCell.coordinates.x - evenModificator, myCell.coordinates.y - 1),
                    myCell.mapLayer);
            
            _downRightCell =
                EntryPoint.Instance.Map.GetCell(new Vector2Int(myCell.coordinates.x + oddModificator, myCell.coordinates.y - 1),
                    myCell.mapLayer);

            _leftCell = EntryPoint.Instance.Map.GetCell(myCell.coordinates + Vector2Int.left, myCell.mapLayer);
            _rightCell = EntryPoint.Instance.Map.GetCell(myCell.coordinates + Vector2Int.right, myCell.mapLayer);

            _neighbors = new List<Cell>();
            _neighbors.Add(_topLeftCell);
            _neighbors.Add(_topRightCell);
            _neighbors.Add(_downLeftCell);
            _neighbors.Add(_downRightCell);
            _neighbors.Add(_leftCell);
            _neighbors.Add(_rightCell);
        }
        
        private void InitNeighbours()
        {
            _neighbours = new Dictionary<HexDir, Cell>
            {
                {HexDir.left, _leftCell},
                {HexDir.right, _rightCell},
                {HexDir.topLeft, _topLeftCell},
                {HexDir.topRight, _topRightCell},
                {HexDir.downLeft, _downLeftCell},
                {HexDir.downRight, _downRightCell}
            };
        }
        
        private void InitOppositeNeighbours()
        {
            _oppositeNeighbours = new Dictionary<HexDir, Cell>
            {
                {HexDir.left, _rightCell},
                {HexDir.right, _leftCell},
                {HexDir.topLeft, _downRightCell},
                {HexDir.topRight, _downLeftCell},
                {HexDir.downLeft, _topRightCell},
                {HexDir.downRight, _topLeftCell}
            };
        }
        
        private void InitHexDirsByCell()
        {
            _hexDirsByCell = new Dictionary<Cell, HexDir>
            {
                {_leftCell, HexDir.left},
                {_rightCell, HexDir.right},
                {_topLeftCell, HexDir.topLeft},
                {_topRightCell, HexDir.topRight},
                {_downLeftCell, HexDir.downLeft},
                {_downRightCell, HexDir.downRight}
            };
        }

        private void InitOppositeHexDirsByCell()
        {
            _oppositeDirsByCell = new Dictionary<Cell, HexDir>
            {
                {_leftCell, HexDir.right},
                {_rightCell, HexDir.left},
                {_topLeftCell, HexDir.downRight},
                {_topRightCell, HexDir.downLeft},
                {_downLeftCell, HexDir.topRight},
                {_downRightCell, HexDir.topLeft},
            };
        }
        
        #endregion
    }
    

    public enum HexDir
    {
        /*zero = 0,*/ topRight = 1, right = 2, downRight = 3, downLeft = 4, left = 5, topLeft = 6
    }

}