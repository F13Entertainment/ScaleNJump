using UnityEngine;

namespace F13StandardUtils.MatchThree.Scripts
{
    public abstract class BaseMatchRuleData:ScriptableObject
    {
        public abstract void ItemSelected(Item item);
        public abstract void DragEnter();
        public abstract void DragExit();
    }
}