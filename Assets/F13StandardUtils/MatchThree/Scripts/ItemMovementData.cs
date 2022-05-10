using UnityEngine;

namespace F13StandardUtils.MatchThree.Scripts
{
    [CreateAssetMenu(fileName = "ItemMovement", menuName = "_MatchThree/ItemMovement", order = 0)]
    public class ItemMovementData : ScriptableObject
    {
        [SerializeField, Range(-1,1)] private int _rowMovement;
        [SerializeField, Range(-1,1)] private int _colMovement;

        public int RowMovement => _rowMovement;
        public int ColMovement => _colMovement;
        public ItemMovementData(int rowMovement, int colMovement)
        {
            this._rowMovement = rowMovement;
            this._colMovement = colMovement;
        }
    }
}