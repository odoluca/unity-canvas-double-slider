#region Includes
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
#endregion

namespace TS.DoubleSlider
{
    [RequireComponent(typeof(RectTransform))]
    public class DoubleSlider : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private Slider _sliderLower;
        [SerializeField] private TextMeshProUGUI _lowerLabel;
        [SerializeField] private Slider _sliderHigher;
        [SerializeField] private TextMeshProUGUI _higherLabel;
        [SerializeField] private string _labelFormat;

        [SerializeField] private RectTransform _fillArea;

        [Header("Configuration")]
        [SerializeField] private bool _setupOnStart;
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;
        [SerializeField] private float _minDistance;
        [SerializeField] private bool _wholeNumbers;
        [SerializeField] private float _initialLowerValue;
        [SerializeField] private float _initialHigherValue;

        [Header("Events")]
        public UnityEvent<float,float> OnValueChanged;

        public bool IsEnabled
        {
            get { return _sliderHigher.enabled && _sliderLower.enabled; }
            set
            {
                _sliderLower.enabled = value;
                _sliderHigher.enabled = value;
            }
        }
        public float MinValue
        {
            get { return _sliderLower.value; }
        }
        public float MaxValue
        {
            get { return _sliderHigher.value; }
        }
        public bool WholeNumbers
        {
            get { return _wholeNumbers; }
            set
            {
                _wholeNumbers = value;

                _sliderLower.wholeNumbers = _wholeNumbers;
                _sliderHigher.wholeNumbers = _wholeNumbers;
            }
        }

        private RectTransform _fillRect;

        #endregion

        private void Awake()
        {
            if (_sliderLower == null || _sliderHigher == null)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD

                Debug.LogError("Missing slider min: " + _sliderLower + ", max: " + _sliderHigher);
#endif
                return;
            }

            if (_fillArea == null)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD

                Debug.LogError("Missing fill area");
#endif
                return;
            }

            _fillRect = _fillArea.transform.GetChild(0).transform as RectTransform;
        }
        private void Start()
        {
            if (!_setupOnStart) { return; }
            Setup(_minValue, _maxValue, _initialLowerValue, _initialHigherValue);

        }

        public void Setup( float lowerValue, float higherValue)
        {
            _sliderLower.value = lowerValue;
            _sliderHigher.value = higherValue;

            MinValueChanged(_initialLowerValue);
            MaxValueChanged(_initialHigherValue);

        }

        public void Setup(float minValue, float maxValue, float initialLowerValue, float initialHigherValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;

            _initialLowerValue = initialLowerValue;
            _initialHigherValue = initialHigherValue;

            //_sliderMin.Setup(_initialMinValue, minValue, maxValue, MinValueChanged);
            //_sliderMax.Setup(_initialMaxValue, minValue, maxValue, MaxValueChanged);


            _sliderLower.minValue = minValue;
            _sliderHigher.minValue = minValue;
            _sliderLower.maxValue = maxValue;
            _sliderHigher.maxValue = maxValue;

            _sliderLower.onValueChanged.RemoveAllListeners();
            _sliderHigher.onValueChanged.RemoveAllListeners();
            _sliderLower.onValueChanged.AddListener(MinValueChanged);
            _sliderHigher.onValueChanged.AddListener(MaxValueChanged);

            _sliderLower.value = initialLowerValue;
            _sliderHigher.value = initialHigherValue;

            MinValueChanged(_initialLowerValue);
            MaxValueChanged(_initialHigherValue);

        }

        private void MinValueChanged(float value)
        {
            float offset = ((MinValue - _minValue) / (_maxValue - _minValue)) * _fillArea.rect.width;

            _fillRect.offsetMin = new Vector2(offset, _fillRect.offsetMin.y);

            if ((MaxValue - value) < _minDistance)
            {
                _sliderLower.value = MaxValue - _minDistance;
            }


            OnValueChanged.Invoke(MinValue, MaxValue);
            _lowerLabel.text = _sliderLower.value.ToString(_labelFormat);
            _sliderLower.transform.SetAsLastSibling();
        }
        private void MaxValueChanged(float value)
        {
            float offset = (1 - ((MaxValue - _minValue) / (_maxValue - _minValue))) * _fillArea.rect.width;

            _fillRect.offsetMax = new Vector2(-offset, _fillRect.offsetMax.y);

            if ((value - MinValue) < _minDistance)
            {
                _sliderHigher.value = MinValue + _minDistance;
            }


            OnValueChanged.Invoke(MinValue, MaxValue);
            _higherLabel.text = _sliderHigher.value.ToString(_labelFormat);
            _sliderHigher.transform.SetAsLastSibling();
        }


        //public void Setup(Slider slider, float value, float minValue, float maxValue, UnityAction<float> valueChanged)
        //{
        //    slider.minValue = minValue;
        //    slider.maxValue = maxValue;

        //    slider.value = value;
        //    slider.onValueChanged.AddListener(Slider_OnValueChanged);
        //    slider.onValueChanged.AddListener(valueChanged);

        //    UpdateLabel();
        //}

        //private void Slider_OnValueChanged(float arg0)
        //{
        //    UpdateLabel();
        //}

        //protected virtual void UpdateLabel()
        //{
        //    if (_label == null) { return; }
        //    _label.text = Value.ToString();
        //}

    }
}

//#region Includes
//using UnityEngine;
//using UnityEngine.Events;
//#endregion

//namespace TS.DoubleSlider
//{
//    [RequireComponent(typeof(RectTransform))]
//    public class DoubleSlider : MonoBehaviour
//    {
//        #region Variables

//        [Header("References")]
//        [SerializeField] private SingleSlider _sliderMin;
//        [SerializeField] private SingleSlider _sliderMax;
//        [SerializeField] private RectTransform _fillArea;

//        [Header("Configuration")]
//        [SerializeField] private bool _setupOnStart;
//        [SerializeField] private float _minValue;
//        [SerializeField] private float _maxValue;
//        [SerializeField] private float _minDistance;
//        [SerializeField] private bool _wholeNumbers;
//        [SerializeField] private float _initialMinValue;
//        [SerializeField] private float _initialMaxValue;

//        [Header("Events")]
//        public UnityEvent<float, float> OnValueChanged;

//        public bool IsEnabled
//        {
//            get { return _sliderMax.IsEnabled && _sliderMin.IsEnabled; }
//            set
//            {
//                _sliderMin.IsEnabled = value;
//                _sliderMax.IsEnabled = value;
//            }
//        }
//        public float MinValue
//        {
//            get { return _sliderMin.Value; }
//        }
//        public float MaxValue
//        {
//            get { return _sliderMax.Value; }
//        }
//        public bool WholeNumbers
//        {
//            get { return _wholeNumbers; }
//            set
//            {
//                _wholeNumbers = value;

//                _sliderMin.WholeNumbers = _wholeNumbers;
//                _sliderMax.WholeNumbers = _wholeNumbers;
//            }
//        }

//        private RectTransform _fillRect;

//        #endregion

//        private void Awake()
//        {
//            if (_sliderMin == null || _sliderMax == null)
//            {
//#if UNITY_EDITOR || DEVELOPMENT_BUILD

//                Debug.LogError("Missing slider min: " + _sliderMin + ", max: " + _sliderMax);
//#endif
//                return;
//            }

//            if (_fillArea == null)
//            {
//#if UNITY_EDITOR || DEVELOPMENT_BUILD

//                Debug.LogError("Missing fill area");
//#endif
//                return;
//            }

//            _fillRect = _fillArea.transform.GetChild(0).transform as RectTransform;
//        }
//        private void Start()
//        {
//            if (!_setupOnStart) { return; }
//            Setup(_minValue, _maxValue, _initialMinValue, _initialMaxValue);

//        }

//        public void Setup(float minValue, float maxValue, float initialMinValue, float initialMaxValue)
//        {
//            _minValue = minValue;
//            _maxValue = maxValue;

//            _sliderMin.Value = initialMinValue;
//            _sliderMax.Value = initialMaxValue;
//            _initialMinValue = initialMinValue;
//            _initialMaxValue = initialMaxValue;

//            _sliderMin.Setup(_initialMinValue, minValue, maxValue, MinValueChanged);
//            _sliderMax.Setup(_initialMaxValue, minValue, maxValue, MaxValueChanged);

//            MinValueChanged(_initialMinValue);
//            MaxValueChanged(_initialMaxValue);
//        }

//        private void MinValueChanged(float value)
//        {
//            float offset = ((MinValue - _minValue) / (_maxValue - _minValue)) * _fillArea.rect.width;

//            _fillRect.offsetMin = new Vector2(offset, _fillRect.offsetMin.y);

//            if ((MaxValue - value) < _minDistance)
//            {
//                _sliderMin.Value = MaxValue - _minDistance;
//            }

//            OnValueChanged.Invoke(MinValue, MaxValue);
//            _sliderMin.transform.SetAsLastSibling();
//        }
//        private void MaxValueChanged(float value)
//        {
//            float offset = (1 - ((MaxValue - _minValue) / (_maxValue - _minValue))) * _fillArea.rect.width;

//            _fillRect.offsetMax = new Vector2(-offset, _fillRect.offsetMax.y);

//            if ((value - MinValue) < _minDistance)
//            {
//                _sliderMax.Value = MinValue + _minDistance;
//            }

//            OnValueChanged.Invoke(MinValue, MaxValue);
//            _sliderMax.transform.SetAsLastSibling();
//        }

//    }
//}