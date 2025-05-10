using WpfCalculator2025.Model.ComputeProcess;

namespace WpfCalculator2025.Model
{
    /// <summary>
    /// 演算子に対応する処理を割り当てる機能
    /// </summary>
    class ComputeProcessDispatcher
    {
        private readonly IComputeProcess[] _processMap;

        public ComputeProcessDispatcher()
        {
            _processMap =
            [
                // 処理を追加する場合はIComputeProcessを実装した演算処理を登録してください
                new AddComputeProcess(),
            ];
        }

        /// <summary>
        /// 演算子に対応する処理を返す
        /// </summary>
        /// <param name="processKey">演算子</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">指定された演算子に対応する処理が登録されていない場合</exception>
        public IComputeProcess Dispatch(string processKey)
        {
            // パフォーマンスが気になる場合はDictionary.TryGetValueでの実装に変更すること
            return _processMap.FirstOrDefault(p => p.Operator == processKey) ?? throw new KeyNotFoundException($"未登録のprocessKey:{processKey}");
        }
    }
}