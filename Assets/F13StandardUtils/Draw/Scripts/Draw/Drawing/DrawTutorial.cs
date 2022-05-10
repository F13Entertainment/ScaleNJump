using System.Collections.Generic;
using F13StandardUtils.Draw.Scripts.Draw.DrawHistogramAnalysis;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Draw.Scripts.Draw.Drawing
{
    public class DrawTutorial : MonoBehaviour
    {
        [SerializeField,OnValueChanged(nameof(UpdateType))] private DrawTypes _drawType;
        [SerializeField] private List<GameObject> _list = new List<GameObject>();

        public void SetType(DrawTypes draw)
        {
            _drawType = draw;
            UpdateType();
        }

        private void UpdateType()
        {
            for (var index = 0; index < _list.Count; index++)
            {
                var o = _list[index];
                o.SetActive(index == (int)_drawType);
            }
        }

        public void DisableAll()
        {
            for (var index = 0; index < _list.Count; index++)
            {
                var o = _list[index];
                o.SetActive(false);
            }
        }
    }
}
