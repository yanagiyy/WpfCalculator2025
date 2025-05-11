using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using log4net;
using WpfCalculator2025.Model;

namespace WpfCalculator2025.ViewModel
{
    internal partial class MainViewModel : ObservableObject
    {
        private const int MaxDecimals = 5;

        private CalculatorContext _calculatorContext = new CalculatorContext();
        private static readonly ILog _logger = LogManager.GetLogger(typeof(MainViewModel));

        public MainViewModel()
        {
            displayText = _calculatorContext.DisplayText;
        }

        [ObservableProperty]
        private string displayText;

        [RelayCommand]
        private void KeyInput(string key)
        {
            _logger.Info($"InputKey:{key}");

            // 数字キー or ドット
            if (char.IsDigit(key, 0) || key == ".")
            {
                _calculatorContext.AddDigit(key);
            }
            // 演算子キー (+ - * /)
            else if (key == "+" || key == "-" || key == "*" || key == "/")
            {
                _calculatorContext.SetOperator(key);
            }
            // イコールキー
            else if (key == "=")
            {
                _calculatorContext.Compute();
            }
            // 全クリア（C）
            else if (key == "C")
            {
                _calculatorContext.ClearAll();
            }
            // それ以外
            else
            {
                // nop
            }

            var raw = _calculatorContext.DisplayText;
            DisplayText = FormatTruncate(raw, MaxDecimals);

            _logger.Info($"DisplayText:{DisplayText}");
        }


        /// <summary>
        /// 指定桁数で切り捨て　// とりあえず切り捨てしているが計算精度をどこに合わせるかをきめること
        /// </summary>
        /// <param name="text">元</param>
        /// <param name="maxDecimals">最大桁数</param>
        /// <returns>成型後文字列</returns>
        private static string FormatTruncate(string text, int maxDecimals)
        {
            int dot = text.IndexOf('.');
            if (dot < 0)
            {
                // 小数点がなければそのまま
                return text;
            }

            int fractionLength = text.Length - dot - 1;
            if (fractionLength <= maxDecimals)
            {
                // すでに5桁以下なら変更不要
                return text;
            }

            // 小数点＋5桁分だけを残して切り捨て
            return text.Substring(0, dot + 1 + maxDecimals);
        }
    }
}
