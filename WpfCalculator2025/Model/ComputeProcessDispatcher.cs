using log4net;
using WpfCalculator2025.Model.ComputeProcess;

namespace WpfCalculator2025.Model
{
    /// <summary>
    /// 演算子に対応する処理を割り当てる機能
    /// </summary>
    class ComputeProcessDispatcher
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ComputeProcessDispatcher));
        private readonly IComputeProcess[] _processMap;

        public ComputeProcessDispatcher()
        {
            _processMap =
            [
                // 処理を追加する場合はIComputeProcessを実装した演算処理を登録してください
                new AddComputeProcess(),
                new DivisionComputeProcess(),
                new MultiplicationComputeProcess(),
                new SubtractionComputeProcess(),
            ];
        }

        /// <summary>
        /// 実行 // Dispatcherの責務ではないので別クラスにすることが望ましく専用クラスに移設してください
        /// </summary>
        /// <param name="processKey">演算子</param>
        /// <param name="operandA"></param>
        /// <param name="operandB"></param>
        /// <returns>計算結果</returns>
        public (bool IsSuccess, decimal ResultValue, string Messsage) Executer(string processKey, decimal operandA, decimal operandB)
        {
            try
            {
                var computeProcess = Dispatch(processKey);
                var result = computeProcess.Compute(operandA, operandB);

                var logMessage = $"Executer processKey:{processKey},operadA:{operandA},operandB:{operandB},result:{result}";
                _logger.Info(logMessage);

                // 成功
                return (true, result, string.Empty);
            }
            catch (Exception ex)
            {
                // 必要に応じて例外区別をすること

                _logger.Error($"Executer processKey:{processKey},operadA:{operandA},operandB:{operandB},Exception: {ex.ToString()}"); 
                
                // 失敗
                return (false, 0M, ex.Message);
            }
        }

        /// <summary>
        /// 演算子に対応する処理を返す
        /// </summary>
        /// <param name="processKey">演算子</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">指定された演算子に対応する処理が登録されていない場合</exception>
        private IComputeProcess Dispatch(string processKey)
        {
            return _processMap.FirstOrDefault(p => p.Operator == processKey) ?? throw new KeyNotFoundException($"未登録のprocessKey:{processKey}");
        }
    }
}