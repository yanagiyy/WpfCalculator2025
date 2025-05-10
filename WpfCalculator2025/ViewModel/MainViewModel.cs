using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfCalculator2025.Model;

namespace WpfCalculator2025.ViewModel
{
    internal partial class MainViewModel : ObservableObject
    {

        private CalculatorContext _calculatorContext = new CalculatorContext();

        public MainViewModel()
        {
            displayText = _calculatorContext.DisplayText;
        }

        [ObservableProperty]
        private string displayText;

        [RelayCommand]
        private void KeyInput(string key)
        {
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

            DisplayText = _calculatorContext.DisplayText;
        }
    }
}
