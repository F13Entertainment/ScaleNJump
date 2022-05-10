using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace F13StandardUtils.MatchThree.Scripts
{
    public enum ItemType
    {
        Type0=0,
        Type1=1,
        Type2=2,
        Type3=3,
        Type4=4,
        Type5=5,
        Type6=6,
        Type7=7,
        Type8=8,
        Type9=9
    }
    
    [System.Serializable]
    public class ItemRatioPair
    {
        public ItemType item;
        [Range(0f,1f)]public float ratio;
    }
    
    [CreateAssetMenu(fileName = "LevelData", menuName = "_MatchThree/LevelData", order = 0)]
    public class LevelData: ScriptableObject
    {
        public BoardData board;
        public List<ItemMovementData> itemMovements;
        public List<BaseMatchRuleData> rules;
        [SerializeField] private List<ItemRatioPair> itemRatios=new List<ItemRatioPair>();

        public List<ItemType> Items
        {
            get
            {
                var list= new List<ItemType>();
                foreach (var itemRatioPair in itemRatios)
                {
                    list.Add(itemRatioPair.item);
                }
                return list;
            }
        }

        public float GetRatio(ItemType itemType)
        {
            var sum = itemRatios.Sum(i=>i.ratio);
            var itemRatioPair = itemRatios.Find(i=>i.item==itemType);
            return itemRatioPair.ratio / sum;
        }

        public ItemType RandomViaRatios()
        {
            var random = Random.Range(0f,1f);
            var items = Items;
            ItemType randomType = ItemType.Type0;
            foreach (var item in items)
            {
                var ratio = GetRatio(item);
                if (random > ratio)
                {
                    random -= ratio;
                }
                else
                {
                    randomType = item;
                    break;
                }
            }
            return randomType;
        }
        
    }
}