using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace F13StandardUtils.MatchThree.Scripts
{
    [CreateAssetMenu(fileName = "SameNeighbourMatchRuleData", menuName = "_MatchThree/MatchRule/SameNeighbourMatchRuleData")]
    public class SameNeighbourMatchRuleData: BaseMatchRuleData
    {
        [SerializeField] private int _minMatchCount = 2;
        private float _scaleUp = 1.5f;
        private List<Item> sameList=new List<Item>();
        public override void ItemSelected(Item item)
        {
            if (!sameList.Any())
            {
                sameList.Add(item);
                item.transform.localScale=Vector3.one*_scaleUp;
            }
            else
            {
                var first = sameList.First();
                var isSame = first.ItemType == item.ItemType;
                if (isSame && IsNeighbourToSelection(item))
                {
                    sameList.Add(item);
                    item.transform.localScale=Vector3.one*_scaleUp;
                }
            }
        }

        private bool IsNeighbourToSelection(Item check)
        {
            var result = false;
            foreach (var item in sameList)
            {
                var checkTile = check.Tile;
                var itemTile = item.Tile;
                if (Tile.IsNeighbour(checkTile, itemTile))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public override void DragEnter()
        {
            sameList.Clear();
        }

        public override void DragExit()
        {
            sameList.ForEach(s=>s.transform.localScale = Vector3.one);
            if (sameList.Count >= _minMatchCount)
            {
                BoardManager.Instance.RuleSuccess(this,sameList);
                sameList.ForEach(s=>BoardManager.Instance.Kill(s));
            }
            else
            {
                BoardManager.Instance.RuleFail(this,sameList);
            }

        }
    }
}