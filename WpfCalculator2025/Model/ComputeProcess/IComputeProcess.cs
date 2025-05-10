namespace WpfCalculator2025.Model.ComputeProcess
{
    interface IComputeProcess
    {
        /// <summary>
        /// 処理対象の演算子
        /// </summary>
        public string Operator { get; }

        /// <summary>
        /// 演算処理
        /// </summary>
        /// <param name="x">被オペラント</param>
        /// <param name="y">オペラント</param>
        /// <returns></returns>
        public decimal Compute(decimal x, decimal y);
    }
}
