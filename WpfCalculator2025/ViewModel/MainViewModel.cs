using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WpfCalculator2025.ViewModel
{
    internal partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string displayText = "0";

        [RelayCommand]
        private void KeyInput(string value)
        {
            // とりあえず入力コマンドを表示できるように
            DisplayText = value;
        }
    }
}
