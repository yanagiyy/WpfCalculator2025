namespace WpfCalculator2025.Model.ComputeProcess
{
    /// <summary>
    /// 減算
    /// </summary>
    class SubtractionComputeProcess : IComputeProcess
    {
        public string Operator => "-";

        public decimal Compute(decimal x, decimal y)
        {
            return x - y;
        }
    }
}
