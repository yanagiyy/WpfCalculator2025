namespace WpfCalculator2025.Model.ComputeProcess
{
    /// <summary>
    /// 乗算
    /// </summary>
    class MultiplicationComputeProcess : IComputeProcess
    {
        public string Operator => "*";

        public decimal Compute(decimal x, decimal y)
        {
            return x * y;
        }
    }
}
