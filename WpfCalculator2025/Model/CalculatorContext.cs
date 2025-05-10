namespace WpfCalculator2025.Model
{
    /// <summary>
    /// 電卓の状態保持制御クラス
    /// </summary>
    public class CalculatorContext
    {
        /// <summary>
        /// 現在入力中の値
        /// </summary>
        private string _currentInput = string.Empty;

        /// <summary>
        /// 表示用文字列
        /// </summary>
        /// <returns></returns>
        public string DisplayText
        {
            get
            {
                return string.IsNullOrEmpty(_currentInput) ? "0" : _currentInput;
            }
        }
    }
}