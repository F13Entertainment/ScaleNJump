using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace F13StandardUtils.Gate.NormalizedGate.Scripts
{
    public enum OperatorType
    {
        SubAdd,
        DivMul
    }

    public class NormalizedGate : MonoBehaviour
    {
        [SerializeField,Range(0f,1f),OnValueChanged(nameof(UpdateOperatorNormalizedValue))] private float _operatorNormalizedValue;
        [SerializeField,OnValueChanged(nameof(UpdateOperatorValue))] private float _operatorValue;
        [SerializeField,ReadOnly] private float _lastOperatorValue;


        [SerializeField,OnValueChanged(nameof(UpdateOperatorNormalizedValue))] private OperatorType _operatorType;
        [SerializeField,OnValueChanged(nameof(UpdateOperatorNormalizedValue))] private float minValue;
        [SerializeField,OnValueChanged(nameof(UpdateOperatorNormalizedValue))] private float maxValue;

    
        [SerializeField] private TextMeshProUGUI _operatorText;
        [SerializeField] private Image panelImage;
        [SerializeField] private Image sliderImage;
        [SerializeField] private GameObject blueParticle;
        [SerializeField] private GameObject redParticle;
        [ShowInInspector] private static Color blueColor = new Color(0.32f, 0.84f, 1, 0.95f);
        [ShowInInspector] private static Color redColor =  new Color(1, 0, 0, 0.95f);
   

        public OperatorType OperatorType => _operatorType;

        public float OperatorNormalizedValue
        {
            get => _operatorNormalizedValue;
            set
            {
                _lastOperatorValue = _operatorValue;
                _operatorNormalizedValue = value;
                UpdateOperatorNormalizedValue();
            }
        }

        public float OperatorValue
        {
            get
            {
                return _operatorValue;
            }
            set
            {
                _lastOperatorValue = _operatorValue;
                _operatorValue = value;
                UpdateOperatorValue();
            }
        }
    
        private void UpdateOperatorValue()
        {
            _operatorValue = Mathf.Clamp(_operatorValue, minValue, maxValue);
            _operatorNormalizedValue = Mathf.InverseLerp(minValue, maxValue, _operatorValue);
            NotifyValue();
        }

    
        private void UpdateOperatorNormalizedValue()
        {
            _operatorValue = Mathf.Lerp(minValue, maxValue, _operatorNormalizedValue);
            NotifyValue();
        }
    
        private void NotifyValue()
        {
            if (_operatorValue >= 0 && _operatorValue <= 1.1f) _operatorValue = 1.1f;
            if (_operatorValue < 0 && _operatorValue >= -1.1f) _operatorValue = -1.1f;
            if(_operatorType==OperatorType.SubAdd) _operatorValue = (int) _operatorValue;
            UpdateUI();
        }
    
        private void Awake()
        {
            UpdateUI(true);
        }
    
        private void UpdateUI(bool instantUpdate=false)
        {
            sliderImage.fillAmount = OperatorNormalizedValue;
            var newcolor = OperatorValue>0? blueColor:redColor;
            if (panelImage.color != newcolor) panelImage.DOColor(newcolor, instantUpdate?0f : 0.25f);
            blueParticle.SetActive(OperatorValue > 0);
            redParticle.SetActive(OperatorValue <= 0);
            switch (OperatorType)
            {
                case OperatorType.SubAdd:
                    _operatorText.text =  (OperatorValue >= 0?"+":"-")+Mathf.Abs(OperatorValue).ToString("G3");
                    break;
                case OperatorType.DivMul:
                    _operatorText.text =  (OperatorValue >= 0?"x":"÷")+Mathf.Abs(OperatorValue).ToString("G3");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    
        [Button]
        public void SetIncrementalOperatorValue(float val)
        {
            if (_operatorValue >= 0 && _operatorValue <= 1.1f && _lastOperatorValue>=1.1f) _operatorValue = -1.1f;
            if (_operatorValue < 0 && _operatorValue >= -1.1f && _lastOperatorValue<=-1.1f) _operatorValue = 1.1f;
            OperatorValue = val;
        }
    }
}