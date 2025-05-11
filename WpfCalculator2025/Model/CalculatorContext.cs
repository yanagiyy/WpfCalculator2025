using System.Text.RegularExpressions;
using log4net;

namespace WpfCalculator2025.Model
{
    /// <summary>
    /// 電卓の状態保持制御クラス
    /// </summary>
    public class CalculatorContext
    {
        // 入力桁数制限値
        private const int MaxIntegerDigits = 12;
        private const int MaxFractionDigits = 5;

        private static readonly ILog _logger = LogManager.GetLogger(typeof(CalculatorContext));

        // 被演算対象の変数
        private decimal _operandA = 0M;

        // この変数は連続演算の際の保持に利用するが使い方があいまいになっているので整理したほうがよい
        private decimal? _operandB = null;

        // 初期化用の値は定数化すること
        private string _pendingOperation = "";
        private string _currentInput = "0";
        private string _displayText = "0";
        private bool _isWaitingNextOperand = false;

        private string _inputPatternReg;

        // 計算処理割り当て機能
        private readonly ComputeProcessDispatcher _computeProcessDispatcher = new ComputeProcessDispatcher();

        public CalculatorContext()
        {
            _inputPatternReg = $@"^\d{{1,{MaxIntegerDigits}}}(\.\d{{0,{MaxFractionDigits}}})?$";
        }

        /// <summary>
        /// 表示用文字列
        /// </summary>
        /// <returns></returns>
        public string DisplayText => _displayText;

        /// <summary>
        /// 演算子
        /// </summary>
        public string Operator => _pendingOperation;

        /// <summary>
        /// 現在入力
        /// </summary>
        public string CurrentInput => _currentInput;

        /// <summary>
        /// 操作実行
        /// </summary>
        /// <param name="key">操作</param>
        public void CommandExecuter(string key)
        {
            _logger.Info($"InputKey:{key}");

            try
            {
                // 数字キー or ドット
                if (char.IsDigit(key, 0) || key == ".")
                {
                    AddDigit(key);
                }
                // 演算子キー (+ - * /)
                else if (key == "+" || key == "-" || key == "*" || key == "/")
                {
                    SetOperator(key);
                }
                // イコールキー
                else if (key == "=")
                {
                    Compute();
                }
                // 全クリア（C）
                else if (key == "C")
                {
                    ClearAll();
                }
                // それ以外
                else
                {
                    // nop
                }
            }
            catch (OverflowException ex)
            {
                _logger.Error(ex.ToString());

                ClearAll();
                // ほかの場所とも統一のためにリソース化すること
                _displayText = "桁上限を超えました";

            }
            catch (Exception ex)
            {
                // 未ハンドリングの例外ハンドラー

                _logger.Error(ex.ToString());

                ClearAll();
                _displayText = ex.Message;
            }
        }

        /// <summary>
        /// 現在入力値への桁追加
        /// </summary>
        /// <param name="input"></param>
        public void AddDigit(string input)
        {
            // 今は呼び出し側で担保しているが必要に応じてパラメータチェック追加すること

            // 次の値入力待ち状態か判定する
            if (_isWaitingNextOperand)
            {
                // 初期化用の値は定数化すること
                _currentInput = "0";
                _isWaitingNextOperand = false;
            }

            // 初期状態であるか判定
            if (_currentInput == "0")
            {
                if (input == ".")
                {
                    // 小数点が未入力であれば入力する
                    _currentInput += input;
                }
                else
                {
                    _currentInput = input;
                }
            }
            else
            {
                if (input == ".")
                {
                    if (!_currentInput.Contains('.'))
                    {
                        // 小数点が未入力であれば入力する
                        _currentInput += input;
                    }
                }
                else
                {
                    if (Regex.IsMatch(_currentInput + input, _inputPatternReg))
                    {
                        _currentInput += input;
                    }
                    else
                    {
                        _logger.Warn("入力最大桁数制限のため入力無視しました");
                    }
                }
            }

            _displayText = _currentInput;
        }

        /// <summary>
        /// 全クリア
        /// </summary>
        public void ClearAll()
        {
            // 初期化用の値は定数化すること
            _currentInput = "0";
            _pendingOperation = "";
            _displayText = "0";

            _operandA = 0M;
            _operandB = null;

            _isWaitingNextOperand = false;
        }

        /// <summary>
        /// 演算実行
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Compute()
        {
            if (_pendingOperation != "")
            {
                if (_isWaitingNextOperand)
                {
                    // --数値が未入力（演算子や＝が入力された直後）の場合

                    if (_operandB == null)
                    {
                        _operandB = decimal.Parse(_currentInput);
                    }

                    _operandA = decimal.Parse(_currentInput);
                }
                else
                {
                    // --数値が入力されている場合

                    if (_operandB == null)
                    {
                        _operandB = decimal.Parse(_currentInput);
                    }
                    else
                    {
                        _operandA = decimal.Parse(_currentInput);
                    }
                }

                // 演算子に該当する演算処理を実行
                var result = _computeProcessDispatcher.Executer(_pendingOperation, _operandA, (decimal)_operandB);
                if (!result.IsSuccess)
                {
                    ClearAll();
                    _displayText = result.Messsage;
                    return;
                }

                _currentInput = result.ResultValue.ToString();
                _displayText = result.ResultValue.ToString();
            }
            else
            {
                // 演算子が未入力の場合 3= 0=のみ は入力値を被オペランドにする
                _operandA = decimal.Parse(_currentInput);
            }

            // 次の数値入力待ちとする
            _isWaitingNextOperand = true;
        }

        /// <summary>
        /// 演算子設定
        /// </summary>
        /// <param name="inputOperator">演算子文字列</param>
        public void SetOperator(string inputOperator)
        {
            // 今は呼び出し側で担保しているが必要に応じてパラメータチェック追加すること
            // 演算子は文字型ではなく定義を作成するのが望ましい

            // 演算子による演算実行を判定
            if (_pendingOperation != "" && !_isWaitingNextOperand)
            {
                decimal inputOperand = decimal.Parse(_currentInput);

                // 演算子に該当する演算処理を実行
                var result = _computeProcessDispatcher.Executer(_pendingOperation, _operandA, inputOperand);
                if (!result.IsSuccess)
                {
                    ClearAll();
                    _displayText = result.Messsage;
                }

                _operandA = result.ResultValue;
                _currentInput = result.ResultValue.ToString();
                _displayText = result.ResultValue.ToString();
            }
            else
            {
                if (_isWaitingNextOperand)
                {
                    _operandB = null;
                    _operandA = decimal.Parse(_currentInput);
                }
                else
                {
                    // 現在の入力値を被オペランドとして保存
                    _operandA = decimal.Parse(_currentInput);
                }
            }

            // 入力された演算子を保存
            _pendingOperation = inputOperator;

            // 次の数値入力待ちとする
            _isWaitingNextOperand = true;
        }
    }
}