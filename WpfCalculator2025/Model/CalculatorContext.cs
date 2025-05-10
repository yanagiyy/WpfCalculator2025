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

        // 計算処理割り当て機能
        private readonly ComputeProcessDispatcher _computeProcessDispatcher = new ComputeProcessDispatcher();

        /// <summary>
        /// 表示用文字列
        /// </summary>
        /// <returns></returns>
        public string DisplayText => string.IsNullOrEmpty(_displayText) ? InitialDisplayTex : _displayText;

        /// <summary>
        /// 演算子
        /// </summary>
        public string Operator => _operator;

        /// <summary>
        /// 被オペラント
        /// </summary>
        public string PreviousInput => _previousInput;

        /// <summary>
        /// 現在入力
        /// </summary>
        public string CurrentInput => _currentInput;

        /// <summary>
        /// 直前計算結果
        /// </summary>
        public string LastResult => _lastResult;

        /// <summary>
        /// 演算実行直後判定
        /// </summary>
        /// <returns></returns>
        private bool isComputeJustEnterd => _lastInput == "=";

        /// <summary>
        /// 計算可能状態判定
        /// </summary>
        /// <returns></returns>
        private bool canConpute =>
                _previousInput != InitialPreviousInput
                && _operator != InitialOperator
                && _currentInput != InitialCurrentInput;

        /// <summary>
        /// 現在入力値への桁追加
        /// </summary>
        /// <param name="input"></param>
        public void AddDigit(string input)
        {
            _currentInput += input;
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
            _displayText = InitialDisplayTex;
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

            if (canConpute || (canConpute && isComputeJustEnterd))
            {
                var previousValue = decimal.Parse(_previousInput);
                var currentValue = decimal.Parse(_currentInput);

                // 演算子に該当する処理
                var computeProcess = _computeProcessDispatcher.Dispatch(_operator);
                var result = computeProcess.Compute(previousValue, currentValue);

                _displayText = result.ToString();
                _lastResult = result.ToString();
            }
            else
            {
                // 計算した状態にする
                var input = string.IsNullOrEmpty(_currentInput) ? "0" : _currentInput;
                _previousInput = input;
                _lastResult = input;
            }

            _lastInput = inputOperator;
        }

        /// <summary>
        /// 演算子設定
        /// </summary>
        /// <param name="inputOperator">演算子文字列</param>
        public void SetOperator(string inputOperator)
        {
            // 演算子で計算する場合の判定
            if (canConpute && _lastResult == InitialLastResult)
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

                _currentInput = InitialCurrentInput;
            }

            _operator = inputOperator;
            _lastInput = inputOperator;
        }
    }
}