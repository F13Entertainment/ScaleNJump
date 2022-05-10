using System;
using TMPro;
using UnityEngine;

namespace _GAME.Scripts.Core
{
    public abstract class BaseUpdateableText<T> : MonoBehaviour where T:IEquatable<T>,IFormattable
    {
        [SerializeField] private TextMeshProUGUI _tmp;
        protected T lastValue;
        protected abstract string ValueToString();
        protected abstract T Value();

        public TextMeshProUGUI TMP => _tmp;

        
        private void LateUpdate()
        {
            if (!lastValue.Equals(Value()))
            {
                _tmp.text = ValueToString();
                lastValue = Value();
            }
        }
    }
}