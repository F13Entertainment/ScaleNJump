using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.MatchThree.Scripts
{

    public class Tile : MonoBehaviour
    {
        [SerializeField,ReadOnly] private int _rowIndex;
        [SerializeField,ReadOnly] private int _colIndex;
        [SerializeField] private SpriteRenderer _renderer;

        public TileType tileType;
        
        private BoardManager Board => BoardManager.Instance;
        private List<ItemMovementData> ItemMovements => Board.CurrentLevelData.itemMovements;
        [ShowInInspector] public Item Item => Board?Board.map[this]:null;
        [ShowInInspector] public bool IsFilled => Item != null;

        public int RowIndex=> _rowIndex;
        public int ColIndex=> _colIndex;

        
        public void SetTileIndex(int row, int col)
        {
            _rowIndex = row;
            _colIndex = col;
            UpdatePos();
        }

        private void UpdatePos()
        {
            var startLeft = (((float) BoardManager.Instance.CurrentBoardData.cols) / 2 - 0.5f) * Vector3.left*_renderer.bounds.size.x;
            transform.position = startLeft + Vector3.down * _rowIndex*_renderer.bounds.size.y + Vector3.right*_colIndex*_renderer.bounds.size.x;
        }
        
        private bool isLocked = false;

        private void Update()
        {
            CheckIsEmpty();
        }
        
        private void CheckIsEmpty()
        {
            if (isLocked) return;
            if (IsFilled) return;
            if(tileType== TileType.Creator) //TODO Refactor
                CreateItem();
            if(tileType==TileType.Standard) //TODO Refactor
                FindItemForSwap();
        }

        private void CreateItem()
        {
            Board.CreateItemAnimated(this, () =>
            {
                isLocked = true;
            }, () =>
            {
                isLocked = false;
            });
        }

        private void FindItemForSwap()
        {
            foreach (var movement in ItemMovements)
            {
                var checkedRow = RowIndex - movement.RowMovement;
                var checkedCol = ColIndex - movement.ColMovement;
                var isThereTile = Board.IsThereTile(checkedRow, checkedCol);
                if(!isThereTile) continue;
                var isNeighbourFilled = Board.IsTileFilled(checkedRow, checkedCol);
                if (!isNeighbourFilled) continue;
                var newItem = Board.GetItem(checkedRow,checkedCol);
                Board.SwapItemTile(newItem,this);
                newItem.UpdatePosition(() =>
                {
                    isLocked = true;
                }, () =>
                {
                    isLocked = false;
                });
                break;
            }
        }

        public static bool IsNeighbour(Tile tile1, Tile tile2)
        {
            var rowDiff = Mathf.Abs(tile1.RowIndex-tile2.RowIndex);
            var collDiff = Mathf.Abs(tile1.ColIndex-tile2.ColIndex);
            return rowDiff <= 1 && collDiff <= 1;
        }
    }
}