# WpfCalculator2025

WpfCalculator2025は、WPF (Windows Presentation Foundation) を使用して作成されたデスクトップ電卓アプリケーションです。基本的な四則演算機能に加え、入力桁数制限や表示桁数の調整機能を備えています。開発にはMVVM (Model-View-ViewModel) パターンが採用されており、UIロジックとビジネスロジックの分離が図られています。

## 主な機能

* 基本的な四則演算（加算、減算、乗算、除算）
* 数値入力 (0-9) および小数点入力
* 入力クリア機能 (C)
* **入力桁数制限**:
    * 整数部: 最大12桁
    * 小数部: 最大5桁
    * 上記制限を超える入力は無視されます（正規表現により制御）。
* **表示桁数制御**:
    * 計算結果および表示は、小数点以下5桁で切り捨てられます。
* **エラーハンドリング**:
    * 計算時の桁あふれ（`OverflowException`）やその他の例外を捕捉し、エラーメッセージを表示します。
    * 0除算などの演算エラーにも対応（`ComputeProcessDispatcher`および各演算処理クラス内で処理）。

## 技術スタック

* **フレームワーク**: WPF (.NET)
* **言語**: C#
* **アーキテクチャパターン**: MVVM (Model-View-ViewModel)
* **MVVMライブラリ**: CommunityToolkit.Mvvm
* **ロギング**: log4net

## プロジェクト構造

本プロジェクトは、主に以下の要素で構成されています。

* **View (`WpfCalculator2025/View/MainWindow.xaml`)**:
    * 電卓のユーザーインターフェースを定義します。
    * `Grid`コントロールを使用して、表示部とキーパッドを配置しています。
    * ボタンのクリックイベントは、ViewModelの`RelayCommand`にバインドされています。
    * 表示部の`TextBlock`は`Viewbox`内に配置され、ウィンドウサイズに応じてスケーリングされます。
* **ViewModel (`WpfCalculator2025/ViewModel/MainViewModel.cs`)**:
    * UIのロジックと状態を管理します。`CommunityToolkit.Mvvm`の`ObservableObject`を継承しています。
    * `DisplayText`プロパティ: 電卓の表示内容を保持し、変更をUIに通知します。
    * `KeyInputCommand` (`RelayCommand`): キーパッドのボタンからの入力を受け付け、`CalculatorContext`に処理を委譲します。
    * `FormatTruncate`メソッド: 表示文字列を小数点以下5桁に切り捨てる処理を行います。
* **Model (`WpfCalculator2025/Model/`)**:
    * **`CalculatorContext.cs`**:
        * 電卓のコアとなるロジックと状態（現在の入力値、演算子、オペランドなど）を保持・管理します。
        * `CommandExecuter`メソッド: ViewModelから渡されたキー入力を解釈し、数値入力 (`AddDigit`)、演算子設定 (`SetOperator`)、計算実行 (`Compute`)、クリア (`ClearAll`) などの内部処理を呼び出します。
        * 入力桁数制限（整数部`MaxIntegerDigits`、小数部`MaxFractionDigits`）は、コンストラクタで生成される正規表現`_inputPatternReg`を用いて、`AddDigit`メソッド内で検証されます。
    * **`ComputeProcessDispatcher.cs`**:
        * 演算子（`+`, `-`, `*`, `/`）に対応する具体的な計算処理クラスを割り当てるディスパッチャです。
        * `IComputeProcess`インターフェースを実装した各演算処理クラス（例: `AdditionComputeProcess`）を管理し、要求された演算を実行します。
        * 計算時の例外処理（`OverflowException`など）もここで行われます。
    * **`IComputeProcess.cs` (およびその実装群)**:
        * 各演算処理（加算、減算など）の共通インターフェースを定義します。
        * `Operator`プロパティ（処理対象の演算子を示す文字列）と`Compute`メソッド（実際の計算を行う）を持ちます。

## セットアップとビルド

1.  **.NET SDK**: プロジェクトに対応したバージョンの.NET SDKがインストールされていることを確認してください。
2.  **IDE**: Visual Studio 2022 (または互換性のあるIDE) を使用してプロジェクトを開きます。
3.  **依存関係**: 必要なNuGetパッケージ (CommunityToolkit.Mvvm, log4net) は、プロジェクトファイル (`.csproj`) に基づいて復元されます。
4.  **ビルド**: IDEのビルド機能を使用してプロジェクトをビルドします。
5.  **(log4netの設定)**: log4netを使用するため、適切な設定ファイル (`log4net.config` または `App.config` 内での設定) がプロジェクトに存在し、正しく構成されている必要があります。(本コードには設定ファイルの詳細は含まれていません)

## コード解説

### View (MainWindow.xaml)

```xaml
<Window x:Class="WpfCalculator2025.View.MainWindow"
        xmlns="[http://schemas.microsoft.com/winfx/2006/xaml/presentation](http://schemas.microsoft.com/winfx/2006/xaml/presentation)"
        xmlns:x="[http://schemas.microsoft.com/winfx/2006/xaml](http://schemas.microsoft.com/winfx/2006/xaml)"
        xmlns:d="[http://schemas.microsoft.com/expression/blend/2008](http://schemas.microsoft.com/expression/blend/2008)"
        xmlns:mc="[http://schemas.openxmlformats.org/markup-compatibility/2006](http://schemas.openxmlformats.org/markup-compatibility/2006)"
        xmlns:local="clr-namespace:WpfCalculator2025"
        xmlns:viewmodel="clr-namespace:WpfCalculator2025.ViewModel"
        mc:Ignorable="d"
        Title="MainWindow" Height="500" Width="400"
        d:DataContext="{d:DesignInstance Type=viewmodel:MainViewModel, IsDesignTimeCreatable=True}">
    <Window.DataContext>
        <viewmodel:MainViewModel />
    </Window.DataContext>
    </Window>
```
MainWindowのDataContextにはMainViewModelのインスタンスが設定され、MVVMパターンに基づいたデータバインディングとコマンド実行が行われます。UIはGridでレイアウトされ、各ボタンはKeyInputCommandにバインドされています。

### ViewModel (MainViewModel.cs)
```cs
internal partial class MainViewModel : ObservableObject
{
    private const int MaxDecimals = 5; // 表示用切り捨て桁数
    private CalculatorContext _calculatorContext = new CalculatorContext();
    // ...
    [ObservableProperty]
    private string displayText;

    [RelayCommand]
    private void KeyInput(string key)
    {
        _calculatorContext.CommandExecuter(key);
        var raw = _calculatorContext.DisplayText;
        DisplayText = FormatTruncate(raw, MaxDecimals); // 表示用に整形
    }
    // ... (FormatTruncate メソッド) ...
}
```
MainViewModelは、ユーザー入力（キー）を受け取り、CalculatorContextに処理を委譲します。CalculatorContextから返された表示用文字列をFormatTruncateメソッドで整形し、DisplayTextプロパティを通じてUIに反映させます。

### Model (CalculatorContext.cs)
```cs
public class CalculatorContext
{
    private const int MaxIntegerDigits = 12;
    private const int MaxFractionDigits = 5;
    private string _inputPatternReg; // 入力検証用正規表現
    // ... (状態変数定義) ...

    public CalculatorContext()
    {
        _inputPatternReg = <span class="math-inline">@"^\\d\{\{1,\{MaxIntegerDigits\}\}\}\(\\\.\\d\{\{0,\{MaxFractionDigits\}\}\}\)?</span>";
    }

    public void AddDigit(string input)
    {
        // ... (入力状態の管理とリセットロジック) ...
        if (input == ".")
        {
            if (!_currentInput.Contains('.')) _currentInput += input;
        }
        else // 数字入力の場合
        {
            // 正規表現で入力後の文字列を検証し、パターンにマッチする場合のみ入力を追加
            if (Regex.IsMatch(_currentInput + input, _inputPatternReg))
            {
                _currentInput += input;
            }
            else
            {
                _logger.Warn("入力最大桁数制限のため入力無視しました");
            }
        }
        _displayText = _currentInput;
    }
    // ... (CommandExecuter, SetOperator, Compute, ClearAll メソッド) ...
}
```
CalculatorContextは電卓の頭脳部分です。状態を保持し、入力に基づいて状態を遷移させ、計算を実行します。入力桁数制限は、AddDigitメソッド内で正規表現_inputPatternRegを使用して行われます。この正規表現は、整数部が1～MaxIntegerDigits桁、小数部が0～MaxFractionDigits桁の数値パターンにマッチするかを検証します。

### Model (ComputeProcessDispatcher.cs)
```cs
class ComputeProcessDispatcher
{
    private readonly IComputeProcess[] _processMap;

    public ComputeProcessDispatcher()
    {
        _processMap =
        [
            new AdditionComputeProcess(),
            new DivisionComputeProcess(),
            new MultiplicationComputeProcess(),
            new SubtractionComputeProcess(),
        ];
    }

    public (bool IsSuccess, decimal ResultValue, string Messsage) Executer(string processKey, decimal operandA, decimal operandB)
    {
        // ... (演算処理の呼び出しと例外処理) ...
    }

    private IComputeProcess Dispatch(string processKey)
    {
        return _processMap.FirstOrDefault(p => p.Operator == processKey) ?? throw new KeyNotFoundException($"未登録のprocessKey:{processKey}");
    }
}
```
ComputeProcessDispatcherは、指定された演算子文字列 (processKey) に基づいて、登録されているIComputeProcess実装の中から適切なものを選択し、計算を実行します。これにより、新しい演算処理を追加する際には、IComputeProcessを実装したクラスを作成し、_processMapに登録するだけで対応できます。

## 設計の特徴
- MVVMパターン: UI (View)、UIロジック (ViewModel)、ビジネスロジック (Model) が明確に分離されており、保守性・テスト性が向上しています。
- コマンドパターン: UIからのアクションはViewModelのRelayCommandを通じて処理され、イベントハンドラの直接的な記述を避けています。
- ストラテジーパターン (類似): IComputeProcessインターフェースと具象演算クラスにより、演算アルゴリズムを柔軟に切り替え・追加可能な構造になっています。
- 入力検証: CalculatorContext内で正規表現を用いた入力桁数制限が行われます。
- 設定値の定数化: 整数部・小数部の最大桁数などが定数として管理されており、変更が容易です。

## 今後の課題・改善点
- CalculatorContext内の_operandB変数の使途・状態管理の明確化。
- 各種初期化値（例: "0"）の定数化の推進。
- エラーメッセージ（例: "桁上限を超えました"）の文字列リソース化による多言語対応や一元管理。
- ComputeProcessDispatcherのExecuterメソッドの責務がディスパッチ以外の計算実行も担っているため、これを別クラスに分離するリファクタリングの検討。
- 演算子を現在の文字列ベースから、より型安全なEnumなどに変更することの検討。
- CalculatorContext内の各メソッドにおけるパラメータチェックの強化。
- 表示上の桁丸めだけでなく、計算プロセス全体を通じた計算精度の仕様を明確に定義すること。

このREADMEが、WpfCalculator2025プロジェクトの理解と利用の一助となれば幸いです。
