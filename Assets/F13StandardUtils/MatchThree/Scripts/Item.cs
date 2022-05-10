using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace F13StandardUtils.MatchThree.Scripts
{
    [System.Serializable]
    public class ItemEvent:UnityEvent<Item>{}
    public class Item : MonoBehaviour
    {
        [SerializeField,OnValueChanged(nameof(UpdateType))] private ItemType itemType;
        [SerializeField] private TextMeshPro _tmp;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private List<Color> _colors=new List<Color>();
        private bool isDrag = false;
        private bool isHover = false;
        private bool isSelected = false;
        public ItemEvent OnSelected=new ItemEvent();
        [ShowInInspector] private BoardManager Board => BoardManager.Instance;
        [ShowInInspector] public Color Color => _colors.Count!=10? Color.white:_colors[(int) itemType];
        [ShowInInspector] public Tile Tile => Board?Board.map[this]:null;
        [ShowInInspector] public bool IsSelected => isSelected;

        public ItemType ItemType => itemType;



        public void SetType(ItemType type)
        {
            itemType = type;
            UpdateType();
        }
        
        public void UpdatePosition(Action onStartAction=null,Action onEndAction=null)
        {
            transform.DOMove(Tile.transform.position, Board.animDuration).OnStart(() =>
            {
                onStartAction?.Invoke();
            }).OnComplete(() =>
            {
                onEndAction?.Invoke();
            });
        }

        private void UpdateType()
        {
            _tmp.text = ((int) itemType).ToString();
            _renderer.color = Board.isActive ? Color: Color.white*Color.grayscale;
        }
        
        private void CheckItemIsDragging()
        {
            if(!Board.isActive) return;
            if (Input.GetMouseButtonDown(0))
            {
                isDrag = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDrag = false;
                isSelected = false;
            }

            if (isDrag && isHover && !isSelected)
            {
                isSelected = true;
                OnSelected.Invoke(this);
            }
        }
        
        private void Update()
        {
            UpdateIsActiveColor();
            CheckItemIsDragging();
        }

        private bool lastIsActive = true;
        private void UpdateIsActiveColor()
        {
            if (lastIsActive != Board.isActive)
            {
                lastIsActive = Board.isActive;
                UpdateType();
                ResetItem();
            }
        }

        private void OnMouseEnter()
        {
            isHover = true;
        }

        private void OnMouseExit()
        {
            isHover = false;
        }

        private void OnEnable()
        {
            ResetItem();
        }

        private void ResetItem()
        {
            isDrag = false;
            isHover = false;
            isSelected = false;
        }
    }
}