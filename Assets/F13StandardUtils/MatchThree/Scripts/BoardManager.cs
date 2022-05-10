using System;
using System.Collections.Generic;
using DG.Tweening;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace F13StandardUtils.MatchThree.Scripts
{
    [System.Serializable]
    public class RuleEvent:UnityEvent<BaseMatchRuleData,List<Item>>{}
    public class BoardManager : Singleton<BoardManager>
    {
        [SerializeField] private List<LevelData> boardDataList;
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private int level;
        [SerializeField] private bool isDrag = false;
        private Tile[,] boardMatrix;

        [ReadOnly] public List<Item> selectedItems =new List<Item>();
        [ReadOnly] public  BiDictionary<Tile,Item> map=new BiDictionary<Tile, Item>();
        public float animDuration = 0.1f;
        public bool isActive = true;
        
        public UnityEvent OnDragEnter =new UnityEvent();
        public UnityEvent OnDragExit =new UnityEvent();
        public ItemEvent OnItemSelected =new ItemEvent();
        public RuleEvent OnRuleSuccess =new RuleEvent();
        public RuleEvent OnRuleFail =new RuleEvent();


        public bool IsDrag => isDrag;
        [ShowInInspector] public LevelData CurrentLevelData => boardDataList[level];
        [ShowInInspector] public BoardData CurrentBoardData => CurrentLevelData.board;
        
        private void Awake()
        {
            UpdateLevel();
        }

        private void Update()
        {
            CheckDragging();
        }
        
        private void OnSelectedItem(Item item)
        {
            selectedItems.Add(item);
            CurrentLevelData.rules.ForEach(r=>r.ItemSelected(item));
            OnItemSelected.Invoke(item);
        }
        
        public void SetLevel(int l)
        {
            this.level = l;
            UpdateLevel();
        }

        private void UpdateLevel()
        {
            level %= boardDataList.Count;
            BuildBoard();
        }

        private void CheckDragging()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDrag = true;
                CurrentLevelData.rules.ForEach(r=>r.DragEnter());
                OnDragEnter.Invoke();
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDrag = false;
                CurrentLevelData.rules.ForEach(r=>r.DragExit());
                selectedItems.Clear();
                OnDragExit.Invoke();
            }
        }
        
        [Button]
        private void BuildBoard()
        {
            BuildTiles();
            BuildItems();
            
        }
        
        private void BuildTiles()
        {
            ClearTiles();
            CreateTiles();
        }

        
        private void ClearTiles()
        {
            if(boardMatrix==null) 
                return;
            
            foreach (var tile in boardMatrix)
            {
                if(tile)
                    PoolManager.Instance.Destroy(tile);
            }
            boardMatrix = null;
        }
        
        private void CreateTiles()
        {
            boardMatrix=new Tile[CurrentBoardData.rows,CurrentBoardData.cols];
            for (var rowIndex = 0; rowIndex < CurrentBoardData.rows; rowIndex++)
            {
                for (var colIndex = 0; colIndex < CurrentBoardData.cols; colIndex++)
                {
                    var tileType = CurrentBoardData.GetValue(rowIndex,colIndex);
                    if (tileType!=TileType.None)
                    {
                        var tile = PoolManager.Instance.Instantiate<Tile>(tilePrefab);
                        tile.SetTileIndex(rowIndex,colIndex);
                        tile.tileType = tileType;
                        boardMatrix[rowIndex, colIndex] = tile;
                    }
                }
            }
        }
        
        private void BuildItems()
        {
            ClearItems();
            CreateItems();
        }
        
        [Button]
        private void ClearItems()
        {
            foreach (var item in map.GetSecondValues())
            {
                if (item)
                {
                    map.Remove(item);
                    PoolManager.Instance.Destroy(item);
                }
            }
        }
        
        private void CreateItems()
        {
            foreach (var tile in boardMatrix)
            {
                if (tile)
                {
                    CreateItem(tile);
                }
            }
        }
        
        
        public Item CreateItem(Tile tile)
        {
            var item = PoolManager.Instance.Instantiate<Item>(itemPrefab);
            var randomType = CurrentLevelData.RandomViaRatios();
            item.SetType(randomType);
            item.transform.position = tile.transform.position;
            item.OnSelected.AddListener(OnSelectedItem);
            map.Add(tile, item);
            return item;
        }

        public Item CreateItemAnimated(Tile tile, Action startAction=null, Action endAction=null)
        {
            var item = CreateItem(tile);
            item.transform.localScale=Vector3.zero;
            item.transform.DOScale(Vector3.one, animDuration).OnStart(() =>
            {
                startAction?.Invoke();
            }).OnComplete(() =>
            {
                endAction?.Invoke();
            });
            return item;
        }
        
        public void Kill(Item item)
        {
            item.OnSelected.RemoveListener(OnSelectedItem);
            PoolManager.Instance.Destroy(item);
            map.Remove(item);
        }

        public Tile GetTile(int rowIndex, int colIndex)
        {
            return boardMatrix[rowIndex,colIndex];
        }
        public Item GetItem(int rowIndex, int colIndex)
        {
            return GetTile(rowIndex, colIndex).Item;
        }

        public void SwapItemTile(Item item, Tile newTile)
        {
            map.Remove(item);
            map.Add(newTile, item);
        }
        
        public bool IsThereTile(int rowIndex, int colIndex)
        {
            if (rowIndex<0 || rowIndex >= boardMatrix.GetLength(0))
            {
                return false;
            }
            if (colIndex<0 || colIndex >= boardMatrix.GetLength(1))
            {
                return false;
            }
            
            var tile = boardMatrix[rowIndex,colIndex];
            return tile != null;
        }

        public bool IsTileFilled(int rowIndex, int colIndex)
        {
            return boardMatrix[rowIndex,colIndex].IsFilled;
        }

        public void RuleSuccess(BaseMatchRuleData rule, List<Item> itemList)
        {
            OnRuleSuccess.Invoke(rule,itemList);
        }
        public void RuleFail(BaseMatchRuleData rule, List<Item> itemList)
        {
            OnRuleFail.Invoke(rule,itemList);

        }
    }
}
