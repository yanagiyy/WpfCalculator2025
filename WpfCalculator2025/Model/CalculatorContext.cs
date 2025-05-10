namespace WpfCalculator2025.Model
{
    /// <summary>
    /// 電卓の状態保持制御クラス
    /// </summary>
    public class CalculatorContext
    {
        private const string InitialCurrentInput = "";
        private const string InitialDisplayTex = "0";
        private const string InitialOperator = "";
        private const string InitialPreviousInput = "";
        private const string InitialLastInput = "";
        private const string InitialLastResult = "";

        private string _currentInput = InitialCurrentInput;
        private string _operator = InitialOperator;
        private string _previousInput = InitialPreviousInput;
        private string _displayText = InitialDisplayTex;

        private string _lastInput = InitialLastInput;
        private string _lastResult = InitialLastResult;

        /// <summary>
        /// 表示用文字列
        /// </summary>
        /// <returns></returns>
        public string DisplayText
        {
            get
            {
                return string.IsNullOrEmpty(_currentInput) ? InitialDisplayTex : _displayText;
            }
        }

        /// <summary>
        /// 演算子
        /// </summary>
        public string Operator => _operator;

        /// <summary>
        /// 被オペラント
        /// </summary>
        public string PreviousInput => _previousInput;

        /// <summary>
        /// 演算実行直後判定
        /// </summary>
        /// <returns></returns>
        private bool isComputeJustEnterd => _lastInput == "=";

        /// <summary>
        /// 演算子入力直後判定
        /// </summary>
        /// <returns></returns>
        private bool isOperatorJustEnterd => "+-*/".IndexOf(_lastInput) >= 0;

        /// <summary>
        /// 計算可能状態判定
        /// </summary>
        /// <returns></returns>
        private bool canConpute =>
         !string.IsNullOrEmpty(_previousInput)
                && !string.IsNullOrEmpty(_operator)
                && !isComputeJustEnterd
                && !isOperatorJustEnterd;

        /// <summary>
        /// 入力桁追加
        /// </summary>
        /// <param name="input"></param>
        public void AddDigit(string input)
        {

            if (string.IsNullOrEmpty(_currentInput) || isOperatorJustEnterd)
            {
                _currentInput = input;
            }
            else
            {
                _currentInput += input;
            }

            _displayText = _currentInput;
            _lastInput = input;
        }

        /// <summary>
        /// 全クリア
        /// </summary>
        public void ClearAll()
        {
            _currentInput = InitialCurrentInput;
            _operator = InitialOperator;
            _previousInput = InitialPreviousInput;
            _lastInput = InitialLastInput;
            _lastResult = InitialLastResult;
        }

        /// <summary>
        /// 演算実行
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Compute(string inputOperator = "=")
        {
            if (isComputeJustEnterd)
            {
                // 連続演算の場合は直前の結果に対して同じ計算を繰り返す
                _previousInput = _lastResult;
            }

            switch (_operator)
            {
                case "+":
                    var previousValue = decimal.Parse(_previousInput);
                    var currentValue = decimal.Parse(_currentInput);

                    var resut = previousValue + currentValue;

                    _displayText = resut.ToString();
                    _lastResult = resut.ToString();
                    break;
                default:
                    break;
            }

            _lastInput = inputOperator;
        }

        /// <summary>
        /// 演算子設定
        /// </summary>
        /// <param name="inputOperator">演算子文字列</param>
        public void SetOperator(string inputOperator)
        {
            if (canConpute)
            {
                Compute(inputOperator);
            }

            if (_currentInput == InitialCurrentInput)
            {
                // 現在値が未入力での演算子設定時はゼロとする
                _previousInput = "0";
            }
            else
            {
                if (isComputeJustEnterd)
                {
                    _previousInput = _lastResult;
                }
                else
                {
                    _previousInput = _currentInput;
                }
            }

            _operator = inputOperator;
            _lastInput = inputOperator;
        }
    }
}